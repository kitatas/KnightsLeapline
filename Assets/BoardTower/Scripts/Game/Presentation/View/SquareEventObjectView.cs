using BoardTower.Game.Application;
using DG.Tweening;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Presentation.View
{
    public sealed class SquareEventObjectView : MonoBehaviour
    {
        [SerializeField] private Transform eventObjectTransform = default;
        private SquareEventType _currentType = SquareEventType.None;

        public Tween Render(SquareEventVO squareEvent, RenderType render, float duration, float delay)
        {
            var isRetain = render is RenderType.RetainUp or RenderType.RetainDown;
            if (isRetain && _currentType == squareEvent.type) return null;
            _currentType = squareEvent.type;

            eventObjectTransform.gameObject.DestroyChildren();
            if (squareEvent.eventObject == null) return null;

            var _ = Instantiate(squareEvent.eventObject, eventObjectTransform);
            return DOTween.Sequence()
                .Append(eventObjectTransform
                    .DOLocalMoveY(-1.0f, 0.0f))
                .Append(eventObjectTransform
                    .DOLocalMoveY(0.2f, duration))
                .SetDelay(delay)
                .SetLink(eventObjectTransform.gameObject);
        }
    }
}