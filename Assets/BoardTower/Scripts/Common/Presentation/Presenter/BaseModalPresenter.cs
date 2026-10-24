using System;
using System.Threading;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public abstract class BaseModalPresenter<T> : IInitializable, IDisposable where T : Enum
    {
        private readonly BaseModalUseCase<T> _modalUseCase;
        private readonly BaseModalFacade<T> _modalFacade;
        private readonly CancellationTokenSource _tokenSource;
        private readonly CompositeDisposable _disposable;

        public BaseModalPresenter(BaseModalUseCase<T> modalUseCase, BaseModalFacade<T> modalFacade)
        {
            _modalUseCase = modalUseCase;
            _modalFacade = modalFacade;
            _tokenSource = new CancellationTokenSource();
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _modalUseCase.subscriber
                .Subscribe(_modalFacade.FadeAsync)
                .AddTo(_disposable);

            foreach (var observable in _modalFacade.OnClickAsObservables())
            {
                observable
                    .Subscribe(x => _modalUseCase.FadeAsync(x, _tokenSource.Token))
                    .AddTo(_disposable);
            }
        }

        void IDisposable.Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _disposable?.Dispose();
        }
    }
}