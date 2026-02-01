using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts
{
    [CreateAssetMenu(fileName = "Audio Clips Config", menuName = "Core/Audio Clips")]
    public class AudioClipsConfig : ScriptableObject
    {
        public SingleAudioClipData testClip;
    }
}