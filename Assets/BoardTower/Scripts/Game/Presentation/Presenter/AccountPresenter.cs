using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Presentation.Facade;
using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

namespace BoardTower.Game.Presentation.Presenter
{
    public sealed class AccountPresenter : IStartable, IDisposable
    {
        private readonly AccountUseCase _accountUseCase;
        private readonly ExceptionUseCase _exceptionUseCase;
        private readonly LoadingUseCase _loadingUseCase;
        private readonly AccountFacade _accountFacade;
        private readonly CompositeDisposable _disposable;

        public AccountPresenter(AccountUseCase accountUseCase, ExceptionUseCase exceptionUseCase,
            LoadingUseCase loadingUseCase, AccountFacade accountFacade)
        {
            _accountUseCase = accountUseCase;
            _exceptionUseCase = exceptionUseCase;
            _loadingUseCase = loadingUseCase;
            _accountFacade = accountFacade;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _accountFacade.OnDecisionDisplayName()
                .SubscribeAwait(async (x, ct) => await UpdateDisplayNameAsync(x, ct))
                .AddTo(_disposable);

            _accountFacade.Render(_accountUseCase.user);
        }

        private async UniTask UpdateDisplayNameAsync(string name, CancellationToken token)
        {
            try
            {
                await _loadingUseCase.FadeAsync(Fade.In, token);

                var userDisplayName = new UserDisplayNameVO(name);
                await _accountUseCase.UpdateDisplayNameAsync(userDisplayName, token);

                await _loadingUseCase.FadeAsync(Fade.Out, token);
            }
            catch (ExceptionVO e)
            {
                await _loadingUseCase.FadeAsync(Fade.Out, token);
                await _exceptionUseCase.ThrowAsync(e, token);

            }
            catch (Exception)
            {
                await _loadingUseCase.FadeAsync(Fade.Out, token);
                await _exceptionUseCase.ThrowQuitAsync(ExceptionConfig.UNKNOWN_ERROR, token);
            }
            finally
            {
                _accountFacade.Render(_accountUseCase.user);
            }
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}