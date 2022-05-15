using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi.Component
{
    [System.Serializable]
    public class Sound
    {
        public string Name;

        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume;
        [Range(0.1f, 3f)]
        public float Pitch;

        [HideInInspector]
        public AudioSource Source;
    }
}

