using BoardTower.Game.Application;
using DG.Tweening;
using R3;
using R3.Triggers;
using TMPro;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class EventLogView : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera = default;
        [SerializeField] private Transform target = default;
        [SerializeField] private RectTransform safeArea = default;
        [SerializeField] private RectTransform view = default;
        [SerializeField] private TextMeshProUGUI message = default;

        private Tween _tween;

        private void Start()
        {
            var rect = safeArea.rect;
            var halfHeight = view.rect.height * 0.5f;

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        safeArea,
                        mainCamera.WorldToScreenPoint(target.position),
                        null,
                        out var localPosition
                    );

                    localPosition.ClampY(rect.yMin + halfHeight, rect.yMax - halfHeight + 75.0f);
                    view.position = safeArea.TransformPoint(localPosition);
                })
                .AddTo(this);
        }

        public Tween Tween(EventLogMessageVO eventLogMessage)
        {
            _tween?.Kill(true);

            var y = message.rectTransform.anchoredPosition.y;
            return _tween = DOTween.Sequence()
                .Append(Show(eventLogMessage, y))
                .AppendInterval(eventLogMessage.duration)
                .Append(Hide(eventLogMessage, y))
                .SetLink(gameObject);
        }

        private Tween Show(EventLogMessageVO eventLogMessage, float y)
        {
            message.text = eventLogMessage.message;

            return DOTween.Sequence()
                .Append(message
                    .DOFade(1.0f, eventLogMessage.duration / 2.0f)
                    .SetEase(Ease.Linear))
                .Join(message.rectTransform
                    .DOAnchorPosY(y + 25.0f, eventLogMessage.duration / 2.0f)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        private Tween Hide(EventLogMessageVO eventLogMessage, float y)
        {
            return DOTween.Sequence()
                .Append(message
                    .DOFade(0.0f, eventLogMessage.duration / 2.0f)
                    .SetEase(Ease.Linear))
                .Join(message.rectTransform
                    .DOAnchorPosY(y + 50.0f, eventLogMessage.duration / 2.0f)
                    .SetEase(Ease.Linear))
                .Append(message.rectTransform
                    .DOAnchorPosY(y, 0.0f)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }
    }
}