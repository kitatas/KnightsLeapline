using System;
using BoardTower.Game.Application;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform highlight = default;
        [SerializeField] private SquareEventObjectView squareEventObjectView = default;

        public event Action click;

        public Tween FadeIn(float duration, float delay)
        {
            return DOTween.Sequence()
                .Append(transform
                    .DOLocalMoveY(0.0f, duration)
                    .SetEase(Ease.OutBack))
                .SetDelay(delay)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration, float delay)
        {
            return DOTween.Sequence()
                .Append(transform
                    .DOLocalMoveY(-1.0f, duration)
                    .SetEase(Ease.InBack))
                .SetDelay(delay)
                .SetLink(gameObject);
        }

        public Tween ShowHighlight(float duration)
        {
            return DOTween.Sequence()
                .Append(highlight
                    .DOLocalMoveY(0.2f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        public Tween HideHighlight(float duration)
        {
            return DOTween.Sequence()
                .Append(highlight
                    .DOLocalMoveY(-0.2f, duration)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData _)
        {
            click?.Invoke();
        }

        public Tween RenderEvent(SquareEventVO squareEvent, RenderType render, float duration, float delay)
        {
            return squareEventObjectView
                .Render(squareEvent, render, duration, delay);
        }
    }
}