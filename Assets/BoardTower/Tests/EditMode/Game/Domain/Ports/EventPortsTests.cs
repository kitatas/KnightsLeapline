using System;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class EventPortsTests
    {
        private EventPorts _ports;
        private FakeAsyncPublisher<ChessmenMovementVO> _movementPublisher;
        private FakeAsyncPublisher<RenderEventSquareVO> _renderEventSquarePublisher;

        [SetUp]
        public void SetUp()
        {
            _movementPublisher = new FakeAsyncPublisher<ChessmenMovementVO>();
            _renderEventSquarePublisher = new FakeAsyncPublisher<RenderEventSquareVO>();

            _ports = new EventPorts(_movementPublisher, _renderEventSquarePublisher);
        }

        [Test]
        public async Task PublishChessmenMovementAsync_CallsPublisherOnce()
        {
            var vo = new ChessmenMovementVO(new SquareVO(1, 1));

            await _ports.PublishChessmenMovementAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_movementPublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishChessmenMovementAsync_PublishesExactVO()
        {
            var vo = new ChessmenMovementVO(new SquareVO(3, 5));

            await _ports.PublishChessmenMovementAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_movementPublisher.LastPublished, Is.EqualTo(vo));
        }

        [Test]
        public async Task PublishChessmenMovementAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = new ChessmenMovementVO(new SquareVO(1, 1));

            await _ports.PublishChessmenMovementAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishChessmenMovementAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_movementPublisher.PublishCount, Is.EqualTo(2));
        }

        [Test]
        public async Task PublishEventSquaresAsync_CallsPublisherOnce()
        {
            var vo = new RenderEventSquareVO(RenderType.RetainUp, Array.Empty<EventSquareVO>());

            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_renderEventSquarePublisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishEventSquaresAsync_PublishesExactVO()
        {
            var vo = new RenderEventSquareVO(RenderType.Refresh, Array.Empty<EventSquareVO>());

            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_renderEventSquarePublisher.LastPublished, Is.EqualTo(vo));
        }

        [TestCase(RenderType.Refresh)]
        [TestCase(RenderType.RetainUp)]
        [TestCase(RenderType.RetainDown)]
        public async Task PublishEventSquaresAsync_PublishesCorrectRenderType(RenderType render)
        {
            var vo = new RenderEventSquareVO(render, Array.Empty<EventSquareVO>());

            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_renderEventSquarePublisher.LastPublished.render, Is.EqualTo(render));
        }

        [Test]
        public async Task PublishEventSquaresAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = new RenderEventSquareVO(RenderType.RetainUp, Array.Empty<EventSquareVO>());

            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishEventSquaresAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_renderEventSquarePublisher.PublishCount, Is.EqualTo(2));
        }
    }
}
