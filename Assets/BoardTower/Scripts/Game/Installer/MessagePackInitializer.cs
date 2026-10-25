#if !UNITY_EDITOR
using BoardTower.Game.Data.DataStore;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace BoardTower.Game.Installer
{
    public sealed class MessagePackInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            StaticCompositeResolver.Instance.Register(
                MasterMemoryResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance
            );

            var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
            MessagePackSerializer.DefaultOptions = options;
        }
    }
}
#endif