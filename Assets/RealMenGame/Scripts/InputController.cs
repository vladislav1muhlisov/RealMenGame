using RealMenGame.Scripts.Bandits;
using UnityEngine;

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
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    var target = hit.collider.gameObject.GetComponent<BanditController>();
                    if (target != null)
                    {
                        LarekManager.Instance.OnShoot(target.transform);
                    }
                }
            }
        }
    }
}