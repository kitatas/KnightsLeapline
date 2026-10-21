using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using FastEnumUtility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class LotRelicEntityTests
    {
        private LotRelicEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new LotRelicEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsNull()
        {
            Assert.That(_entity.value, Is.Null);
        }

        [Test]
        public void Set_WithLotRelicVO_StoresValue()
        {
            var relics = new[] { new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true) };
            var vo = new LotRelicVO(relics);

            _entity.Set(vo);

            Assert.That(_entity.value, Is.EqualTo(vo));
        }

        [Test]
        public void Set_WithEmptyRelics_StoresValue()
        {
            var vo = new LotRelicVO(Enumerable.Empty<RelicVO>());

            _entity.Set(vo);

            Assert.That(_entity.value, Is.EqualTo(vo));
        }

        [Test]
        public void Set_CalledMultipleTimes_OverwritesPreviousValue()
        {
            var first = new LotRelicVO(new[] { new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true) });
            var second = new LotRelicVO(new[] { new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false) });

            _entity.Set(first);
            _entity.Set(second);

            Assert.That(_entity.value, Is.EqualTo(second));
        }

        [Test]
        public void IsEqual_WithSameInstance_ReturnsTrue()
        {
            var vo = new LotRelicVO(Enumerable.Empty<RelicVO>());
            _entity.Set(vo);

            Assert.That(_entity.IsEqual(vo), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentInstance_ReturnsFalse()
        {
            var first = new LotRelicVO(Enumerable.Empty<RelicVO>());
            var second = new LotRelicVO(Enumerable.Empty<RelicVO>());
            _entity.Set(first);

            Assert.That(_entity.IsEqual(second), Is.False);
        }
    }
}
