using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using FastEnumUtility;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class PickRelicEntityTests
    {
        private PickRelicEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new PickRelicEntity();
        }

        [Test]
        public void Constructor_InitialValue_IsNull()
        {
            Assert.That(_entity.value, Is.Null);
        }

        [Test]
        public void RelicTypes_WhenValueIsNull_ReturnsEmpty()
        {
            var types = _entity.relicTypes;

            Assert.That(types, Is.Empty);
        }

        [Test]
        public void RelicTypes_WhenValueIsEmpty_ReturnsEmpty()
        {
            _entity.Set(PickRelicVO.Empty());

            var types = _entity.relicTypes;

            Assert.That(types, Is.Empty);
        }

        [Test]
        public void RelicTypes_AfterSet_ReturnsCorrectTypes()
        {
            var relic = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            _entity.Set(new PickRelicVO(new[] { relic }));

            var types = _entity.relicTypes.ToArray();

            Assert.That(types, Has.Length.EqualTo(1));
            Assert.That(types[0], Is.EqualTo(RelicType.Boots));
        }

        [Test]
        public void Effect_WhenValueIsNull_ReturnsDefaultEffect()
        {
            var effect = _entity.effect;

            Assert.That(effect, Is.Not.Null);
            Assert.That(effect.canMoveToBlock, Is.False);
            Assert.That(effect.isIgnoreBelt, Is.False);
        }

        [Test]
        public void Effect_WithBoots_SetsCanMoveToBlockTrue()
        {
            var relic = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            _entity.Set(new PickRelicVO(new[] { relic }));

            var effect = _entity.effect;

            Assert.That(effect.canMoveToBlock, Is.True);
        }

        [Test]
        public void Add_AppendsRelicToCollection()
        {
            _entity.Set(PickRelicVO.Empty());
            var relic = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false);

            _entity.Add(relic);

            Assert.That(_entity.relicTypes.Contains(RelicType.Charm), Is.True);
        }

        [Test]
        public void Add_CalledMultipleTimes_AccumulatesRelics()
        {
            _entity.Set(PickRelicVO.Empty());
            var relic1 = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            var relic2 = new RelicVO(RelicType.Anchor.ToInt32(), "Anchor", "desc", 0, 0, true);

            _entity.Add(relic1);
            _entity.Add(relic2);

            var types = _entity.relicTypes.ToArray();
            Assert.That(types.Length, Is.EqualTo(2));
        }

        [Test]
        public void Set_WithPickRelicVO_StoresCorrectValue()
        {
            var relics = new[]
            {
                new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true),
                new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false),
            };
            var vo = new PickRelicVO(relics);

            _entity.Set(vo);

            Assert.That(_entity.relicTypes.Count(), Is.EqualTo(2));
        }
    }
}
