using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Common.Data.DataStore;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoardTower.Common.Domain.Repository
{
    public sealed class SaveRepository
    {
        private static SaveDTO Load()
        {
            var data = ES3.Load(SaveConfig.ES3_KEY, defaultValue: "");
            return string.IsNullOrEmpty(data)
                ? Create()
                : JsonUtility.FromJson<SaveDTO>(data);
        }

        public async UniTask<SaveVO> LoadAsync(CancellationToken token)
        {
#if UNITY_WEBGL
            // WebGL はスレッドプールが使えないため、RunOnThreadPool と同じ挙動をメインスレッド上で再現する
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            token.ThrowIfCancellationRequested();

            SaveDTO dto;
            try
            {
                dto = Load();
            }
            finally
            {
                await UniTask.Yield();
            }

            return dto.ToVO();
#else
            var dto = await UniTask.RunOnThreadPool(Load, cancellationToken: token);
            return dto.ToVO();
#endif
        }

        private static SaveDTO Create()
        {
            var data = new SaveDTO();
            Save(data);
            return data;
        }

        public void Delete()
        {
            var data = SaveDTO.Recreate(Load());
            Save(data);
        }

        private static void Save(SaveDTO data)
        {
            ES3.Save(SaveConfig.ES3_KEY, JsonUtility.ToJson(data));
        }

        public void SaveUser(LocalUserVO value)
        {
            var data = Load();
            data.user = new LocalUserDTO(value);
            Save(data);
        }

        public void SaveBgmVolume(VolumeVO value)
        {
            var data = Load();
            data.bgmVolume = new VolumeDTO(value);
            Save(data);
        }

        public void SaveSeVolume(VolumeVO value)
        {
            var data = Load();
            data.seVolume = new VolumeDTO(value);
            Save(data);
        }

        public void SaveMasterVolume(VolumeVO value)
        {
            var data = Load();
            data.masterVolume = new VolumeDTO(value);
            Save(data);
        }
    }
}