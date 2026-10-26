using BoardTower.Game.Application;
using DG.Tweening;
using R3;
using R3.Triggers;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class TapScreenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private UnityEngine.UI.Button button = default;
        [SerializeField] private CanvasGroup touch = default;

        public Observable<Unit> tap => button
            .OnPointerDownAsObservable()
            .Select(_ => Unit.Default);

        private void Awake()
        {
            touch
                .DOFade(0.1f, TapScreenConfig.FLASH_DURATION)
                .SetEase(Ease.InCubic)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        public Tween FadeIn(float duration)
        {
            return DOTween.Sequence()
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.OutBack))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            return DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.OutQuart))
                .AppendCallback(() => canvasGroup.blocksRaycasts = false)
                .SetLink(gameObject);
        }
    }
}