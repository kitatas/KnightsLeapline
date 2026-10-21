using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.Ports;
using BoardTower.Tests.EditMode.TestHelpers;
using Cysharp.Threading.Tasks;
using FastEnumUtility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class PickRelicPortsTests
    {
        private PickRelicPorts _ports;
        private FakeAsyncPublisher<PickRelicVO> _publisher;
        private FakeAsyncSubscriber<PickRelicVO> _subscriber;

        [SetUp]
        public void SetUp()
        {
            _publisher = new FakeAsyncPublisher<PickRelicVO>();
            _subscriber = new FakeAsyncSubscriber<PickRelicVO>();
            _ports = new PickRelicPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_AssignsPickRelicSubscriber()
        {
            Assert.That(_ports.pickRelicSubscriber, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task PublishPickRelicAsync_CallsPublisherOnce()
        {
            var vo = PickRelicVO.Empty();

            await _ports.PublishPickRelicAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task PublishPickRelicAsync_PublishesExactVO()
        {
            var vo = PickRelicVO.Empty();

            await _ports.PublishPickRelicAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished, Is.EqualTo(vo));
        }

        [Test]
        public async Task PublishPickRelicAsync_PublishesVOWithCorrectRelics()
        {
            var relics = new[]
            {
                new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true),
            };
            var vo = new PickRelicVO(relics);

            await _ports.PublishPickRelicAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.relics.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task PublishPickRelicAsync_CalledMultipleTimes_IncrementsCount()
        {
            var vo = PickRelicVO.Empty();

            await _ports.PublishPickRelicAsync(vo, CancellationToken.None).AsTask();
            await _ports.PublishPickRelicAsync(vo, CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
        }
    }
}
