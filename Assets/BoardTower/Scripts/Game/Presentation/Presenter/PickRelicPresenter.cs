using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class PickRelicPresenter : IStartable, IDisposable
    {
        private readonly PickRelicUseCase _pickRelicUseCase;
        private readonly PickRelicFacade _pickRelicFacade;
        private readonly CompositeDisposable _disposable;

        public PickRelicPresenter(PickRelicUseCase pickRelicUseCase, PickRelicFacade pickRelicFacade)
        {
            _pickRelicUseCase = pickRelicUseCase;
            _pickRelicFacade = pickRelicFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _pickRelicUseCase.pickRelic
                .Subscribe(_pickRelicFacade.RenderAsync)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}