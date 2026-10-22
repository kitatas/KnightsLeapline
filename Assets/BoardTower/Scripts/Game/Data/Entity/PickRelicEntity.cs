using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Data.Entity;
using BoardTower.Game.Application;
using UnityEngine;

namespace BoardTower.Game.Data.Entity
{
    public sealed class PickRelicEntity : BaseEntity<PickRelicVO>
    {
        private IEnumerable<RelicVO> relics
        {
            get
            {
                if (value != null && value.relics.Any())
                {
                    return value.relics;
                }
                else
                {
                    return Enumerable.Empty<RelicVO>();
                }
            }
        }

        public IEnumerable<RelicType> relicTypes => relics.Select(x => x.type);
        public RelicEffectVO effect => RelicEffectVO.Create(relicTypes);

        public IEnumerable<RelicVO> Finds(RelicType type)
        {
            return relics.ToList().FindAll(x => x.type == type);
        }

        public bool FindAny(RelicType type)
        {
            return Finds(type).Any();
        }

        public int GetTotalValue(RelicType type)
        {
            var targets = Finds(type);
            if (targets == null) return 0;

            return targets
                .Where(x => Random.Range(0, 100) < x.probability)
                .Sum(x => x.value);
        }

        public float GetTotalRate(RelicType type)
        {
            var targets = Finds(type);
            if (targets == null) return 1.0f;

            var totalRate = targets
                .Where(x => Random.Range(0, 100) < x.probability)
                .Sum(x => x.value);

            return (100.0f + totalRate) / 100.0f;
        }

        public void Add(RelicVO relic)
        {
            var vos = relics.Append(relic);
            Set(new PickRelicVO(vos));
        }
    }
}