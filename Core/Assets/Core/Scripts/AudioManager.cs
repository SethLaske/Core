using UnityEngine;

namespace Core.Scripts
{
    public class AudioManager : ManagerBase<AudioManager>
    {
        [Header("Settings")] 
        [SerializeField] 
        private PooledAudioSource sfxPrefab;

        [SerializeField] 
        private AudioClipsConfig clipsConfig;
        public AudioClipsConfig clips => clipsConfig;
        
        [SerializeField] 
        private int defaultPoolSize = 20;

        [Header("Music Channel")] 
        [SerializeField]
        private AudioSource musicSource;

        // The Pool
        private ObjectPool<PooledAudioSource> _sfxPool;

        protected override void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(gameObject);

            InitializePool();
        }

        private void InitializePool()
        {
            // Create the pool, parenting objects to this AudioManager to keep hierarchy clean
            _sfxPool = new ObjectPool<PooledAudioSource>(sfxPrefab, defaultPoolSize, transform);
        }

        #region Music Logic

        public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true)
        {
            // Don't restart if it's the same song
            if (musicSource.clip == clip && musicSource.isPlaying)
            {
                return;
            }

            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = volume;
            musicSource.Play();
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void ResumeMusic()
        {
            musicSource.UnPause();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        #endregion

        #region SFX Logic
        
        public void PlaySFX(AudioClipData argClip)
        {
            PlaySFX(argClip, transform);
        }
        
        public void PlaySFX(AudioClipData argClip, Vector3 position)
        {
            if (argClip == null) return;

            PooledAudioSource sfx = _sfxPool.Get();
            
            sfx.Initialize(_sfxPool);

            sfx.transform.position = position;
            sfx.Play(argClip);
        }
        
        public void PlaySFX(AudioClipData argClip, Transform target)
        {
            if (argClip == null) return;

            PooledAudioSource sfx = _sfxPool.Get();
            sfx.Initialize(_sfxPool);

            // Attach to the target
            sfx.transform.SetParent(target);
            sfx.transform.localPosition = Vector3.zero;

            sfx.Play(argClip);
        }

        #endregion
    }
}