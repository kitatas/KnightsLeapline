using BoardTower.Boot.Application;
using DG.Tweening;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Boot.Presentation.View
{
    public sealed class SplashView : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button button = default;
        [SerializeField] private Image copyright = default;

        private Tween _tween;

        public Observable<Unit> tap => button
            .OnPointerDownAsObservable()
            .Select(_ => Unit.Default);

        public Tween FadeIn(Sprite sprite, float duration)
        {
            Kill();

            copyright.sprite = sprite;
            return _tween = DOTween.Sequence()
                .Append(Fade(1.0f, duration))
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration)
        {
            Kill();

            return _tween = DOTween.Sequence()
                .Append(Fade(0.0f, duration))
                .SetLink(gameObject);
        }

        public Tween FadeInOut(Sprite sprite, float duration)
        {
            Kill();

            copyright.sprite = sprite;
            return _tween = DOTween.Sequence()
                .Append(Fade(1.0f, duration))
                .AppendInterval(SplashConfig.DISPLAY_DURATION)
                .Append(Fade(0.0f, duration))
                .SetLink(gameObject);
        }

        private Tween Fade(float value, float duration)
        {
            return copyright
                .DOFade(value, duration)
                .SetEase(Ease.Linear);
        }

        private void Kill()
        {
            _tween?.Kill(true);
            _tween = null;
        }
    }
}