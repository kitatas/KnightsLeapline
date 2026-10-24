using System.Threading;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.UseCase;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using BoardTower.Game.Domain.Ports;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Domain.UseCase
{
    public sealed class GameStateUseCase : BaseStateUseCase<GameState>
    {
        public GameStateUseCase(GameStateEntity entity, RetryCountEntity retryCountEntity, GameStatePorts ports) : base(
            entity, retryCountEntity, ports)
        {
        }

        public override UniTask InitAsync(CancellationToken token)
        {
            return PublishAsync(GameState.Init, token);
        }

        public override UniTask PublishAsync(GameState value, CancellationToken token)
        {
            return value is GameState.None
                ? UniTask.Yield(token)
                : base.PublishAsync(value, token);
        }
    }
}