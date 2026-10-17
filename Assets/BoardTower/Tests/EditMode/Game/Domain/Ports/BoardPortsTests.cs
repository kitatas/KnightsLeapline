using System.Threading;
using System.Threading.Tasks;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class BoardPortsTests
    {
        private BoardPorts _ports;
        private FakeAsyncPublisher<BoardTransitionVO> _boardTransitionPublisher;
        private FakeAsyncSubscriber<BoardTransitionVO> _boardTransitionSubscriber;
        private FakeAsyncPublisher<RenderEventSquareVO> _renderEventSquarePublisher;
        private FakeAsyncSubscriber<RenderEventSquareVO> _renderEventSquareSubscriber;

        [SetUp]
        public void SetUp()
        {
            _boardTransitionPublisher = new FakeAsyncPublisher<BoardTransitionVO>();
            _boardTransitionSubscriber = new FakeAsyncSubscriber<BoardTransitionVO>();
            _renderEventSquarePublisher = new FakeAsyncPublisher<RenderEventSquareVO>();
            _renderEventSquareSubscriber = new FakeAsyncSubscriber<RenderEventSquareVO>();

            _ports = new BoardPorts(
                _boardTransitionSubscriber,
                _renderEventSquareSubscriber,
                _boardTransitionPublisher,
                _renderEventSquarePublisher);
        }

        [Test]
        public void Constructor_AssignsBoardTransitionSubscriber()
        {
            Assert.That(_ports.boardTransitionSubscriber, Is.EqualTo(_boardTransitionSubscriber));
        }

        [Test]
        public void Constructor_AssignsRenderEventSquareSubscriber()
        {
            Assert.That(_ports.renderEventSquareSubscriber, Is.EqualTo(_renderEventSquareSubscriber));
        }

        [Test]
        public async Task PublishBoardTransitionAsync_CallsPublisherOnce()
        {
            var vo = BoardTransitionVO.Create(Fade.Out);

            await _ports.PublishBoardTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_boardTransitionPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishBoardTransitionAsync_PublishesExactVO()
        {
            var vo = BoardTransitionVO.Create(Fade.Out);

            await _ports.PublishBoardTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_boardTransitionPublisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public async Task PublishBoardTransitionAsync_PublishesCorrectFade(Fade fade)
        {
            var vo = BoardTransitionVO.Create(fade);

            await _ports.PublishBoardTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_boardTransitionPublisher.LastPublished.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public async Task PublishBoardTransitionAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = BoardTransitionVO.Create(Fade.Out);

            await _ports.PublishBoardTransitionAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishBoardTransitionAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_boardTransitionPublisher.PublishCount, Is.EqualTo(2));
        }

        [Test]
        public async Task PublishEventSquaresAsync_CallsPublisherOnce()
        {
            var vo = new RenderEventSquareVO(RenderType.Refresh, System.Array.Empty<EventSquareVO>());

            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_renderEventSquarePublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishEventSquaresAsync_PublishesExactVO()
        {
            var vo = new RenderEventSquareVO(RenderType.Refresh, System.Array.Empty<EventSquareVO>());

            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_renderEventSquarePublisher.LastPublished, Is.EqualTo(vo));
        }

        [Test]
        public async Task PublishEventSquaresAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = new RenderEventSquareVO(RenderType.RetainUp, System.Array.Empty<EventSquareVO>());

            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_renderEventSquarePublisher.PublishCount, Is.EqualTo(2));
        }
    }
}
