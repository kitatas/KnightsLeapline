using BoardTower.Common.Application;

namespace BoardTower.Common.Presentation.View
{
    public interface ISoundView
    {
        void PlayBgm(BgmSoundVO sound);
        void StopBgm();
        void PlaySe(SeSoundVO sound);
        void SetBgmVolume(float volume);
        void SetSeVolume(float volume);
        void PauseBgm();
        void UnPauseBgm();
    }
}