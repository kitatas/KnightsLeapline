using R3;
using R3.Triggers;
using UniEx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BoardTower.Common.Presentation.View
{
    public sealed class TapEffectPoolView : BasePoolView<TapEffectView>
    {
        [SerializeField] private TapEffectView tapEffectView = default;
        private Camera _camera;
        private Camera _mainCamera => _camera ? _camera : _camera = Camera.main;

        public override TapEffectView Create()
        {
            return Instantiate(tapEffectView, transform);
        }

        public override void OnGet(TapEffectView item)
        {
            item.Rent();
        }

        public override void OnRelease(TapEffectView item)
        {
            item.Release();
        }

        private void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                .Subscribe(_ =>
                {
                    var tapPoint = Mouse.current.position.ReadValue();
                    var tapPosition = new Vector3(tapPoint.x, tapPoint.y, 10.0f);

                    var eff = pool.Get();
                    eff.SetUp(_mainCamera.ScreenToWorldPoint(tapPosition), _mainCamera.transform.rotation);
                    this.Delay(eff.duration + 0.5f, () => pool.Release(eff));
                })
                .AddTo(this);
        }
    }
}