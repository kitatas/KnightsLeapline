using System;
using System.Linq;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using BoardTower.Game.Domain.Repository;
using Cysharp.Threading.Tasks;
using FastEnumUtility;
using MessagePipe;
using R3;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class LotRelicUseCase : IDisposable
    {
        private readonly LotRelicEntity _lotRelicEntity;
        private readonly PickRelicEntity _pickRelicEntity;
        private readonly LotRelicPorts _lotRelicPorts;
        private readonly LocaleRepository _localeRepository;
        private readonly RelicRepository _relicRepository;
        private readonly Subject<LotRelicVO> _lotRelic;

        public LotRelicUseCase(LotRelicEntity lotRelicEntity, PickRelicEntity pickRelicEntity,
            LotRelicPorts lotRelicPorts, LocaleRepository localeRepository, RelicRepository relicRepository)
        {
            _lotRelicEntity = lotRelicEntity;
            _pickRelicEntity = pickRelicEntity;
            _lotRelicPorts = lotRelicPorts;
            _localeRepository = localeRepository;
            _relicRepository = relicRepository;
            _lotRelic = new Subject<LotRelicVO>();
        }

        public IAsyncSubscriber<LotRelicTransitionVO> transition => _lotRelicPorts.lotRelicTransitionSubscriber;
        public Observable<LotRelicVO> lotRelic => _lotRelic;

        public UniTask InitAsync(CancellationToken token)
        {
            var lotRelicTransition = LotRelicTransitionVO.Create(Fade.Out, 0.0f);
            return _lotRelicPorts.PublishLotRelicTransitionAsync(lotRelicTransition, token);
        }

        public UniTask FadeAsync(Fade fade, CancellationToken token)
        {
            var lotRelicTransition = LotRelicTransitionVO.Create(fade, RelicConfig.LOT_FADE_DURATION);
            return _lotRelicPorts.PublishLotRelicTransitionAsync(lotRelicTransition, token);
        }

        public void Lot()
        {
            var rand = new Random();
            var relics = _relicRepository.FindsLotRelics(_pickRelicEntity.relicTypes)
                .OrderBy(_ => rand.Next())
                .Take(RelicConfig.LOT_NUM)
                .Select(Convert)
                .ToArray();

            _lotRelicEntity.Set(new LotRelicVO(relics));
            _lotRelic?.OnNext(_lotRelicEntity.value);
        }

        /// <summary>
        /// Converts a given <see cref="RelicVO"/> into a new instance of <see cref="RelicVO"/>
        /// by transforming its properties using localized data.
        /// </summary>
        /// <param name="relic">The original relic value object to be converted.</param>
        /// <returns>A new instance of <see cref="RelicVO"/> populated with localized data.</returns>
        private RelicVO Convert(RelicVO relic)
        {
            var name = _localeRepository.Get(relic.relicName);
            var content = _localeRepository.Get(relic.content);
            return new RelicVO(relic.type.ToInt32(), name, content, relic.probability, relic.value, relic.isUniq);
        }

        void IDisposable.Dispose()
        {
            _lotRelic?.Dispose();
        }
    }
}