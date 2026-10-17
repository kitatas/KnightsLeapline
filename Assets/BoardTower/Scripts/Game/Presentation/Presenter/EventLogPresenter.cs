using System;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class EventLogPresenter : IStartable, IDisposable
    {
        private readonly SeUseCase _seUseCase;
        private readonly EventLogUseCase _eventLogUseCase;
        private readonly EventLogFacade _eventLogFacade;
        private readonly CompositeDisposable _disposable;

        public EventLogPresenter(SeUseCase seUseCase, EventLogUseCase eventLogUseCase, EventLogFacade eventLogFacade)
        {
            _seUseCase = seUseCase;
            _eventLogUseCase = eventLogUseCase;
            _eventLogFacade = eventLogFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _eventLogUseCase.eventLogMessage
                .Subscribe((m, ct) =>
                {
                    _seUseCase.Play(SeType.PopLog);
                    return _eventLogFacade.RenderAsync(m, ct);
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}