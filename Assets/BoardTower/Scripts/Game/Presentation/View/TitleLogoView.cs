using Coffee.UIEffects;
using DG.Tweening;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class TitleLogoView : MonoBehaviour
    {
        [SerializeField] private UIEffect uiEffect = default;

        private void Start()
        {
            uiEffect.transitionRate = -1.0f;

            DOTween.Sequence()
                .Append(DOTween.To(
                        () => uiEffect.transitionRate,
                        x => uiEffect.transitionRate = x,
                        1.0f,
                        3.0f
                    )
                    .SetEase(Ease.InOutCirc))
                .AppendInterval(1.5f)
                .SetLoops(-1, LoopType.Restart)
                .SetLink(gameObject);
        }
    }
}