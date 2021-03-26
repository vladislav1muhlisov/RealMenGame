using UnityEngine;
using UnityEngine.EventSystems;

namespace RealMenGame.Scripts
{
    public class InputController : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject.layer != 5)
                {
                    StallManager.Instance.OnShoot(hit.point);
                }
            }
        }
    }
}