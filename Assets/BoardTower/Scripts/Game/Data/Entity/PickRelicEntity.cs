using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Data.Entity;
using BoardTower.Game.Application;

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

        public void Add(RelicVO relic)
        {
            var vos = relics.Append(relic);
            Set(new PickRelicVO(vos));
        }
    }
}