using System;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class Mood : MonoBehaviour
    {
        [SerializeField] private GameObject Fine;
        [SerializeField] private GameObject Angry;

        public void SetMood(bool isFine)
        {
            Fine.SetActive(isFine);
            Angry.SetActive(isFine == false);
        }

        private void Update()
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);
        }
    }
}