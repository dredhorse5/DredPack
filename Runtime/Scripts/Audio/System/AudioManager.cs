using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DredPack.Audio
{
    ///==========================================
    ///         Made in Pair of slippers
    ///              By dredhorse5
    ///         gmail: dima.titov18@gmail.com
    ///         yandex: dredhorse5@yandex.ru
    ///==========================================
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : GeneralSingleton<AudioManager>
    {
        private static List<AudioByType> audioList = new List<AudioByType>();

        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        public static AudioByType GetAudioType(AudioTypes type)
        {
            var audioByType = audioList.Find(_ => _.Type == type);
            if (audioByType != null)
                return audioByType;
            audioList.Add(new AudioByType(type));
            return audioList.Last();
        }


        public static void SetVolume(AudioTypes type, float volume) => GetAudioType(type).SetVolume(volume);
        public static float GetVolume(AudioTypes type) => GetAudioType(type).Volume;
        
        public static void SetMute(AudioTypes type, bool state) => GetAudioType(type).SetMute(state);
        public static bool GetMute(AudioTypes type) => GetAudioType(type).Muted;


        public void PlayOneShot(AudioField audioField, float _volume = 1f)
        {
            if(audioField.audioByType.Muted)
                return;
            var volume = audioField.audioByType.Volume * audioField.LocalVolume * _volume;
            var audioClip = audioField.GetClip();
            if(audioClip)
                PlayOneShot(audioClip, volume);
        }

        public void PlayOneShot(AudioClip clip, float volume)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
        

        
        public enum AudioTypes
        {
            Music,
            Audio
            // Shoot
            // Environment
            // voice
            // and more
        }

        [Serializable]
        public class AudioByType
        {
            public AudioTypes Type;
            public float Volume { get; private set; }
            public bool Muted { get; private set; }
            
            private string VOLUME_KEY => $"AudioVolume_{Type.ToString()}";
            private string MUTE_KEY => $"AudioMute_{Type.ToString()}";

            public UnityEvent<bool> ChangeMuteEvent { get; set; } = new UnityEvent<bool>();
            public UnityEvent<float> ChangeVolumeEvent { get; set; } = new Scrollbar.ScrollEvent();

            public AudioByType(AudioTypes type)
            {
                Type = type;
                Volume = GetVolumePlayerPrefs();
                Muted = GetMutePlayerPrefs();
            }

            
            public void SetVolume(float volume)
            {
                volume = Mathf.Clamp01(volume);
                PlayerPrefs.SetFloat(VOLUME_KEY, volume);
                Volume = volume;
                ChangeVolumeEvent?.Invoke(volume);
            }
            private float GetVolumePlayerPrefs() => PlayerPrefs.HasKey(VOLUME_KEY) ? PlayerPrefs.GetFloat(VOLUME_KEY) : 1f;

            public void SetMute(bool state)
            {
                PlayerPrefs.SetInt(MUTE_KEY, state ? 1 : 0);
                Muted = state;
                ChangeMuteEvent?.Invoke(Muted);
            }

            private bool GetMutePlayerPrefs()
            {
                var hasKey = PlayerPrefs.HasKey(MUTE_KEY);
                var intPtr = PlayerPrefs.GetInt(MUTE_KEY);
                return hasKey && intPtr == 1;
            }
        }
    }

}