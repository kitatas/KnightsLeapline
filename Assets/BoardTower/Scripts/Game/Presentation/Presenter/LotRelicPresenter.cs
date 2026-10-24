using System;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class LotRelicPresenter : IStartable, IDisposable
    {
        private readonly SeUseCase _seUseCase;
        private readonly LotRelicUseCase _lotRelicUseCase;
        private readonly SelectRelicUseCase _selectRelicUseCase;
        private readonly LotRelicFacade _lotRelicFacade;
        private readonly CompositeDisposable _disposable;

        public LotRelicPresenter(SeUseCase seUseCase, LotRelicUseCase lotRelicUseCase,
            SelectRelicUseCase selectRelicUseCase, LotRelicFacade lotRelicFacade)
        {
            _seUseCase = seUseCase;
            _lotRelicUseCase = lotRelicUseCase;
            _selectRelicUseCase = selectRelicUseCase;
            _lotRelicFacade = lotRelicFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _lotRelicUseCase.lotRelic
                .Subscribe(_lotRelicFacade.Render)
                .AddTo(_disposable);

            _lotRelicUseCase.transition
                .Subscribe((l, ct) =>
                {
                    if (l.transition.duration > 0)
                        _seUseCase.PlayOneShots(SeType.PopLog, RelicConfig.LOT_NUM, RelicConfig.LOT_DELAY_RATE);
                    return _lotRelicFacade.FadeAsync(l, ct);
                })
                .AddTo(_disposable);

            _lotRelicFacade.OnClickAnyAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(0.1f))
                .Subscribe(x =>
                {
                    _seUseCase.Play(SeType.Decision);
                    _selectRelicUseCase.Select(x);
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}