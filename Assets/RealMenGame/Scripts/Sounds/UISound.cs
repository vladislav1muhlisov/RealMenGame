using RealMenGame.Scripts.Common;
using UnityEngine;

namespace RealMenGame.Scripts.Sounds
{
    public class UISound : MonoBehaviourSceneSingleton<UISound>
    {
        [SerializeField] private AudioSource _audioSource;

        public void Play(AudioClipSettings audioClip)
        {
            _audioSource.clip = audioClip.AudioClip;
            _audioSource.volume = audioClip.Volume;
            _audioSource.Play();
        }
    }
}