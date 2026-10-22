using System;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using R3;
using UnityEngine;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class RoundClearUseCase : IDisposable
    {
        private readonly GemEntity _gemEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly RoundClearGemEntity _roundClearGemEntity;
        private readonly RoundRepository _roundRepository;
        private readonly ReactiveProperty<int> _roundClear;

        public RoundClearUseCase(GemEntity gemEntity, PickRelicEntity pickRelicEntity,
            RoundClearGemEntity roundClearGemEntity, RoundRepository roundRepository)
        {
            _gemEntity = gemEntity;
            _pickRelicEntity = pickRelicEntity;
            _roundClearGemEntity = roundClearGemEntity;
            _roundRepository = roundRepository;
            _roundClear = new ReactiveProperty<int>(0);
        }

        public Observable<int> roundClear => _roundClear;

        public void SetUp(int round)
        {
            var gemCount = round > 0 ? _roundRepository.Find(round)?.gemCount ?? 0 : 0;

            if (_pickRelicEntity.FindAny(RelicType.Pact)) gemCount = Mathf.CeilToInt(gemCount / 2.0f);

            _roundClearGemEntity.Set(gemCount);
            _roundClear.Value = _roundClearGemEntity.value;
        }

        public bool IsClear()
        {
            return _roundClearGemEntity.value <= _gemEntity.value;
        }

        public bool IsOverflowClearGem()
        {
            return _roundClearGemEntity.value < _gemEntity.value;
        }

        void IDisposable.Dispose()
        {
            _roundClear?.Dispose();
        }
    }
}