using R3;
using R3.Triggers;
using UniEx;
using UnityEngine;
using UnityEngine.UI;

namespace BoardTower.Common.Presentation.View
{
    public sealed class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera = default;
        [SerializeField] private CanvasScaler canvasScaler = default;

        private float _referenceAspectRatio;
        private float _baseFieldOfView;
        private float _lastScreenAspectRatio;

        private void Start()
        {
            _referenceAspectRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
            _baseFieldOfView = mainCamera.fieldOfView;

            this.UpdateAsObservable()
                .Select(_ => (float)Screen.width / Screen.height)
                .Where(x => !x.IsEqual(_lastScreenAspectRatio))
                .Subscribe(UpdateFieldOfView)
                .AddTo(this);
        }

        private void UpdateFieldOfView(float lastScreenAspectRatio)
        {
            _lastScreenAspectRatio = lastScreenAspectRatio;
            mainCamera.fieldOfView = _baseFieldOfView;

            var d = _referenceAspectRatio / _lastScreenAspectRatio;
            if (d <= 1.0f) return;

            var halfFieldOfView = _baseFieldOfView * Mathf.Deg2Rad * 0.5f;
            mainCamera.fieldOfView = 2.0f * Mathf.Atan(Mathf.Tan(halfFieldOfView) * d) * Mathf.Rad2Deg;
        }
    }
}