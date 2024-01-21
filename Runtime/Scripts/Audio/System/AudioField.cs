using System;
using System.Collections.Generic;
using DredPack.Audio.Help;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DredPack.Audio
{
    ///==========================================
    ///         Made by dredhorse5
    ///         gmail: dima.titov18@gmail.com
    ///         yandex: dredhorse5@yandex.ru
    ///==========================================
    [Serializable]
    public class AudioField
    {
        [AudioType]
        public string Type;
        [Range(0,1f)]
        public float LocalVolume = 1f;
        public FindAudioMethods FindAudioMethod;
        [AudioGroup]
        public string GroupID;
        public List<AudioClip> Clip;
        [AllowNesting]
        public AdvancedSettings Advanced = new AdvancedSettings(); 
        
        public enum FindAudioMethods { Clips, GroupID }

        public AudioManager.AudioByType audioByType { get; private set; }
        private float globalVolume = 1f;
        private bool isEnabled = false;

        private MonoBehaviour owner;
        [NonSerialized] 
        private bool isInited = false;

        public AudioField()
        {
            Type = "Audio";
            LocalVolume = 1f;
            Advanced.OneShot = true; 
        }

        public AudioField(string type, float localVolume = 1f)
        {
            Type = type;
            LocalVolume = localVolume;
            Advanced.OneShot = true;
        }

        ~AudioField()
        {
            if(audioByType != null)
            {
                if(audioByType.ChangeEnableEvent != null)
                    audioByType.ChangeEnableEvent.RemoveListener(OnEnabledChanged);
                if(audioByType.ChangeVolumeEvent != null)
                    audioByType.ChangeVolumeEvent.RemoveListener(OnGlobalVolumeChange);
            }
        }

        public void Initialize(MonoBehaviour owner)
        {
            if (isInited)
                return;
            this.owner = owner;
            audioByType = AudioManager.GetAudioType(Type);
            audioByType.ChangeEnableEvent.AddListener(OnEnabledChanged);
            audioByType.ChangeVolumeEvent.AddListener(OnGlobalVolumeChange);
            if (Advanced.CustomAudioSource && !Advanced.LocalAudioSource)
                Advanced.LocalAudioSource = NewAudioSource(Clip[0].name + "AudioSource", null);
            isInited = true;
        }

        public AudioClip GetClip(int index = -1)
        {
            if (FindAudioMethod == FindAudioMethods.GroupID)
                return AudioManager.Instance.GetClipFromGroup(GroupID);
            var audioClips = Clip.FindAll(_ => _ != null);
            if (index < 0)
                index = Random.Range(0, audioClips.Count);
            if (audioClips.Count > 0)
                return audioClips[index];
            return null;
        }

        public void Play(float _volume = 1f) => Play(GetClip(), _volume);
        public void Play(int index, float _volume = 1f) => Play(GetClip(index), _volume);

        public void Play(string audioGroupID) => Play(AudioManager.Instance.GetClipFromGroup(audioGroupID),1f);
        public void Play(string audioGroupID, float volume = 1f) => Play(AudioManager.Instance.GetClipFromGroup(audioGroupID),volume);
        public void Play(AudioClip clip) => Play(clip, 1f);
        public void Play(AudioClip clip, float _volume = 1f)
        {
            if (!isInited)
                Initialize(null);
            if(clip == null)
                return;
            if (Advanced.LocalAudioSource)
            {
                var volume = audioByType.Volume * LocalVolume * _volume * (audioByType.Enabled ? 1f : 0f);
                if (Advanced.OneShot)
                    Advanced.LocalAudioSource.PlayOneShot(clip, volume);
                else
                {
                    Advanced.LocalAudioSource.clip = clip;
                    Advanced.LocalAudioSource.volume = volume;
                    Advanced.LocalAudioSource.Play();
                }
            }
            else
                AudioManager.Instance.PlayOneShot(this, _volume);
        }


        public void SetLocalVolume(float volume)
        {
            LocalVolume = volume;
            OnGlobalVolumeChange(globalVolume);
        }


        protected virtual void OnEnabledChanged(bool state)
        {
            isEnabled = state;
            if (Advanced.LocalAudioSource)
                Advanced.LocalAudioSource.volume = (state ? 1f : 0f) * audioByType.Volume * LocalVolume;
        }

        protected virtual void OnGlobalVolumeChange(float volume)
        {
            globalVolume = volume;
            if (Advanced.LocalAudioSource)
                Advanced.LocalAudioSource.volume = globalVolume * LocalVolume * (isEnabled ? 0f : 1f);
        }




        private AudioSource NewAudioSource(string name, AudioClip audio)
        {
            if (!owner)
            {
                Debug.LogError(
                    "When you use not global audio source, you should INIT the audio field or SET TO THE INSPECTOR local audio source! Now will be used global audio source");
                return null;
            }

            var folder = owner.transform.Find("Audio Folder");
            if (!folder)
            {
                folder = new GameObject("Audio Folder").transform;
                folder.parent = owner.transform;
                folder.localPosition = Vector3.zero;
            }

            var go = new GameObject(name);
            go.transform.parent = folder.transform;
            go.transform.localPosition = Vector3.zero;

            var source = go.AddComponent<AudioSource>();
            source.clip = audio;
            source.loop = true;
            source.spatialBlend = 1f;
            source.minDistance = 38f;
            source.maxDistance = 130f;
            return source;
        }

        [Serializable]
        public class AdvancedSettings
        {
            public bool CustomAudioSource = false;
            [ShowIf(nameof(CustomAudioSource))][AllowNesting]
            public AudioSource LocalAudioSource;
            [ShowIf(nameof(CustomAudioSource))][AllowNesting]
            public bool OneShot = true;
        }
    }
}
