using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class GiveUpPresenter : IAsyncStartable, IDisposable
    {
        private readonly GameModalUseCase _gameModalUseCase;
        private readonly GameStateUseCase _gameStateUse;
        private readonly GiveUpFacade _giveUpFacade;
        private readonly CompositeDisposable _disposable;

        public GiveUpPresenter(GameModalUseCase gameModalUseCase, GameStateUseCase gameStateUse,
            GiveUpFacade giveUpFacade)
        {
            _gameModalUseCase = gameModalUseCase;
            _gameStateUse = gameStateUse;
            _giveUpFacade = giveUpFacade;
            _disposable = new CompositeDisposable();
        }

        UniTask IAsyncStartable.StartAsync(CancellationToken token)
        {
            _giveUpFacade.clickDecision
                .Subscribe(_ => _gameStateUse.ForceChange(GameState.Finish))
                .AddTo(_disposable);

            _giveUpFacade.clickBackTop
                .Subscribe(_ =>
                {
                    var modal = new GameModalVO(GameModalType.GiveUpConfirm, Fade.Out);
                    _gameModalUseCase.FadeAsync(modal, token);
                })
                .AddTo(_disposable);

            return default;
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}