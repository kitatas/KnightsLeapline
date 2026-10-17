using BoardTower.Common.Application;
using CriWare;
using CriWare.Assets;
using UniEx;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class CriSoundView : MonoBehaviour, ISoundView
    {
        [SerializeField] private CriAtomSourceForAsset bgmSource = default;
        [SerializeField] private CriAtomSourceForAsset seSource = default;

        private string _requestBgmCue = string.Empty;
        private string _currentBgmCue = string.Empty;

        public void PlayBgm(BgmSoundVO sound)
        {
            _requestBgmCue = sound.audio.cue;

            if (sound.isMute) return;

            if (sound.delay > 0.0f)
            {
                this.Delay(sound.delay, PlayBgm);
            }
            else
            {
                PlayBgm();
            }
        }

        public void StopBgm()
        {
            bgmSource.Stop();
        }

        private void PlayBgm()
        {
            if (bgmSource.status is CriAtomSourceBase.Status.Playing)
            {
                bgmSource.Stop();
                bgmSource.Pause(false);
            }
            bgmSource.Play(_requestBgmCue);
            _currentBgmCue = _requestBgmCue;
        }

        public void PlaySe(SeSoundVO sound)
        {
            if (sound.isMute) return;

            if (sound.delay > 0.0f)
            {
                this.Delay(sound.delay, () => PlaySe(sound.audio.cue));
            }
            else
            {
                PlaySe(sound.audio.cue);
            }
        }

        private void PlaySe(string cueName)
        {
            seSource.Play(cueName);
        }

        public void SetBgmVolume(float volume)
        {
            bgmSource.volume = volume;
        }

        public void SetSeVolume(float volume)
        {
            seSource.volume = volume;
        }

        public void PauseBgm()
        {
            bgmSource.Pause(true);
        }

        public void UnPauseBgm()
        {
            if (_requestBgmCue == _currentBgmCue)
            {
                bgmSource.Pause(false);
            }
            else
            {
                PlayBgm();
            }
        }
    }
}