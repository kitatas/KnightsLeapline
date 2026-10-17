using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class BgmPresenter : IInitializable, IDisposable
    {
        private readonly BgmUseCase _bgmUseCase;
        private readonly BgmFacade _bgmFacade;
        private readonly CompositeDisposable _disposable;

        public BgmPresenter(BgmUseCase bgmUseCase, BgmFacade bgmFacade)
        {
            _bgmUseCase = bgmUseCase;
            _bgmFacade = bgmFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _bgmUseCase.play
                .Subscribe(_bgmFacade.Play)
                .AddTo(_disposable);

            _bgmUseCase.stop
                .Subscribe(_ => _bgmFacade.Stop())
                .AddTo(_disposable);

            _bgmUseCase.volume
                .Subscribe(_bgmFacade.SetVolume)
                .AddTo(_disposable);

            _bgmUseCase.isMute
                .Subscribe(_bgmFacade.SetMute)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}