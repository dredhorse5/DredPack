using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        public List<AudioGroup> AudioGroups;
        public List<AudioByType> AudioTypes = new List<AudioByType>() { new AudioByType("Audio"), new AudioByType("Music") };

        public string[] audioTypesNames
        {
            get
            {
                string[] arr = new string[AudioTypes.Count];
                for (int i = 0; i < AudioTypes.Count; i++)
                    arr[i] = AudioTypes[i].Type;
                return arr;
            }
        }

        public string[] audioGroupNames
        {
            get
            {
                string[] arr = new string[AudioGroups.Count];
                for (int i = 0; i < AudioGroups.Count; i++)
                    arr[i] = AudioGroups[i].ID;
                return arr;
            }
        }

        private static List<AudioByType> audioList => Instance.AudioTypes;
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            audioList.ForEach(_ => _.Init());
        }

        public static AudioByType GetAudioType(string type)
        {
            var audioByType = audioList.Find(_ => _.Type == type);
            if (audioByType != null)
                return audioByType;
            audioList.Add(new AudioByType(type));
            return audioList.Last();
        }


        public static void SetVolume(string type, float volume) => GetAudioType(type).SetVolume(volume);
        public static float GetVolume(string type) => GetAudioType(type).Volume;
        
        public static void SetMute(string type, bool state) => GetAudioType(type).SetEnabled(state);
        public static bool GetMute(string type) => GetAudioType(type).Enabled;


        public void PlayOneShot(AudioField audioField, float _volume = 1f)
        {
            if(!audioField.audioByType.Enabled)
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

        public void PlayOneShot(string audioClipGroupID, float volume = 1f)
        {
            _audioSource.PlayOneShot(GetClipFromGroup(audioClipGroupID), volume);
        }

        public AudioGroup GetAudioGroup(string byID) => AudioGroups.Find(_ => _.ID == byID);
        public AudioClip GetClipFromGroup(string byID)
        {
            var audioGroup = GetAudioGroup(byID);
            if(audioGroup == null)
            {
                Debug.Log($"AudioGroup by id: {byID} does not exists!");
                return null;
            }

            return audioGroup.GetRandomClip();
        }

        [Serializable]
        public class AudioByType
        {
            public string Type;
            [Range(0,1f)]
            public float Volume = 1;
            public bool Enabled = true;
            
            private string VOLUME_KEY => $"AudioVolume_{Type}";
            private string ENABLED_KEY => $"AudioMute_{Type}";

            public UnityEvent<bool> ChangeEnableEvent { get; set; } = new UnityEvent<bool>();
            public UnityEvent<float> ChangeVolumeEvent { get; set; } = new Scrollbar.ScrollEvent();


            public AudioByType(string type)
            {
                Type = type;
            }

            public void Init()
            {
                Volume = GetVolumePlayerPrefs();
                Enabled = GetEnabledPlayerPrefs();
            }
            
            public void SetVolume(float volume)
            {
                volume = Mathf.Clamp01(volume);
                PlayerPrefs.SetFloat(VOLUME_KEY, volume);
                this.Volume = volume;
                ChangeVolumeEvent?.Invoke(volume);
            }
            private float GetVolumePlayerPrefs()
            {
                if(PlayerPrefs.HasKey(VOLUME_KEY))
                    return PlayerPrefs.GetFloat(VOLUME_KEY);
                return Volume;
            }

            public void SetEnabled(bool state)
            {
                PlayerPrefs.SetInt(ENABLED_KEY, state ? 1 : 0);
                Enabled = state;
                ChangeEnableEvent?.Invoke(Enabled);
            }

            private bool GetEnabledPlayerPrefs()
            {
                if(PlayerPrefs.HasKey(ENABLED_KEY))
                    return PlayerPrefs.GetInt(ENABLED_KEY) == 1;
                return Enabled;
            }
        }
        
        [Serializable]
        public class AudioGroup
        {
            public string ID;
            public List<AudioClip> Clips;

            public AudioClip GetRandomClip()
            {
                return Clips[Random.Range(0, Clips.Count)];
            }
        }
    }

}
