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
    public sealed class ChessmenPresenter : IStartable, IDisposable
    {
        private readonly SeUseCase _seUseCase;
        private readonly ChessmenUseCase _chessmenUseCase;
        private readonly ChessmenFacade _chessmenFacade;
        private readonly CompositeDisposable _disposable;

        public ChessmenPresenter(SeUseCase seUseCase, ChessmenUseCase chessmenUseCase, ChessmenFacade chessmenFacade)
        {
            _seUseCase = seUseCase;
            _chessmenUseCase = chessmenUseCase;
            _chessmenFacade = chessmenFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _chessmenUseCase.transition
                .Subscribe((t, ct) =>
                {
                    _seUseCase.Play(t.transition.fade.ToChessmenSeType());
                    return _chessmenFacade.FadeAsync(t, ct);
                })
                .AddTo(_disposable);

            _chessmenUseCase.movement
                .Subscribe((m, ct) =>
                {
                    _seUseCase.Play(SeType.Step);
                    return _chessmenFacade.MoveAsync(m, ct);
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}