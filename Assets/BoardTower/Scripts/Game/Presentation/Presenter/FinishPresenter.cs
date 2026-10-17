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
    public sealed class FinishPresenter : IStartable, IDisposable
    {
        private readonly BgmUseCase _bgmUseCase;
        private readonly SeUseCase _seUseCase;
        private readonly FinishUseCase _finishUseCase;
        private readonly FinishFacade _finishFacade;
        private readonly CompositeDisposable _disposable;

        public FinishPresenter(BgmUseCase bgmUseCase, SeUseCase seUseCase, FinishUseCase finishUseCase,
            FinishFacade finishFacade)
        {
            _bgmUseCase = bgmUseCase;
            _seUseCase = seUseCase;
            _finishUseCase = finishUseCase;
            _finishFacade = finishFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _finishUseCase.finishTransition
                .Subscribe((f, ct) =>
                {
                    if (f.transition.fade is Fade.In)
                    {
                        _bgmUseCase.Stop();
                        _seUseCase.Play(f.type.ToSeType());
                    }

                    return _finishFacade.FadeAsync(f, ct);
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}