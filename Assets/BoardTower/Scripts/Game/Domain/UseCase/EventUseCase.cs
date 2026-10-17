using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Utility;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using BoardTower.Game.Utility;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class EventUseCase
    {
        private readonly BoardEntity _boardEntity;
        private readonly ChessmenEntity _chessmenEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly EventPorts _eventPorts;
        private readonly SquareEventRepository _squareEventRepository;

        public EventUseCase(BoardEntity boardEntity, ChessmenEntity chessmenEntity, PickRelicEntity pickRelicEntity,
            EventPorts eventPorts, SquareEventRepository squareEventRepository)
        {
            _boardEntity = boardEntity;
            _chessmenEntity = chessmenEntity;
            _pickRelicEntity = pickRelicEntity;
            _eventPorts = eventPorts;
            _squareEventRepository = squareEventRepository;
        }

        public async UniTask<EventResultVO> ApplyEventAsync(CancellationToken token)
        {
            var relicEffect = _pickRelicEntity.effect;

            var (squareEvent, index) = _boardEntity.FindEvent(_chessmenEntity.square);
            await (squareEvent.type switch
            {
                SquareEventType.Empty => UniTask.Yield(token),
                SquareEventType.Block => relicEffect.canMoveToBlock ? UniTask.Yield(token) : BeltBlockAsync(token),
                SquareEventType.Gem => OverrideSquareEventAsync(index, SquareEventType.Empty, token),
                SquareEventType.Ply => OverrideSquareEventAsync(index, SquareEventType.Empty, token),
                SquareEventType.Collapse => relicEffect.isIgnoreCollapse ? UniTask.Yield(token) : OverrideSquareEventAsync(index, SquareEventType.Block, token),
                var type when type.IsBeltEvent() => relicEffect.isIgnoreBelt ? UniTask.Yield(token) : BeltAsync(squareEvent.type, token),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT),
            });

            return EventResultVO.Create(squareEvent.type, relicEffect);
        }

        private UniTask BeltAsync(SquareEventType type, CancellationToken token)
        {
            return BeltMovementAsync(new[] { type }, token);
        }

        private UniTask BeltBlockAsync(CancellationToken token)
        {
            return BeltMovementAsync(BoardConfig.BELTS.CopyShuffle(), token);
        }

        private UniTask BeltMovementAsync(SquareEventType[] types, CancellationToken token)
        {
            foreach (var type in types)
            {
                var offset = type.ToBeltOffset();
                var square = _chessmenEntity.CalcMovementOffset(offset);
                if (BoardHelper.IsOutOfBoard(square.file, square.rank)) continue;

                _chessmenEntity.Set(square);
                break;
            }

            return _eventPorts.PublishChessmenMovementAsync(_chessmenEntity.movement, token);
        }

        private UniTask OverrideSquareEventAsync(int index, SquareEventType type, CancellationToken token)
        {
            _boardEntity.Set(index, _squareEventRepository.Find(type));
            var retain = type is SquareEventType.Empty ? RenderType.RetainUp : RenderType.RetainDown;
            return _eventPorts.PublishEventSquaresAsync(_boardEntity.GetRenderEventSquare(retain), token);
        }
    }
}