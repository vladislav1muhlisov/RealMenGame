using UnityEngine;

namespace RealMenGame.Scripts
{
    public class SpawnPoint : MonoBehaviour
    {
        public Way[] Ways;

        public Way GetRandomWay() => Ways[Random.Range(0, Ways.Length)];

        private void OnValidate()
        {
            Ways = GetComponentsInChildren<Way>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.4f);
        }
    }
}