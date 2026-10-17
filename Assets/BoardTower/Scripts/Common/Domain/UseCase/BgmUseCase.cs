using System;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Domain.Repository;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Common.Domain.UseCase
{
    public sealed class BgmUseCase : BaseSoundUseCase<BgmType, BgmSoundVO>, IDisposable
    {
        private readonly Subject<Unit> _stop;
        
        public BgmUseCase(SaveRepository saveRepository, ISoundRepository soundRepository) : base(saveRepository,
            soundRepository)
        {
            _stop = new Subject<Unit>();
        }
        
        public Subject<Unit> stop => _stop;

        protected override async UniTask<(VolumeVO thisVolume, VolumeVO masterVolume)> LoadVolumeAsync(
            CancellationToken token)
        {
            var saveData = await _saveRepository.LoadAsync(token);
            return (saveData.bgmVolume, saveData.masterVolume);
        }
        
        public override void SwitchMasterMute()
        {
            base.SwitchMasterMute();
            SaveMasterVolume();
        }

        public override void SaveVolume()
        {
            var vo = new VolumeVO(thisVolumeValue, isThisMute.CurrentValue);
            _saveRepository.SaveBgmVolume(vo);
        }

        protected override BgmSoundVO CreateSound(AudioVO<BgmType> audio, float delay)
        {
            return new BgmSoundVO(audio as BgmAudioVO, delay, isMute.CurrentValue);
        }

        public void Stop()
        {
            _stop?.OnNext(Unit.Default);
        }

        void IDisposable.Dispose()
        {
            _stop?.Dispose();
        }
    }
}