using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Common.Presentation.View
{
    public sealed class TransitionView : MonoBehaviour
    {
        [SerializeField] private RectTransform maskLeft = default;
        [SerializeField] private RectTransform maskRight = default;
        [SerializeField] private Image blocker = default;

        public Tween FadeIn(float duration)
        {
            return DOTween.Sequence()
                .AppendCallback(() => blocker.raycastTarget = true)
                .Append(maskLeft
                    .DOAnchorPos(new Vector2(-540.0f, 960.0f), duration)
                    .SetEase(Ease.OutQuad))
                .Join(maskLeft
                    .DORotate(new Vector3(0.0f, 0.0f, -60.64f), duration)
                    .SetEase(Ease.OutQuad))
                .Join(maskRight
                    .DOAnchorPos(new Vector2(540.0f, -960.0f), duration)
                    .SetEase(Ease.OutQuad))
                .Join(maskRight
                    .DORotate(new Vector3(0.0f, 0.0f, -60.64f), duration)
                    .SetEase(Ease.OutQuad))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return DOTween.Sequence()
                .Append(maskLeft
                    .DOAnchorPos(new Vector2(-2061.455f, 561.455f), duration)
                    .SetEase(Ease.OutQuad))
                .Join(maskLeft
                    .DORotate(Vector3.zero, duration)
                    .SetEase(Ease.OutQuad))
                .Join(maskRight
                    .DOAnchorPos(new Vector2(2061.455f, -561.455f), duration)
                    .SetEase(Ease.OutQuad))
                .Join(maskRight
                    .DORotate(Vector3.zero, duration)
                    .SetEase(Ease.OutQuad))
                .AppendCallback(() => blocker.raycastTarget = false)
                .SetLink(gameObject);
        }
    }
}