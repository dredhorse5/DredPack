using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DredPack.Audio
{
    
    ///==========================================
    ///         Made in Pair of slippers
    ///              By dredhorse5
    ///         gmail: dima.titov18@gmail.com
    ///         yandex: dredhorse5@yandex.ru
    ///==========================================
    [Serializable]
    public class AudioField
    {
        public AudioManager.AudioTypes Type;
        public float LocalVolume = 1f;
        public bool GlobalAudioSource = true;
        public bool OneShot = true;
        public AudioSource LocalAudioSource;
        public List<AudioClip> Clip;
        
        public AudioManager.AudioByType audioByType { get; private set; }
        private float globalVolume = 1f;
        private bool isMuted = false;

        private MonoBehaviour owner;
        [NonSerialized]
        private bool isInited = false;
        public AudioField()
        {
            Type = AudioManager.AudioTypes.Audio;
            LocalVolume = 1f;
            OneShot = true;
        }
        public AudioField(AudioManager.AudioTypes type, float localVolume = 1f)
        {
            Type = type;
            LocalVolume = localVolume;
            OneShot = true;
        }
        ~AudioField()
        {
            audioByType.ChangeMuteEvent.RemoveListener(OnMuteChange);
            audioByType.ChangeVolumeEvent.RemoveListener(OnGlobalVolumeChange);
        }

        public void Initialize(MonoBehaviour owner)
        {
            if(isInited)
                return;
            this.owner = owner;
            audioByType = AudioManager.GetAudioType(Type);
            audioByType.ChangeMuteEvent.AddListener(OnMuteChange);
            audioByType.ChangeVolumeEvent.AddListener(OnGlobalVolumeChange);
            if (!GlobalAudioSource && !LocalAudioSource)
                LocalAudioSource = NewAudioSource(Clip[0].name + "AudioSource", null);
            isInited = true;
        }
        public AudioClip GetClip(int index = -1)
        {
            var audioClips = Clip.FindAll(_ => _ != null);
            if (index < 0)
                index = Random.Range(0, audioClips.Count);
            if(audioClips.Count > 0)
                return audioClips[index];
            return null;
        }
        
        public void Play(float _volume = 1f) => Play(GetClip(),_volume);
        public void Play(int index, float _volume = 1f) => Play(GetClip(index),_volume);
        public void Play(AudioClip clip, float _volume = 1f)
        {
            if(!isInited)
                Initialize(null);
            if(audioByType.Muted)
                return;
            if (LocalAudioSource)
            {
                var volume = audioByType.Volume * LocalVolume * _volume;
                if(OneShot)
                    LocalAudioSource.PlayOneShot(clip,volume);
                else
                {
                    LocalAudioSource.clip = clip;
                    LocalAudioSource.volume = volume;
                    LocalAudioSource.Play();
                }
            }
            else
                AudioManager.Instance.PlayOneShot(this,_volume);
        }

        
        public void SetLocalVolume(float volume)
        {
            LocalVolume = volume;
            OnGlobalVolumeChange(globalVolume);
        }

        
        protected virtual void OnMuteChange(bool state)
        {
            isMuted = state;
            if(LocalAudioSource)
                LocalAudioSource.volume = (state ? 0f : 1f) * audioByType.Volume * LocalVolume;
        }

        protected virtual void OnGlobalVolumeChange(float volume)
        {
            globalVolume = volume;
            if(LocalAudioSource)
                LocalAudioSource.volume = globalVolume * LocalVolume * (isMuted ? 0f : 1f);
        }

        
        
        
        private AudioSource NewAudioSource(string name, AudioClip audio)
        {
            if (!owner)
            {
                Debug.LogError("When you use not global audio source, you should INIT the audio field or SET TO THE INSPECTOR local audio source! Now will be used global audio source");
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
    }
}
