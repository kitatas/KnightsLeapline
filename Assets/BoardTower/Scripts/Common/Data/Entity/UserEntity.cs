using System;
using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;

namespace BoardTower.Common.Data.Entity
{
    public sealed class UserEntity : BaseEntity<UserVO>
    {
        public string displayName => value.playFabUser.displayName.value;
        public bool isRegistered => !string.IsNullOrEmpty(displayName);

        private Dictionary<int, ProgressVO> _achievement;

        private Dictionary<int, ProgressVO> achievement =>
            _achievement ??= (value.playFabUser.progresses ?? Array.Empty<ProgressVO>())
                .ToDictionary(x => x.type, x => x);

        public void SetDisplayName(UserDisplayNameVO userDisplayName)
        {
            var localUser = value.localUser;
            var playFabUser = PlayFabUserVO.UpdateDisplayName(value.playFabUser, userDisplayName);
            var user = new UserVO(localUser, playFabUser);
            Set(user);
        }

        public ProgressVO Find(int type)
        {
            if (achievement.TryGetValue(type, out var progress)) return progress;
            return new ProgressVO(type, 0);
        }

        public void UpdateProgress(ProgressVO[] progresses)
        {
            var playFabUser = PlayFabUserVO.UpdateProgresses(value.playFabUser, progresses);
            Set(new UserVO(value.localUser, playFabUser));
            _achievement = null;
        }

        public bool IsEqualEntityId(string id) => value.playFabUser.entityId == id;

        public void ClearAchievement()
        {
            _achievement = null;
        }
    }
}