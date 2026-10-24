using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Common.Presentation.Facade;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace BoardTower.Common.Presentation.Presenter
{
    public sealed class ExceptionPresenter : IInitializable, IDisposable
    {
        private readonly ExceptionUseCase _exceptionUseCase;
        private readonly SceneUseCase _sceneUseCase;
        private readonly ExceptionFacade _exceptionFacade;
        private readonly CompositeDisposable _disposable;

        public ExceptionPresenter(ExceptionUseCase exceptionUseCase, SceneUseCase sceneUseCase,
            ExceptionFacade exceptionFacade)
        {
            _exceptionUseCase = exceptionUseCase;
            _sceneUseCase = sceneUseCase;
            _exceptionFacade = exceptionFacade;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _exceptionUseCase.exceptionNotify
                .Subscribe(_exceptionFacade.FadeAsync)
                .AddTo(_disposable);

            _exceptionUseCase.exceptionAction
                .Subscribe(HandleExceptionActionAsync)
                .AddTo(_disposable);

            _exceptionFacade.OnDecision()
                .Subscribe(_exceptionUseCase.HandleDecision)
                .AddTo(_disposable);
        }

        private UniTask HandleExceptionActionAsync(ExceptionActionVO exceptionAction, CancellationToken token)
        {
            return exceptionAction.exception switch
            {
                RebootExceptionVO => HandleRebootActionAsync(token),
                RetryExceptionVO => HandleRetryActionAsync(token),
                _ => HandleQuitActionAsync(token),
            };
        }

        private UniTask HandleRebootActionAsync(CancellationToken token)
        {
            _sceneUseCase.Load(SceneName.Boot, LoadType.Fade);
            return UniTask.Yield(token);
        }

        private static UniTask HandleRetryActionAsync(CancellationToken token)
        {
            return UniTask.Yield(token);
        }

        private static UniTask HandleQuitActionAsync(CancellationToken token)
        {
            // QuitException含む意図しないExceptionの場合は強制終了
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE
            UnityEngine.Application.Quit();
#else
            // TODO: 上記PF以外の場合
#endif
            return UniTask.Yield(token);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}