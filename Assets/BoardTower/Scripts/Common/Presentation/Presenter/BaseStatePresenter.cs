using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.State;
using BoardTower.Common.Utility;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public abstract class BaseStatePresenter<T> : IAsyncStartable, IDisposable where T : Enum
    {
        private readonly ExceptionUseCase _exceptionUseCase;
        private readonly BaseStateUseCase<T> _stateUseCase;
        private readonly Dictionary<T, BaseState<T>> _stateMap;
        private readonly AsyncLockLite _locker;
        private readonly CompositeDisposable _disposable;
        private CancellationTokenSource _tokenSource;

        public BaseStatePresenter(ExceptionUseCase exceptionUseCase, BaseStateUseCase<T> stateUseCase,
            IEnumerable<BaseState<T>> states)
        {
            _exceptionUseCase = exceptionUseCase;
            _stateUseCase = stateUseCase;
            _stateMap = states.ToDictionary(x => x.state, x => x);
            _locker = new AsyncLockLite();
            _disposable = new CompositeDisposable();
            _tokenSource = new CancellationTokenSource();
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken token)
        {
            await UniTask.WhenAll(_stateMap.Values
                .Select(x => x.InitAsync(token)));

            _stateUseCase.subscriber
                .Subscribe(async (s, ct) =>
                {
                    T nextState;
                    {
                        using var _ = await _locker.LockAsync(ct);
                        nextState = await RunAsync(s, ct);
                    }

                    await _stateUseCase.PublishAsync(nextState, ct);
                })
                .AddTo(_disposable);

            _stateUseCase.forceChange
                .Subscribe(_ =>
                {
                    _tokenSource?.Cancel();
                    _tokenSource?.Dispose();
                    _tokenSource = new CancellationTokenSource();
                })
                .AddTo(_disposable);

            await _stateUseCase.InitAsync(token);
        }

        private async UniTask<T> RunAsync(T state, CancellationToken token)
        {
            try
            {
                if (!_stateMap.TryGetValue(state, out var currentState))
                {
                    throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_STATE);
                }

                var nextState = state;

                using var linkedTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(token, _tokenSource.Token);

                await currentState.EnterAsync(linkedTokenSource.Token);
                while (EqualityComparer<T>.Default.Equals(nextState, state))
                {
                    linkedTokenSource.Token.ThrowIfCancellationRequested();
                    nextState = await currentState.TickAsync(linkedTokenSource.Token);
                }
                await currentState.ExitAsync(linkedTokenSource.Token);

                return nextState;
            }
            catch (OperationCanceledException)
            {
                if (!_stateMap.TryGetValue(state, out var currentState))
                {
                    await _exceptionUseCase.ThrowQuitAsync(ExceptionConfig.NOT_FOUND_STATE, token);
                    throw;
                }

                await currentState.ForceExitAsync(token);

                // Stateの強制変更
                return _stateUseCase.forceChangeState;
            }
            catch (ExceptionVO e)
            {
                var isRetry = e is RetryExceptionVO;
                if (isRetry && _stateUseCase.IsMaxRetry(state))
                {
                    await _exceptionUseCase.ThrowRebootAsync(ExceptionConfig.MAX_RETRY, token);
                    throw;
                }

                await _exceptionUseCase.ThrowAsync(e, token);
                if (isRetry) return state;

                throw;
            }
            catch (Exception)
            {
                await _exceptionUseCase.ThrowQuitAsync(ExceptionConfig.UNKNOWN_ERROR, token);
                throw;
            }
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }
    }
}