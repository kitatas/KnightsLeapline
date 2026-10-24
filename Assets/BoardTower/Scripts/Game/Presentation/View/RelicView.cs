using BoardTower.Common.Utility;
using BoardTower.Game.Application;
using Cysharp.Text;
using DG.Tweening;
using FastEnumUtility;
using TMPro;
using UniEx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoardTower.Game.Presentation.View
{
    public sealed class RelicView : UIBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private Image icon = default;
        [SerializeField] private TextMeshProUGUI relicName = default;
        [SerializeField] private TextMeshProUGUI content = default;
        [SerializeField] private GameObject uniqLabel = default;

        public void Render(RelicVO value)
        {
            Render(value.type.ToInt32(), value.relicName, value.content, value.isUniq);
        }

        public void RenderEmpty()
        {
            Render(0, "---", "---", false);
        }

        private void Render(int type, string relic, string relicContent, bool isUniq)
        {
            var path = ZString.Format(RelicConfig.IMG_PATH, type);
            this.LoadAsset<Sprite>(path, x =>
            {
                icon.sprite = x;
                relicName.text = relic;
                content.text = relicContent;
                uniqLabel.SetActive(isUniq);
            });
        }

        public Tween FadeIn(float duration, float delay)
        {
            return DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(1.0f, duration)
                    .SetEase(Ease.OutBack))
                .Join(canvasGroup.transform.ToRectTransform()
                    .DOScale(Vector3.one, duration)
                    .SetEase(Ease.OutBack))
                .AppendCallback(() => canvasGroup.blocksRaycasts = true)
                .SetDelay(delay)
                .SetLink(gameObject);
        }

        public Tween FadeOut(float duration, float delay)
        {
            canvasGroup.blocksRaycasts = false;

            return DOTween.Sequence()
                .Append(canvasGroup
                    .DOFade(0.0f, duration)
                    .SetEase(Ease.OutQuart))
                .Join(canvasGroup.transform.ToRectTransform()
                    .DOScale(Vector3.one * 0.8f, duration)
                    .SetEase(Ease.OutQuart))
                .SetDelay(delay)
                .SetLink(gameObject);
        }
    }
}