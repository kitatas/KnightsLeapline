using System;
using System.Collections.Generic;
using Cysharp.Text;
using UniEx;
using UnityEngine;

namespace BoardTower.Common.Application
{
    public sealed class LoadVO
    {
        public readonly SceneName sceneName;
        public readonly LoadType loadType;

        public LoadVO(SceneName sceneName, LoadType loadType)
        {
            if (sceneName is SceneName.None) throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_SCENE);
            if (loadType is LoadType.None) throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_LOAD);

            this.sceneName = sceneName;
            this.loadType = loadType;
        }
    }

    public abstract class ExceptionVO : Exception
    {
        public ExceptionVO(string message) : base(message)
        {
        }

        public virtual string exceptionMessage => ExceptionConfig.QUIT_MESSAGE;
        public string message => ZString.Format("{0}\n{1}", base.Message, exceptionMessage);
    }

    public sealed class RebootExceptionVO : ExceptionVO
    {
        public RebootExceptionVO(string message) : base(message)
        {
        }

        public override string exceptionMessage => ExceptionConfig.REBOOT_MESSAGE;
    }

    public sealed class RetryExceptionVO : ExceptionVO
    {
        public RetryExceptionVO(string message) : base(message)
        {
        }

        public override string exceptionMessage => ExceptionConfig.RETRY_MESSAGE;
    }

    public sealed class QuitExceptionVO : ExceptionVO
    {
        public QuitExceptionVO(string message) : base(message)
        {
        }
    }

    public sealed class ExceptionNotifyVO
    {
        public readonly ExceptionVO exception;
        public readonly TransitionVO transition;

        public ExceptionNotifyVO(ExceptionVO exception, TransitionVO transition)
        {
            this.exception = exception;
            this.transition = transition;
        }

        public static ExceptionNotifyVO Create(ExceptionVO exception, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new ExceptionNotifyVO(exception, transition);
        }
    }

    public sealed class ExceptionActionVO
    {
        public readonly ExceptionVO exception;

        public ExceptionActionVO(ExceptionVO exception)
        {
            this.exception = exception;
        }
    }

    public sealed class LoadingTransitionVO
    {
        public readonly TransitionVO transition;

        public LoadingTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static LoadingTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new LoadingTransitionVO(transition);
        }
    }

    public sealed class TransitionVO
    {
        public readonly Fade fade;
        public readonly float duration;

        public TransitionVO(Fade fade, float duration = 0.0f)
        {
            if (fade is Fade.None) throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE);
            if (duration < 0.0f) throw new QuitExceptionVO(ExceptionConfig.INVALID_DURATION);

            this.fade = fade;
            this.duration = duration;
        }
    }

    public abstract class BaseModalVO<T> where T : Enum
    {
        public readonly T type;
        public readonly Fade fade;

        protected BaseModalVO(T type, Fade fade)
        {
            if (fade is Fade.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE);

            this.type = type;
            this.fade = fade;
        }
    }

    public abstract class BaseModalTransitionVO<T> where T : Enum
    {
        public readonly T type;
        public readonly TransitionVO transition;

        protected BaseModalTransitionVO(T type, TransitionVO transition)
        {
            this.type = type;
            this.transition = transition;
        }
    }

    public abstract class AudioVO<T> where T : Enum
    {
        public readonly T type;
        public readonly AudioClip clip;
        public readonly string cue;

        public AudioVO(T type, AudioClip clip)
        {
            this.type = type;
            this.clip = clip;
        }

        public AudioVO(T type, string cue)
        {
            this.type = type;
            this.cue = cue;
        }
    }

    public sealed class BgmAudioVO : AudioVO<BgmType>
    {
        public BgmAudioVO(BgmType type, AudioClip clip) : base(type, clip)
        {
        }

        public BgmAudioVO(BgmType type, string cue) : base(type, cue)
        {
        }
    }

    public sealed class SeAudioVO : AudioVO<SeType>
    {
        public SeAudioVO(SeType type, AudioClip clip) : base(type, clip)
        {
        }

        public SeAudioVO(SeType type, string cue) : base(type, cue)
        {
        }
    }

    public abstract class SoundVO<T> where T : Enum
    {
        public readonly AudioVO<T> audio;
        public readonly float delay;
        public readonly bool isMute;

        public SoundVO(AudioVO<T> audio, float delay, bool isMute)
        {
            this.audio = audio;
            this.delay = delay;
            this.isMute = isMute;
        }
    }

    public sealed class BgmSoundVO : SoundVO<BgmType>
    {
        public BgmSoundVO(AudioVO<BgmType> audio, float delay, bool isMute) : base(audio, delay, isMute)
        {
        }
    }

    public sealed class SeSoundVO : SoundVO<SeType>
    {
        public SeSoundVO(AudioVO<SeType> audio, float delay, bool isMute) : base(audio, delay, isMute)
        {
        }
    }

    public sealed class SaveVO
    {
        public readonly LocalUserVO user;
        public readonly VolumeVO masterVolume;
        public readonly VolumeVO bgmVolume;
        public readonly VolumeVO seVolume;

        public SaveVO(LocalUserVO user, VolumeVO masterVolume, VolumeVO bgmVolume, VolumeVO seVolume)
        {
            this.user = user;
            this.masterVolume = masterVolume;
            this.bgmVolume = bgmVolume;
            this.seVolume = seVolume;
        }
    }

    public sealed class LocalUserVO
    {
        public readonly string id;

        public LocalUserVO(string id)
        {
            this.id = id;
        }
    }

    public sealed class VolumeVO
    {
        public readonly float value;
        public readonly bool isMute;

        public VolumeVO(float value, bool isMute)
        {
            this.value = value;
            this.isMute = isMute;
        }
    }

    public sealed class PlayFabUserVO
    {
        public readonly string playFabId;
        public readonly string entityId;
        public readonly bool isNewly;
        public readonly UserDisplayNameVO displayName;
        public readonly ProgressVO[] progresses;

        public PlayFabUserVO(string playFabId, string entityId, bool isNewly, UserDisplayNameVO displayName,
            ProgressVO[] progresses)
        {
            this.playFabId = playFabId;
            this.entityId = entityId;
            this.isNewly = isNewly;
            this.displayName = displayName;
            this.progresses = progresses;
        }

        public static PlayFabUserVO CreateDummy()
        {
            return new PlayFabUserVO("", "", false, UserDisplayNameVO.Create(), Array.Empty<ProgressVO>());
        }

        public static PlayFabUserVO UpdateDisplayName(PlayFabUserVO playFabUser, UserDisplayNameVO displayName)
        {
            return new PlayFabUserVO(playFabUser.playFabId, playFabUser.entityId, playFabUser.isNewly,
                displayName, playFabUser.progresses);
        }

        public static PlayFabUserVO UpdateProgresses(PlayFabUserVO playFabUser, ProgressVO[] progresses)
        {
            return new PlayFabUserVO(playFabUser.playFabId, playFabUser.entityId, playFabUser.isNewly,
                playFabUser.displayName, progresses);
        }
    }

    public sealed class ProgressVO
    {
        public readonly int type;
        public readonly int value;

        public ProgressVO(int type, int value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public sealed class UserDisplayNameVO
    {
        public readonly string value;

        private UserDisplayNameVO()
        {
            this.value = "";
        }

        public UserDisplayNameVO(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                throw new RetryExceptionVO(ExceptionConfig.INVALID_DISPLAY_NAME);

            if (value.Length.IsBetween(3, 10) == false)
                throw new RetryExceptionVO(ExceptionConfig.INVALID_DISPLAY_NAME_LENGTH);

            this.value = value;
        }

        public static UserDisplayNameVO Create()
        {
            return new UserDisplayNameVO();
        }
    }

    public sealed class UserVO
    {
        public readonly LocalUserVO localUser;
        public readonly PlayFabUserVO playFabUser;

        public UserVO(LocalUserVO localUser, PlayFabUserVO playFabUser)
        {
            this.localUser = localUser;
            this.playFabUser = playFabUser;
        }
    }

    public sealed class LoginResultVO
    {
        public readonly bool isSuccess;
        public readonly bool isRegistered;

        public LoginResultVO(bool isSuccess, bool isRegistered)
        {
            this.isSuccess = isSuccess;
            this.isRegistered = isRegistered;
        }
    }

    public sealed class PlayFabRankingVO
    {
        public readonly IEnumerable<PlayFabRankingEntryVO> entries;

        public PlayFabRankingVO(IEnumerable<PlayFabRankingEntryVO> entries)
        {
            this.entries = entries;
        }
    }

    public sealed class PlayFabRankingEntryVO
    {
        public readonly string entityId;
        public readonly int rank;
        public readonly string displayName;
        public readonly string score;

        public PlayFabRankingEntryVO(string entityId, int rank, string displayName, string score)
        {
            this.entityId = entityId;
            this.rank = rank;
            this.displayName = displayName;
            this.score = score;
        }
    }

    public sealed class GameModeVO
    {
        public readonly GameMode mode;

        public GameModeVO(GameMode mode)
        {
            this.mode = mode;
        }
    }

    public sealed class GameModeTransitionVO
    {
        public readonly GameMode gameMode;
        public readonly TransitionVO transition;

        public GameModeTransitionVO(GameMode gameMode, TransitionVO transition)
        {
            this.gameMode = gameMode;
            this.transition = transition;
        }

        public static GameModeTransitionVO Create(GameMode mode, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new GameModeTransitionVO(mode, transition);
        }
    }
}