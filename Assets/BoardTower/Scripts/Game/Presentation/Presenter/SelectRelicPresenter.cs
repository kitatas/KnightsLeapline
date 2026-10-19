using System;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class SelectRelicPresenter : IStartable, IDisposable
    {
        private readonly PickRelicUseCase _pickRelicUseCase;
        private readonly SelectRelicUseCase _selectRelicUseCase;
        private readonly SelectRelicFacade _selectRelicFacade;
        private readonly CompositeDisposable _disposable;

        public SelectRelicPresenter(PickRelicUseCase pickRelicUseCase, SelectRelicUseCase selectRelicUseCase,
            SelectRelicFacade selectRelicFacade)
        {
            _pickRelicUseCase = pickRelicUseCase;
            _selectRelicUseCase = selectRelicUseCase;
            _selectRelicFacade = selectRelicFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _selectRelicUseCase.selectRelic
                .Subscribe(_selectRelicFacade.Render)
                .AddTo(_disposable);

            _selectRelicUseCase.decisionRelic
                .Subscribe(_pickRelicUseCase.HandlePick)
                .AddTo(_disposable);

            _selectRelicUseCase.hideHighlight
                .Subscribe(_ => _selectRelicFacade.Hide())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}