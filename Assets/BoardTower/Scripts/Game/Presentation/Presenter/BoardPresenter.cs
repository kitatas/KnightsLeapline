using System;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class BoardPresenter : IStartable, IDisposable
    {
        private readonly SeUseCase _seUseCase;
        private readonly BoardUseCase _boardUseCase;
        private readonly MovementUseCase _movementUseCase;
        private readonly BoardFacade _boardFacade;
        private readonly CompositeDisposable _disposable;

        public BoardPresenter(SeUseCase seUseCase, BoardUseCase boardUseCase, MovementUseCase movementUseCase,
            BoardFacade boardFacade)
        {
            _seUseCase = seUseCase;
            _boardUseCase = boardUseCase;
            _movementUseCase = movementUseCase;
            _boardFacade = boardFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _boardUseCase.transition
                .Subscribe((t, ct) => _boardFacade.FadeAsync(t, ct))
                .AddTo(_disposable);

            _boardUseCase.renderEvent
                .Subscribe((e, ct) =>
                {
                    if (e.render is not RenderType.Refresh) _seUseCase.Play(e.render.ToSeType());
                    return _boardFacade.RenderEventAsync(e, ct);
                })
                .AddTo(_disposable);

            _movementUseCase.highlights
                .Subscribe((hs, ct) => _boardFacade.ShowHighlightAsync(hs, ct))
                .AddTo(_disposable);

            _boardFacade.OnClickAnySquareAsObservable()
                .Subscribe(s => _movementUseCase.HandleClick(s))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}