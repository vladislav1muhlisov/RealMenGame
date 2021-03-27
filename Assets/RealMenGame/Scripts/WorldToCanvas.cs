using UnityEngine;

namespace RealMenGame.Scripts
{
    public class WorldToCanvas : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private RectTransform _canvas;

        private void OnEnable()
        {
            Update();
        }

        private void Update()
        {
            if (_target == null) return;
            
            var rectTransform = (RectTransform) transform;
            var cam = Camera.main;

            if (!cam) return;
            
            var viewportPoint = cam.WorldToViewportPoint(_target.position);
            var sizeDelta = _canvas.sizeDelta;

            var screenPoint = new Vector2(
                viewportPoint.x * sizeDelta.x - sizeDelta.x * 0.5f,
                viewportPoint.y * sizeDelta.y - sizeDelta.y * 0.5f);

            rectTransform.anchoredPosition = screenPoint;
        }
    }
}