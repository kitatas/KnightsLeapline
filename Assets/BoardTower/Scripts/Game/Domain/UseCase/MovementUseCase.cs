using System;
using System.Linq;
using System.Threading;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using BoardTower.Game.Utility;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class MovementUseCase : IDisposable
    {
        private readonly BoardEntity _boardEntity;
        private readonly ChessmenEntity _chessmenEntity;
        private readonly GameStateEntity _gameStateEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly MovementPorts _movementPorts;
        private readonly ChessmenMovementRepository _chessmenMovementRepository;
        private readonly Subject<ClickSquareVO> _movement;

        private HighlightSquareVO[] _lastHighlights;

        public MovementUseCase(BoardEntity boardEntity, ChessmenEntity chessmenEntity, GameStateEntity gameStateEntity,
            PickRelicEntity pickRelicEntity, MovementPorts movementPorts,
            ChessmenMovementRepository chessmenMovementRepository)
        {
            _boardEntity = boardEntity;
            _chessmenEntity = chessmenEntity;
            _gameStateEntity = gameStateEntity;
            _pickRelicEntity = pickRelicEntity;
            _movementPorts = movementPorts;
            _chessmenMovementRepository = chessmenMovementRepository;
            _movement = new Subject<ClickSquareVO>();
            _lastHighlights = Array.Empty<HighlightSquareVO>();
        }

        public IAsyncSubscriber<HighlightSquareVO[]> highlights => _movementPorts.highlightsSubscriber;

        public async UniTask PublishMovableSquaresAsync(CancellationToken token)
        {
            var rule = _chessmenMovementRepository.Find(_chessmenEntity.chessmenType);
            var highlightVos = ChessmenHelper.GetMovableSquares(_chessmenEntity.square, rule)
                .Where(x => _boardEntity.IsMovable(x, _pickRelicEntity.FindAny(RelicType.Boots)))
                .Select(x => new HighlightSquareVO(x, HighlightSquareType.Movable))
                .ToArray();

            await _movementPorts.PublishHighlightSquaresAsync(highlightVos, token);
            _lastHighlights = highlightVos;
        }

        public void HandleClick(ClickSquareVO clickSquare)
        {
            // GameState.Input 以外は処理させない
            if (!_gameStateEntity.IsEqual(GameState.Input)) return;

            // 移動可能なマスが更新されていない場合は処理させない
            if (_lastHighlights.Length == 0) return;

            // 移動可能範囲外であれば処理させない
            if (!_lastHighlights.Any(x => x.square.IsEqual(clickSquare.square))) return;

            _movement?.OnNext(clickSquare);
        }

        public UniTask ClearHighlightSquareAsync(CancellationToken token)
        {
            var highlightVos = _lastHighlights
                .Select(x => new HighlightSquareVO(x.square, HighlightSquareType.Default))
                .ToArray();

            _lastHighlights = Array.Empty<HighlightSquareVO>();
            return _movementPorts.PublishHighlightSquaresAsync(highlightVos, token);
        }

        public async UniTask InputAsync(CancellationToken token)
        {
            var clickSquare = await _movement.FirstAsync(cancellationToken: token);
            _chessmenEntity.Set(clickSquare.square);
        }

        public UniTask MoveAsync(CancellationToken token)
        {
            return _movementPorts.PublishChessmenMovementAsync(_chessmenEntity.movement, token);
        }

        void IDisposable.Dispose()
        {
            _movement?.Dispose();
        }
    }
}