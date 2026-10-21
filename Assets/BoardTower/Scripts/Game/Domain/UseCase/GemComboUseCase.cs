using System;
using System.Linq;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using R3;
using Random = UnityEngine.Random;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GemComboUseCase : IDisposable
    {
        private readonly EventLogEntity _eventLogEntity;
        private readonly GemComboEntity _gemComboEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly ReactiveProperty<int> _combo;

        public GemComboUseCase(EventLogEntity eventLogEntity, GemComboEntity gemComboEntity,
            PickRelicEntity pickRelicEntity)
        {
            _eventLogEntity = eventLogEntity;
            _gemComboEntity = gemComboEntity;
            _pickRelicEntity = pickRelicEntity;
            _combo = new ReactiveProperty<int>(0);
        }

        public ReadOnlyReactiveProperty<int> combo => _combo;

        public void SetUp()
        {
            _gemComboEntity.Reset();
            _combo.Value = _gemComboEntity.value;
        }

        public void Increment()
        {
            _gemComboEntity.Add(1);
            _combo.Value = _gemComboEntity.value;
        }

        public void Apply(SquareEventType type)
        {
            if (type is SquareEventType.Gem)
            {
                Increment();
                _eventLogEntity.Add(EventLogType.Combo, _gemComboEntity.value);
            }
            else if (IsComboContinuation())
            {
                _eventLogEntity.Add(EventLogType.Keep, _gemComboEntity.value);
            }
            else
            {
                SetUp();
            }
        }

        private bool IsComboContinuation()
        {
            var relics = _pickRelicEntity.Finds(RelicType.Medal);
            if (relics == null) return false;

            return relics
                .Any(x => Random.Range(0, 100) < x.probability);
        }

        void IDisposable.Dispose()
        {
            _combo?.Dispose();
        }
    }
}