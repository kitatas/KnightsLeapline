using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using BoardTower.Game.Data.DataStore.Tables;

namespace BoardTower.Game.Domain.Repository
{
    public sealed class RoundRepository
    {
        private readonly RoundMasterTable _roundMasterTable;

        public RoundRepository(MemoryDatabase memoryDatabase)
        {
            _roundMasterTable = memoryDatabase.RoundMasterTable;
        }

        public RoundVO Find(int round)
        {
            return _roundMasterTable.TryFindByRound(round, out var master)
                ? master.ToVO()
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_ROUND);
        }
    }
}