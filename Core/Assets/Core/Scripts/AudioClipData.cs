using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Scripts
{
    [Serializable]
    public abstract class AudioClipData
    {
        [SerializeField]
        private float volume = 1f;
        
        [SerializeField]
        private float pitch = 1f;
        
        public abstract AudioClip GetAudioClip();

        public float GetVolume()
        {
            return volume;
        }

        public float GetPitch()
        {
            return Random.Range(.95f * pitch, 1.05f * pitch);
        }
    }

    [Serializable]
    public class SingleAudioClipData : AudioClipData
    {
        [SerializeField]
        private AudioClip clip;
        
        public override AudioClip GetAudioClip()
        {
            return clip;
        }
    }
}
