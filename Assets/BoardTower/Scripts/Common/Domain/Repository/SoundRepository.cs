using System;
using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;

namespace BoardTower.Common.Domain.Repository
{
    public sealed class SoundRepository : ISoundRepository
    {
        private readonly Dictionary<BgmType, BgmAudioVO> _bgmMap;
        private readonly Dictionary<SeType, SeAudioVO> _seMap;

        public SoundRepository(BgmTable bgmTable, SeTable seTable)
        {
            _bgmMap = bgmTable.all.ToDictionary(x => x.type, x => x.ToVO());
            _seMap = seTable.all.ToDictionary(x => x.type, x => x.ToVO());
        }

        public BgmAudioVO Find(BgmType type)
        {
            return _bgmMap.TryGetValue(type, out var vo)
                ? vo
                : throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_BGM);
        }

        public SeAudioVO Find(SeType type)
        {
            return _seMap.TryGetValue(type, out var vo)
                ? vo
                : throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_SE);
        }

        public AudioVO<T> Find<T>(T type) where T : Enum
        {
            return type switch
            {
                BgmType bgm => Find(bgm) as AudioVO<T>,
                SeType se => Find(se) as AudioVO<T>,
                _ => throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_SOUND_TYPE),
            };
        }
    }
}