using System.Collections.Generic;
using UnityEngine;

namespace YUnity
{
    [RequireComponent(typeof(AudioSource))]
    public partial class AudioSource3DHelper : MonoBehaviourBaseY
    {
        private AudioSource audioSource;
        private float minDistance = 1f;
        private float maxDistance = 10f;
        public Transform listenerTransform;

        protected void Awake()
        {
            audioSource = this.GetOrAddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            minDistance = audioSource.minDistance;
            maxDistance = audioSource.maxDistance;
            InvokeRepeating(nameof(CheckShoudPlayLoop), 2f, 2f);

            if (!AudioSource3DHelper.list.Contains(this))
            {
                AudioSource3DHelper.list.Add(this);
            }
        }
        private void OnDestroy()
        {
            if (AudioSource3DHelper.list.Contains(this))
            {
                AudioSource3DHelper.list.Remove(this);
            }
        }

        public void SetupDistance(float minDistance, float maxDistance)
        {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
        }

        public void SetupRolloffMode(AudioRolloffMode model)
        {
            audioSource.rolloffMode = model;
        }

        public void SetupVolume(float volume)
        {
            audioSource.volume = Mathf.Clamp(volume, 0, 1);
        }

        public void SetupListenerTransorm(Transform listenerTransform)
        {
            this.listenerTransform = listenerTransform;
        }
    }

    public partial class AudioSource3DHelper
    {
        public void PlayAudioClip(AudioClip audioClip, bool loop)
        {
            if (audioClip == null) { StopAudioClip(); return; }
            audioSource.loop = loop;
            if (audioSource.clip != null && audioSource.clip.name == audioClip.name && audioSource.isPlaying)
            {
                return;
            }
            audioSource.clip = audioClip;
            Play();
        }

        private void Play()
        {
            if (EnableAudioSource3D)
            {
                UpdatePanStereo();
                audioSource.Play();
            }
        }

        public void PauseAudioClip()
        {
            audioSource.Pause();
        }

        public void StopAudioClip()
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        private void CheckShoudPlayLoop()
        {
            if (audioSource.clip == null || listenerTransform == null) { StopAudioClip(); return; }
            float distance = Vector3.Distance(TransformY.position, listenerTransform.position);
            if (audioSource.loop)
            {
                if ((distance <= minDistance || distance >= maxDistance) && audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
                else if (EnableAudioSource3D)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                    else
                    {
                        UpdatePanStereo();
                    }
                }
            }
            else
            {
                if (distance <= minDistance || distance >= maxDistance)
                {
                    StopAudioClip();
                }
            }
        }
    }

    public partial class AudioSource3DHelper
    {
        private void UpdatePanStereo()
        {
            if (listenerTransform == null) { audioSource.panStereo = 0; return; }
            Vector3 selfWorldP = TransformY.TransformDirection(TransformY.position);
            Vector3 listenerWorldP = listenerTransform.TransformDirection(listenerTransform.position);
            Vector2 selfWorldP2 = new Vector2(selfWorldP.x, selfWorldP.z);
            Vector2 listenerWorldP2 = new Vector2(listenerWorldP.x, listenerWorldP.z);
            audioSource.panStereo = Mathf.Sin(Vector2.Angle(selfWorldP2, listenerWorldP2));
        }
    }

    #region 开关设置，方便与业务系统解耦
    public partial class AudioSource3DHelper
    {
        private static bool _enableAudioSource3D = true;
        public static bool EnableAudioSource3D => _enableAudioSource3D;

        private static List<AudioSource3DHelper> list = new List<AudioSource3DHelper>();

        public static void SetupEnableAudioSource3D(bool enable)
        {
            _enableAudioSource3D = enable;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                AudioSource3DHelper item = list[i];
                if (item == null)
                {
                    list.RemoveAt(i);
                    continue;
                }
                if (enable)
                {
                    if (item.audioSource != null && item.audioSource.clip != null && item.audioSource.loop)
                    {
                        item.Play();
                    }
                }
                else
                {
                    if (item.audioSource != null && item.audioSource.clip != null && item.audioSource.isPlaying)
                    {
                        item.PauseAudioClip();
                    }
                }
            }
        }
    }
    #endregion
}