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
        public void Finds_WhenValueIsNull_ReturnsEmpty()
        {
            var result = _entity.Finds(RelicType.Boots);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Finds_WhenValueIsEmpty_ReturnsEmpty()
        {
            _entity.Set(PickRelicVO.Empty());

            var result = _entity.Finds(RelicType.Boots);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Finds_WhenNoRelicMatches_ReturnsEmpty()
        {
            var relic = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            _entity.Set(new PickRelicVO(new[] { relic }));

            var result = _entity.Finds(RelicType.Charm);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Finds_WhenOneRelicMatches_ReturnsOnlyMatchingRelic()
        {
            var boots = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false);
            _entity.Set(new PickRelicVO(new[] { boots, charm }));

            var result = _entity.Finds(RelicType.Charm).ToArray();

            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0].type, Is.EqualTo(RelicType.Charm));
        }

        [Test]
        public void Finds_WhenMultipleRelicsMatch_ReturnsAllMatchingRelics()
        {
            var boots1 = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false);
            var boots2 = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            _entity.Set(new PickRelicVO(new[] { boots1, charm, boots2 }));

            var result = _entity.Finds(RelicType.Boots).ToArray();

            Assert.That(result, Has.Length.EqualTo(2));
            Assert.That(result.All(x => x.type == RelicType.Boots), Is.True);
        }

        [Test]
        public void Finds_AfterAdd_IncludesAddedRelic()
        {
            _entity.Set(PickRelicVO.Empty());
            _entity.Add(new RelicVO(RelicType.Anchor.ToInt32(), "Anchor", "desc", 0, 0, true));

            var result = _entity.Finds(RelicType.Anchor);

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FindAny_WhenValueIsNull_ReturnsFalse()
        {
            var result = _entity.FindAny(RelicType.Boots);

            Assert.That(result, Is.False);
        }

        [Test]
        public void FindAny_WhenValueIsEmpty_ReturnsFalse()
        {
            _entity.Set(PickRelicVO.Empty());

            var result = _entity.FindAny(RelicType.Boots);

            Assert.That(result, Is.False);
        }

        [Test]
        public void FindAny_WhenNoRelicMatches_ReturnsFalse()
        {
            var relic = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            _entity.Set(new PickRelicVO(new[] { relic }));

            var result = _entity.FindAny(RelicType.Charm);

            Assert.That(result, Is.False);
        }

        [Test]
        public void FindAny_WhenOneRelicMatches_ReturnsTrue()
        {
            var boots = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true);
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false);
            _entity.Set(new PickRelicVO(new[] { boots, charm }));

            var result = _entity.FindAny(RelicType.Charm);

            Assert.That(result, Is.True);
        }

        [Test]
        public void FindAny_WhenProbabilityIsZero_ReturnsTrue()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 50, false);
            _entity.Set(new PickRelicVO(new[] { charm }));

            var result = _entity.FindAny(RelicType.Charm);

            Assert.That(result, Is.True);
        }

        [Test]
        public void FindAny_AfterAdd_ReturnsTrue()
        {
            _entity.Set(PickRelicVO.Empty());
            _entity.Add(new RelicVO(RelicType.Anchor.ToInt32(), "Anchor", "desc", 0, 0, true));

            var result = _entity.FindAny(RelicType.Anchor);

            Assert.That(result, Is.True);
        }

        [Test]
        public void FindAnyProbability_WhenValueIsNull_ReturnsFalse()
        {
            var result = _entity.FindAnyProbability(RelicType.Charm);

            Assert.That(result, Is.False);
        }

        [Test]
        public void FindAnyProbability_WhenNoRelicMatches_ReturnsFalse()
        {
            var boots = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 100, 50, true);
            _entity.Set(new PickRelicVO(new[] { boots }));

            var result = _entity.FindAnyProbability(RelicType.Charm);

            Assert.That(result, Is.False);
        }

        [Test]
        public void FindAnyProbability_WhenProbabilityIsHundred_ReturnsTrue()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 50, false);
            _entity.Set(new PickRelicVO(new[] { charm }));

            var result = _entity.FindAnyProbability(RelicType.Charm);

            Assert.That(result, Is.True);
        }

        [Test]
        public void FindAnyProbability_WhenProbabilityIsZero_ReturnsFalse()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 50, false);
            _entity.Set(new PickRelicVO(new[] { charm }));

            var result = _entity.FindAnyProbability(RelicType.Charm);

            Assert.That(result, Is.False);
        }

        [Test]
        public void FindAnyProbability_WhenOnlyOneRelicHits_ReturnsTrue()
        {
            var hit = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 20, false);
            var miss = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 30, false);
            _entity.Set(new PickRelicVO(new[] { miss, hit }));

            var result = _entity.FindAnyProbability(RelicType.Charm);

            Assert.That(result, Is.True);
        }

        [Test]
        public void FindAnyProbability_WhenOtherTypeRelicHasHundredProbability_ReturnsFalse()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 20, false);
            var chalice = new RelicVO(RelicType.Chalice.ToInt32(), "Chalice", "desc", 100, 40, false);
            _entity.Set(new PickRelicVO(new[] { charm, chalice }));

            var result = _entity.FindAnyProbability(RelicType.Charm);

            Assert.That(result, Is.False);
        }

        [Test]
        public void GetTotalValue_WhenValueIsNull_ReturnsZero()
        {
            var total = _entity.GetTotalValue(RelicType.Charm);

            Assert.That(total, Is.EqualTo(0));
        }

        [Test]
        public void GetTotalValue_WhenNoRelicMatches_ReturnsZero()
        {
            var boots = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 100, 50, true);
            _entity.Set(new PickRelicVO(new[] { boots }));

            var total = _entity.GetTotalValue(RelicType.Charm);

            Assert.That(total, Is.EqualTo(0));
        }

        [Test]
        public void GetTotalValue_WhenProbabilityIsHundred_ReturnsValue()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 50, false);
            _entity.Set(new PickRelicVO(new[] { charm }));

            var total = _entity.GetTotalValue(RelicType.Charm);

            Assert.That(total, Is.EqualTo(50));
        }

        [Test]
        public void GetTotalValue_WhenProbabilityIsZero_ReturnsZero()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 50, false);
            _entity.Set(new PickRelicVO(new[] { charm }));

            var total = _entity.GetTotalValue(RelicType.Charm);

            Assert.That(total, Is.EqualTo(0));
        }

        [Test]
        public void GetTotalValue_WhenMultipleRelicsMatch_SumsValues()
        {
            var charm1 = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 20, false);
            var charm2 = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 30, false);
            _entity.Set(new PickRelicVO(new[] { charm1, charm2 }));

            var total = _entity.GetTotalValue(RelicType.Charm);

            Assert.That(total, Is.EqualTo(50));
        }

        [Test]
        public void GetTotalValue_WhenOnlyHitRelicsExist_SumsOnlyHitRelics()
        {
            var hit = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 20, false);
            var miss = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 30, false);
            _entity.Set(new PickRelicVO(new[] { hit, miss }));

            var total = _entity.GetTotalValue(RelicType.Charm);

            Assert.That(total, Is.EqualTo(20));
        }

        [Test]
        public void GetTotalValue_WhenOtherTypeRelicHasHundredProbability_IgnoresOtherType()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 20, false);
            var chalice = new RelicVO(RelicType.Chalice.ToInt32(), "Chalice", "desc", 100, 40, false);
            _entity.Set(new PickRelicVO(new[] { charm, chalice }));

            var total = _entity.GetTotalValue(RelicType.Charm);

            Assert.That(total, Is.EqualTo(20));
        }

        [Test]
        public void GetTotalRate_WhenValueIsNull_ReturnsOne()
        {
            var rate = _entity.GetTotalRate(RelicType.Charm);

            Assert.That(rate, Is.EqualTo(1.0f));
        }

        [Test]
        public void GetTotalRate_WhenNoRelicMatches_ReturnsOne()
        {
            var boots = new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 100, 50, true);
            _entity.Set(new PickRelicVO(new[] { boots }));

            var rate = _entity.GetTotalRate(RelicType.Charm);

            Assert.That(rate, Is.EqualTo(1.0f));
        }

        [Test]
        public void GetTotalRate_WhenProbabilityIsHundred_AddsValue()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 50, false);
            _entity.Set(new PickRelicVO(new[] { charm }));

            var rate = _entity.GetTotalRate(RelicType.Charm);

            Assert.That(rate, Is.EqualTo(1.5f).Within(0.0001f));
        }

        [Test]
        public void GetTotalRate_WhenProbabilityIsZero_ReturnsOne()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 50, false);
            _entity.Set(new PickRelicVO(new[] { charm }));

            var rate = _entity.GetTotalRate(RelicType.Charm);

            Assert.That(rate, Is.EqualTo(1.0f));
        }

        [Test]
        public void GetTotalRate_WhenMultipleRelicsMatch_SumsValues()
        {
            var charm1 = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 20, false);
            var charm2 = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 30, false);
            _entity.Set(new PickRelicVO(new[] { charm1, charm2 }));

            var rate = _entity.GetTotalRate(RelicType.Charm);

            Assert.That(rate, Is.EqualTo(1.5f).Within(0.0001f));
        }

        [Test]
        public void GetTotalRate_WhenOnlyHitRelicsExist_SumsOnlyHitRelics()
        {
            var hit = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 20, false);
            var miss = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 30, false);
            _entity.Set(new PickRelicVO(new[] { hit, miss }));

            var rate = _entity.GetTotalRate(RelicType.Charm);

            Assert.That(rate, Is.EqualTo(1.2f).Within(0.0001f));
        }

        [Test]
        public void GetTotalRate_WhenOtherTypeRelicHasHundredProbability_IgnoresOtherType()
        {
            var charm = new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 100, 20, false);
            var chalice = new RelicVO(RelicType.Chalice.ToInt32(), "Chalice", "desc", 100, 40, false);
            _entity.Set(new PickRelicVO(new[] { charm, chalice }));

            var rate = _entity.GetTotalRate(RelicType.Charm);

            Assert.That(rate, Is.EqualTo(1.2f).Within(0.0001f));
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
