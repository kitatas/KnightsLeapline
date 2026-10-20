using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using BoardTower.Common.Domain.Repository;
using Cysharp.Threading.Tasks;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class AccountUseCase
    {
        private readonly UserEntity _userEntity;
        private readonly PlayFabRepository _playFabRepository;
        private readonly SaveRepository _saveRepository;

        public AccountUseCase(UserEntity userEntity, PlayFabRepository playFabRepository,
            SaveRepository saveRepository)
        {
            _userEntity = userEntity;
            _playFabRepository = playFabRepository;
            _saveRepository = saveRepository;
        }

        public UserVO user => _userEntity.value;

        public async UniTask UpdateDisplayNameAsync(UserDisplayNameVO userDisplayName, CancellationToken token)
        {
            var displayName = await _playFabRepository.UpdateDisplayNameAsync(userDisplayName.value, token);
            _userEntity.SetDisplayName(displayName);
        }

        public void Delete()
        {
            _userEntity.ClearAchievement();
            _saveRepository.Delete();
        }
    }
}