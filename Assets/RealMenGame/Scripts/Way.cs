using System;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class Way : MonoBehaviour
    {
        public Transform[] WayPoints;

        private void OnDrawGizmos()
        {
            if (WayPoints.Length <= 1) return;

            Gizmos.DrawWireSphere(WayPoints[0].position, 0.2f);

            for (var i = 1; i < WayPoints.Length; ++i)
            {
                Gizmos.DrawLine(WayPoints[i - 1].position, WayPoints[i].position);
                Gizmos.DrawWireSphere(WayPoints[i].position, 0.2f);
            }
        }

        private void OnTransformChildrenChanged()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            WayPoints = new Transform[transform.childCount];

            for (var i = 0; i < transform.childCount; ++i)
                WayPoints[i] = transform.GetChild(i);
        }
    }
}