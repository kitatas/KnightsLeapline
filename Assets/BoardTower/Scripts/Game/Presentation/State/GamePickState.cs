using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GamePickState : BaseGameState
    {
        private readonly LotRelicUseCase _lotRelicUseCase;
        private readonly PickRelicUseCase _pickRelicUseCase;
        private readonly SelectRelicUseCase _selectRelicUseCase;

        public GamePickState(LotRelicUseCase lotRelicUseCase, PickRelicUseCase pickRelicUseCase,
            SelectRelicUseCase selectRelicUseCase)
        {
            _lotRelicUseCase = lotRelicUseCase;
            _pickRelicUseCase = pickRelicUseCase;
            _selectRelicUseCase = selectRelicUseCase;
        }

        public override GameState state => GameState.Pick;

        public override async UniTask InitAsync(CancellationToken token)
        {
            _selectRelicUseCase.Hide();
            await (
                _lotRelicUseCase.InitAsync(token)
            );
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _lotRelicUseCase.FadeAsync(Fade.In, token);
            await _pickRelicUseCase.PickAsync(token);
            await _lotRelicUseCase.FadeAsync(Fade.Out, token);

            return GameState.SetUp;
        }

        public override async UniTask ForceExitAsync(CancellationToken token)
        {
            _selectRelicUseCase.Hide();
            await (
                _lotRelicUseCase.FadeAsync(Fade.Out, token)
            );
        }
    }
}