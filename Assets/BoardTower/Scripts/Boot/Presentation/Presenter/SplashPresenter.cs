using System;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Boot.Presentation.Presenter
{
    public sealed class SplashPresenter : IStartable, IDisposable
    {
        private readonly SplashUseCase _splashUseCase;
        private readonly SplashFacade _splashFacade;
        private readonly CompositeDisposable _disposable;

        public SplashPresenter(SplashUseCase splashUseCase, SplashFacade splashFacade)
        {
            _splashUseCase = splashUseCase;
            _splashFacade = splashFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _splashUseCase.transition
                .Subscribe(_splashFacade.FadeAsync)
                .AddTo(_disposable);

            _splashFacade.OnTapAsObservable()
                .Subscribe(_ => _splashUseCase.NotifyTapScreen())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}