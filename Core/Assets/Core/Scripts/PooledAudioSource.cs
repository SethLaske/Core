using UnityEngine;

namespace Core.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudioSource : MonoBehaviour, IPoolable
    {
        private AudioSource audioSource;
        private ObjectPool<PooledAudioSource> poolReference;

        private void Awake()
        { 
            audioSource = GetComponent<AudioSource>();
        }

        // Called by the AudioManager to set up the return path
        public void Initialize(ObjectPool<PooledAudioSource> pool)
        {
            poolReference = pool;
        }

        public void Play(AudioClipData argClip)
        {
            AudioClip clip = argClip.GetAudioClip();
            
            gameObject.name = $"AudioSource_{clip.name}";
            
            audioSource.clip = clip;
            audioSource.volume = argClip.GetVolume();
            audioSource.pitch = argClip.GetPitch();
            audioSource.Play();

            // Use your custom Interval class to handle the lifecycle
            // calculate duration based on pitch (higher pitch = shorter duration)
            float duration = clip.length / Mathf.Abs(argClip.GetPitch());

            // Assuming your Interval constructor starts the timer immediately
            new Interval(duration, 0, null, null, ReturnSelfToPool).Start();
        }

        private void ReturnSelfToPool()
        {
            poolReference.ReturnToPool(this);
        }
        
        public void OnSpawn()
        {
            // Reset 3D settings or spatial blend if needed
            audioSource.spatialBlend = 1.0f; // Default to 3D sound
        }

        public void OnDespawn()
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }
}