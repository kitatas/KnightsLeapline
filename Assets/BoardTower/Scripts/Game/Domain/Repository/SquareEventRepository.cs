using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class SquareEventRepository
    {
        private readonly Dictionary<SquareEventType, SquareEventVO> _squareEventMap;

        public SquareEventRepository(SquareEventTable squareEventTable)
        {
            _squareEventMap = squareEventTable.all.ToDictionary(x => x.type, x => x.ToVO());
        }

        public SquareEventVO Find(SquareEventType type)
        {
            return _squareEventMap.TryGetValue(type, out var vo)
                ? vo
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT);
        }
    }
}