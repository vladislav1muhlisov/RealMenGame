using System;
using UnityEngine;

namespace RealMenGame.Scripts.Sounds
{
    [Serializable]
    public class AudioClipSettings
    {
        public AudioClip AudioClip;
        [Range(0f, 1f)] public float Volume = 1f;
    }
}