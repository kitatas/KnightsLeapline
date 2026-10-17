using BoardTower.Common.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Common.Presentation.View
{
    public sealed class SoundView : MonoBehaviour, ISoundView
    {
        [SerializeField] private AudioSource bgmSource = default;
        [SerializeField] private AudioSource seSource = default;

        public void PlayBgm(BgmSoundVO sound)
        {
            if (bgmSource.clip == sound.audio.clip) return;
            bgmSource.clip = sound.audio.clip;

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
            bgmSource.Play();
        }

        public void PlaySe(SeSoundVO sound)
        {
            if (sound.isMute) return;

            if (sound.delay > 0.0f)
            {
                this.Delay(sound.delay, () => PlaySe(sound.audio.clip));
            }
            else
            {
                PlaySe(sound.audio.clip);
            }
        }

        private void PlaySe(AudioClip clip)
        {
            seSource.PlayOneShot(clip);
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
            bgmSource.Pause();
        }

        public void UnPauseBgm()
        {
            if (bgmSource.time > 0.0f)
            {
                bgmSource.UnPause();
            }
            else
            {
                bgmSource.Play();
            }
        }
    }
}