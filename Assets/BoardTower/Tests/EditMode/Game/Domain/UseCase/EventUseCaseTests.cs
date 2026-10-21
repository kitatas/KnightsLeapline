using System.Threading;
using System.Threading.Tasks;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using FastEnumUtility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class EventUseCaseTests
    {
        private EventUseCase _useCase;
        private BoardEntity _boardEntity;
        private ChessmenEntity _chessmenEntity;
        private PickRelicEntity _pickRelicEntity;
        private FakeAsyncPublisher<ChessmenMovementVO> _movementPublisher;
        private FakeAsyncPublisher<RenderEventSquareVO> _renderEventSquarePublisher;
        private EventPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _boardEntity = new BoardEntity();
            _chessmenEntity = new ChessmenEntity();
            _pickRelicEntity = new PickRelicEntity();
            _movementPublisher = new FakeAsyncPublisher<ChessmenMovementVO>();
            _renderEventSquarePublisher = new FakeAsyncPublisher<RenderEventSquareVO>();
            _ports = new EventPorts(_movementPublisher, _renderEventSquarePublisher);

            // SquareEventRepository は MasterMemory 依存のため null を渡す
            // Empty / Belt イベントではリポジトリを呼ばないため null で動作可
            _useCase = new EventUseCase(_boardEntity, _chessmenEntity, _pickRelicEntity, _ports, null);
        }

        [Test]
        public async Task ApplyEventAsync_WithEmptySquare_ReturnsEmptyType()
        {
            // Empty マスを駒の現在位置に設定
            var square = _chessmenEntity.square;
            _boardEntity.Add(EventSquareVO.Create(square.file, square.rank, new SquareEventVO(SquareEventType.Empty, null)));
            _pickRelicEntity.Set(PickRelicVO.Empty());

            var result = await _useCase.ApplyEventAsync(CancellationToken.None).AsTask();

            Assert.That(result.type, Is.EqualTo(SquareEventType.Empty));
        }

        [Test]
        public async Task ApplyEventAsync_WithEmptySquare_GemAndPlyNumAreZero()
        {
            var square = _chessmenEntity.square;
            _boardEntity.Add(EventSquareVO.Create(square.file, square.rank, new SquareEventVO(SquareEventType.Empty, null)));
            _pickRelicEntity.Set(PickRelicVO.Empty());

            var result = await _useCase.ApplyEventAsync(CancellationToken.None).AsTask();

            Assert.That(result.gemNum, Is.EqualTo(0));
            Assert.That(result.plyNum, Is.EqualTo(0));
        }

        [Test]
        public async Task ApplyEventAsync_WithEmptySquare_IsBeltIsFalse()
        {
            var square = _chessmenEntity.square;
            _boardEntity.Add(EventSquareVO.Create(square.file, square.rank, new SquareEventVO(SquareEventType.Empty, null)));
            _pickRelicEntity.Set(PickRelicVO.Empty());

            var result = await _useCase.ApplyEventAsync(CancellationToken.None).AsTask();

            Assert.That(result.isBelt, Is.False);
        }

        [Test]
        public async Task ApplyEventAsync_WithBlockAndCanMoveToBlockRelic_ReturnsBlockType()
        {
            // Boots レリックで Block マスに侵入可能
            var square = _chessmenEntity.square;
            _boardEntity.Add(EventSquareVO.Create(square.file, square.rank, new SquareEventVO(SquareEventType.Block, null)));
            var relic = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            _pickRelicEntity.Set(PickRelicVO.Empty());
            _pickRelicEntity.Add(relic);

            var result = await _useCase.ApplyEventAsync(CancellationToken.None).AsTask();

            Assert.That(result.type, Is.EqualTo(SquareEventType.Block));
        }

        [Test]
        public async Task ApplyEventAsync_WithBeltAndIgnoreBeltRelic_IsBeltIsFalse()
        {
            // Anchor レリックでベルトを無効化
            var square = _chessmenEntity.square;
            _boardEntity.Add(EventSquareVO.Create(square.file, square.rank, new SquareEventVO(SquareEventType.BeltRight, null)));
            var relic = new RelicVO(RelicType.Anchor.ToInt32(), "Anchor", "desc", 0, 0, true);
            _pickRelicEntity.Set(PickRelicVO.Empty());
            _pickRelicEntity.Add(relic);

            var result = await _useCase.ApplyEventAsync(CancellationToken.None).AsTask();

            Assert.That(result.isBelt, Is.False);
        }
    }
}
