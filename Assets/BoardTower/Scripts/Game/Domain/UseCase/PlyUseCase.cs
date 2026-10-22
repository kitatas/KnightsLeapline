using System;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Repository;
using R3;
using UnityEngine;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class PlyUseCase : IDisposable
    {
        private readonly EventLogEntity _eventLogEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly PlyEntity _plyEntity;
        private readonly RoundRepository _roundRepository;
        private readonly ReactiveProperty<int> _ply;
        private readonly ReactiveProperty<int> _plyMax;

        public PlyUseCase(EventLogEntity eventLogEntity, PickRelicEntity pickRelicEntity, PlyEntity plyEntity,
            RoundRepository roundRepository)
        {
            _eventLogEntity = eventLogEntity;
            _pickRelicEntity = pickRelicEntity;
            _plyEntity = plyEntity;
            _roundRepository = roundRepository;
            _ply = new ReactiveProperty<int>(0);
            _plyMax = new ReactiveProperty<int>(0);
        }

        public Observable<int> ply => _ply;
        public Observable<int> plyMax => _plyMax;

        public void SetUp(int round)
        {
            var plyCount = round > 0 ? _roundRepository.Find(round)?.plyCount ?? 0 : 0;

            if (_pickRelicEntity.FindAny(RelicType.Pact)) plyCount = Mathf.CeilToInt(plyCount / 2.0f);

            _plyEntity.SetUp(plyCount);
            _ply.Value = _plyEntity.value;
            _plyMax.Value = _plyEntity.maxValue;
        }

        public void Add(int value)
        {
            _plyEntity.Add(value);
            _ply.Value = _plyEntity.value;

            _eventLogEntity.Add(EventLogType.Heart, value);
        }

        public void Decrease()
        {
            _plyEntity.Subtract(1);
            _ply.Value = _plyEntity.value;
        }

        public bool IsZero() => _plyEntity.value == 0;

        void IDisposable.Dispose()
        {
            _ply?.Dispose();
            _plyMax?.Dispose();
        }
    }
}