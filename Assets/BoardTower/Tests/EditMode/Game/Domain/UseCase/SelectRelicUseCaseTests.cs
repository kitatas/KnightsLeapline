using System;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class SelectRelicUseCaseTests
    {
        private SelectRelicUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _useCase = new SelectRelicUseCase();
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void SelectRelic_Observable_IsNotNull()
        {
            Assert.That(_useCase.selectRelic, Is.Not.Null);
        }

        [Test]
        public void DecisionRelic_Observable_IsNotNull()
        {
            Assert.That(_useCase.decisionRelic, Is.Not.Null);
        }

        [Test]
        public void HideHighlight_Observable_IsNotNull()
        {
            Assert.That(_useCase.hideHighlight, Is.Not.Null);
        }

        [Test]
        public void Select_FirstSelection_EmitsOnSelectRelic()
        {
            SelectRelicVO observed = null;
            _useCase.selectRelic.Subscribe(x => observed = x);

            _useCase.Select(new SelectRelicVO(0, Vector3.zero));

            Assert.That(observed, Is.Not.Null);
            Assert.That(observed.index, Is.EqualTo(0));
        }

        [Test]
        public void Select_FirstSelection_DoesNotEmitOnDecisionRelic()
        {
            SelectRelicVO observed = null;
            _useCase.decisionRelic.Subscribe(x => observed = x);

            _useCase.Select(new SelectRelicVO(0, Vector3.zero));

            Assert.That(observed, Is.Null);
        }

        [Test]
        public void Select_SameIndexTwice_EmitsOnDecisionRelic()
        {
            SelectRelicVO observed = null;
            _useCase.decisionRelic.Subscribe(x => observed = x);

            _useCase.Select(new SelectRelicVO(1, Vector3.zero));
            _useCase.Select(new SelectRelicVO(1, Vector3.zero));

            Assert.That(observed, Is.Not.Null);
            Assert.That(observed.index, Is.EqualTo(1));
        }

        [Test]
        public void Select_SameIndexTwice_ClearsSelectRelic()
        {
            var emitCount = 0;
            // 初回選択: 1回目のOnNext
            // 2回目同じindex選択: null→フィルタで除外
            _useCase.selectRelic.Subscribe(_ => emitCount++);

            _useCase.Select(new SelectRelicVO(0, Vector3.zero));
            _useCase.Select(new SelectRelicVO(0, Vector3.zero));

            // 1回目のみemit（2回目はnullになりフィルタで除外）
            Assert.That(emitCount, Is.EqualTo(1));
        }

        [Test]
        public void Select_DifferentIndex_ChangesSelectRelic()
        {
            var lastIndex = -1;
            _useCase.selectRelic.Subscribe(x => lastIndex = x.index);

            _useCase.Select(new SelectRelicVO(0, Vector3.zero));
            _useCase.Select(new SelectRelicVO(1, Vector3.zero));

            Assert.That(lastIndex, Is.EqualTo(1));
        }

        [Test]
        public void Select_DifferentIndex_DoesNotEmitOnDecisionRelic()
        {
            SelectRelicVO observed = null;
            _useCase.decisionRelic.Subscribe(x => observed = x);

            _useCase.Select(new SelectRelicVO(0, Vector3.zero));
            _useCase.Select(new SelectRelicVO(1, Vector3.zero));

            Assert.That(observed, Is.Null);
        }

        [Test]
        public void Select_AfterDecision_NewFirstSelection_EmitsOnSelectRelic()
        {
            var selectCount = 0;
            _useCase.selectRelic.Subscribe(_ => selectCount++);

            _useCase.Select(new SelectRelicVO(0, Vector3.zero));
            _useCase.Select(new SelectRelicVO(0, Vector3.zero));
            _useCase.Select(new SelectRelicVO(2, Vector3.zero));

            Assert.That(selectCount, Is.EqualTo(2));
        }

        [Test]
        public void HideHighlight_OnSubscribe_EmitsInitialValue()
        {
            var emitCount = 0;

            // BehaviorSubjectのため購読時に初期値が流れる
            _useCase.hideHighlight.Subscribe(_ => emitCount++);

            Assert.That(emitCount, Is.EqualTo(1));
        }

        [Test]
        public void Hide_EmitsOnHideHighlight()
        {
            var emitCount = 0;
            _useCase.hideHighlight.Subscribe(_ => emitCount++);
            emitCount = 0; // 購読時の初期値をリセット

            _useCase.Hide();

            Assert.That(emitCount, Is.EqualTo(1));
        }

        [Test]
        public void Select_SameIndexTwice_EmitsOnHideHighlight()
        {
            var emitCount = 0;
            _useCase.hideHighlight.Subscribe(_ => emitCount++);
            emitCount = 0; // 購読時の初期値をリセット

            _useCase.Select(new SelectRelicVO(1, Vector3.zero));
            _useCase.Select(new SelectRelicVO(1, Vector3.zero));

            Assert.That(emitCount, Is.EqualTo(1));
        }

        [Test]
        public void Select_FirstSelection_DoesNotEmitOnHideHighlight()
        {
            var emitCount = 0;
            _useCase.hideHighlight.Subscribe(_ => emitCount++);
            emitCount = 0; // 購読時の初期値をリセット

            _useCase.Select(new SelectRelicVO(0, Vector3.zero));

            Assert.That(emitCount, Is.EqualTo(0));
        }

        [Test]
        public void Select_DifferentIndex_DoesNotEmitOnHideHighlight()
        {
            var emitCount = 0;
            _useCase.hideHighlight.Subscribe(_ => emitCount++);
            emitCount = 0; // 購読時の初期値をリセット

            _useCase.Select(new SelectRelicVO(0, Vector3.zero));
            _useCase.Select(new SelectRelicVO(1, Vector3.zero));

            Assert.That(emitCount, Is.EqualTo(0));
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.That(() => ((IDisposable)_useCase).Dispose(), Throws.Nothing);
        }
    }
}
