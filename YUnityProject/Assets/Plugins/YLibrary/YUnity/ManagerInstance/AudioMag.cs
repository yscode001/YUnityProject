using UnityEngine;

namespace YUnity
{
    public partial class AudioMag : MonoBehaviourBaseY
    {
        private AudioMag() { }
        public static AudioMag Instance { get; private set; } = null;

        private AudioSource bgmAudioSource;
        private AudioSource normalAudioSource;

        internal void Init()
        {
            Instance = this;
            EnableBGMAudioClip = true;
            EnableNormalAudioClip = true;

            GameObject bg = new GameObject
            {
                name = "BGMAudioSource"
            };
            bg.transform.SetParent(transform, false);
            bgmAudioSource = bg.AddComponent<AudioSource>();
            bgmAudioSource.volume = 0.1f;

            GameObject normal = new GameObject
            {
                name = "NormalAudioSource"
            };
            normal.transform.SetParent(transform, false);
            normalAudioSource = normal.AddComponent<AudioSource>();
            normalAudioSource.volume = 0.2f;
        }
    }

    #region 背景音乐
    public partial class AudioMag
    {
        public bool EnableBGMAudioClip { get; private set; }

        public void SetupEnableBGMAudioClip(bool enable)
        {
            EnableBGMAudioClip = enable;
            if (!enable)
            {
                StopBGM();
            }
        }

        public void PlayBGM(AudioClip bgMusic, bool isLoop)
        {
            if (bgMusic == null)
            {
                return;
            }
            if (bgmAudioSource.clip == null || bgmAudioSource.clip.name != bgMusic.name || !bgmAudioSource.isPlaying)
            {
                bgmAudioSource.clip = bgMusic;
                bgmAudioSource.loop = isLoop;
                bgmAudioSource.Play();
            }
        }

        public void PauseBGM()
        {
            bgmAudioSource.Pause();
        }

        public void StopBGM()
        {
            bgmAudioSource.Stop();
        }

        public void Play()
        {
            bgmAudioSource.Play();
        }

        public bool IsPlayingBGMusic => bgmAudioSource.isPlaying;
    }
    #endregion

    #region 普通音效
    public partial class AudioMag
    {
        public bool EnableNormalAudioClip { get; private set; }

        public void SetupEnableNormalAudioClip(bool enable)
        {
            EnableNormalAudioClip = enable;
            if (!enable)
            {
                StopAudioClip();
            }
        }

        public void PlayAudioClip(AudioClip audioClip)
        {
            if (audioClip == null)
            {
                return;
            }
            if (normalAudioSource.clip != null && normalAudioSource.clip.name == audioClip.name && normalAudioSource.isPlaying)
            {
                return;
            }
            normalAudioSource.clip = audioClip;
            normalAudioSource.loop = false;
            normalAudioSource.Play();
        }

        public void PlayAudioClipOneShoot(AudioClip audioClip)
        {
            if (audioClip == null)
            {
                return;
            }
            normalAudioSource.PlayOneShot(audioClip);
        }

        public void PauseAudioClip()
        {
            normalAudioSource.Pause();
        }

        public void StopAudioClip()
        {
            normalAudioSource.Stop();
        }
    }
    #endregion

    #region 设置音量
    public partial class AudioMag
    {
        /// <summary>
        /// 设置背景音乐音量
        /// </summary>
        /// <param name="volume">音量范围：0-1</param>
        public void SetBGMAudioSourceVolume(float volume)
        {
            bgmAudioSource.volume = Mathf.Clamp(volume, 0, 1);
        }

        /// <summary>
        /// 设置普通音效音量
        /// </summary>
        /// <param name="volume">音量范围：0-1</param>
        public void SetNormalAudioSourceVolume(float volume)
        {
            normalAudioSource.volume = Mathf.Clamp(volume, 0, 1);
        }
    }
    #endregion
}