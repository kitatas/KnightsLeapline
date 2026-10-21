using System;
using System.Linq;
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
using UnityEngine;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class PickRelicUseCaseTests
    {
        private PickRelicUseCase _useCase;
        private LotRelicEntity _lotRelicEntity;
        private PickRelicEntity _pickRelicEntity;
        private FakeAsyncPublisher<PickRelicVO> _publisher;
        private FakeAsyncSubscriber<PickRelicVO> _subscriber;
        private PickRelicPorts _ports;

        [SetUp]
        public void SetUp()
        {
            _lotRelicEntity = new LotRelicEntity();
            _pickRelicEntity = new PickRelicEntity();
            _publisher = new FakeAsyncPublisher<PickRelicVO>();
            _subscriber = new FakeAsyncSubscriber<PickRelicVO>();
            _ports = new PickRelicPorts(_subscriber, _publisher);
            _useCase = new PickRelicUseCase(_lotRelicEntity, _pickRelicEntity, _ports);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void PickRelic_Property_ReturnsPickRelicSubscriber()
        {
            Assert.That(_useCase.pickRelic, Is.EqualTo(_subscriber));
        }

        [Test]
        public async Task InitAsync_PublishesOnce()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.PublishCount, Is.EqualTo(1));
        }

        [Test]
        public async Task InitAsync_SetsPickRelicEntityToEmpty()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_pickRelicEntity.relicTypes, Is.Empty);
        }

        [Test]
        public async Task InitAsync_PublishesEmptyPickRelic()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.That(_publisher.LastPublished.relics, Is.Empty);
        }

        [Test]
        public void HandlePick_WithValidIndex_DoesNotThrow()
        {
            var relics = new[]
            {
                new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true),
                new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false),
            };
            _lotRelicEntity.Set(new LotRelicVO(relics));

            Assert.That(() => _useCase.HandlePick(new SelectRelicVO(0, Vector3.zero)), Throws.Nothing);
        }

        [Test]
        public async Task PickAsync_AfterHandlePick_PublishesUpdatedPickRelic()
        {
            var relics = new[]
            {
                new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true),
                new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false),
            };
            _lotRelicEntity.Set(new LotRelicVO(relics));
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            var pickTask = _useCase.PickAsync(CancellationToken.None).AsTask();
            _useCase.HandlePick(new SelectRelicVO(0, Vector3.zero));

            await pickTask;
            // InitAsync (1回) + PickAsync (1回) = 計2回
            Assert.That(_publisher.PublishCount, Is.EqualTo(2));
            Assert.That(_publisher.LastPublished.relics.Contains(relics[0]), Is.True);
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
