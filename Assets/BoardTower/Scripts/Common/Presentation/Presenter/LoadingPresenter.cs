using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class LoadingPresenter : IInitializable, IDisposable
    {
        private readonly LoadingUseCase _loadingUseCase;
        private readonly LoadingFacade _loadingFacade;
        private readonly CompositeDisposable _disposable;

        public LoadingPresenter(LoadingUseCase loadingUseCase, LoadingFacade loadingFacade)
        {
            _loadingUseCase = loadingUseCase;
            _loadingFacade = loadingFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _loadingUseCase.transition
                .Subscribe(_loadingFacade.FadeAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}