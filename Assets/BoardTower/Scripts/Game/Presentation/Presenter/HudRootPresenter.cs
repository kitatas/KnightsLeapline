using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class HudRootPresenter : IStartable, IDisposable
    {
        private readonly HudRootUseCase _hudRootUseCase;
        private readonly HudRootFacade _hudRootFacade;
        private readonly CompositeDisposable _disposable;

        public HudRootPresenter(HudRootUseCase hudRootUseCase, HudRootFacade hudRootFacade)
        {
            _hudRootUseCase = hudRootUseCase;
            _hudRootFacade = hudRootFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _hudRootUseCase.transition
                .Subscribe(_hudRootFacade.FadeAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}