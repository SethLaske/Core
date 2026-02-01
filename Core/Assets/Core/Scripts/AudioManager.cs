using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Scripts
{
    public class AudioManager : ManagerBase<AudioManager>
    {
        public override string saveID => "AudioManager";
        private AudioManagerSaveObject loadedAudioSave;

        [Header("Settings")] 
        [SerializeField] 
        private AudioMixer mainMixer;
        
        [SerializeField] 
        private AudioClipsConfig clipsConfig;
        public AudioClipsConfig clips => clipsConfig;
        
        [SerializeField] 
        private PooledAudioSource sfxPrefab;
        
        [SerializeField] 
        private int defaultPoolSize = 20;

        [Header("Music Channel")] 
        [SerializeField]
        private AudioSource musicSource;
        
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
        
        public void SetVolumes(float master, float music, float sfx)
        {
            loadedAudioSave.masterVolume = master;
            loadedAudioSave.musicVolume = music;
            loadedAudioSave.sfxVolume = sfx;

            UpdateMixer();
        }

        private void UpdateMixer()
        {
            // Convert 0-1 linear value to Decibels
            // 0.0001f ensures we don't Log(0) which causes errors
            float masterdB = Mathf.Log10(Mathf.Max(0.0001f, loadedAudioSave.masterVolume)) * 20;
            float musicdB = Mathf.Log10(Mathf.Max(0.0001f, loadedAudioSave.musicVolume)) * 20;
            float sfxdB = Mathf.Log10(Mathf.Max(0.0001f, loadedAudioSave.sfxVolume)) * 20;

            mainMixer.SetFloat("MasterVol", masterdB);
            mainMixer.SetFloat("MusicVol", musicdB);
            mainMixer.SetFloat("SFXVol", sfxdB);
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
        
        public override string SaveData()
        {
            return loadedAudioSave.GetData();
        }

        protected override void LoadData()
        {
            base.LoadData();
            
            loadedAudioSave = new AudioManagerSaveObject();
            SaveSingleton.instance.TryLoadSavedData(saveID, loadedAudioSave);
            UpdateMixer();
        }
        
        [Serializable]
        public class AudioManagerSaveObject : SaveObject<AudioManagerSaveObject>
        {
            public float masterVolume;
            public float sfxVolume;
            public float musicVolume;

            public AudioManagerSaveObject()
            {
                masterVolume = .5f;
                sfxVolume = .5f;
                musicVolume = .5f;
            }
        }
    }
}