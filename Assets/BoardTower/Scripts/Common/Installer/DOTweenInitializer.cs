using DG.Tweening;
using UnityEngine;

namespace BoardTower.Common.Installer
{
    public sealed class DOTweenInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            DOTween.SetTweensCapacity(500, 125);
        }
    }
}