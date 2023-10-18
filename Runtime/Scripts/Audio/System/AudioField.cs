using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
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
        public FindAudioMethods FindAudioMethod;
        [ShowIf(nameof(FindAudioMethod), FindAudioMethods.Name)][AllowNesting]
        public string GroupID;
        [ShowIf(nameof(FindAudioMethod), FindAudioMethods.Clips)][AllowNesting]
        public List<AudioClip> Clip;
        [AllowNesting]
        public AdvancedSettings Advanced = new AdvancedSettings(); 
        
        public enum FindAudioMethods { Clips, Name }

        public AudioManager.AudioByType audioByType { get; private set; }
        private float globalVolume = 1f;
        private bool isMuted = false;

        private MonoBehaviour owner;
        [NonSerialized] 
        private bool isInited = false;

        private int properties;

        public AudioField()
        {
            Type = AudioManager.AudioTypes.Audio;
            LocalVolume = 1f;
            Advanced.OneShot = true; 
        }

        public AudioField(AudioManager.AudioTypes type, float localVolume = 1f)
        {
            Type = type;
            LocalVolume = localVolume;
            Advanced.OneShot = true;
        }

        ~AudioField()
        {
            if(audioByType)
            {
                if(audioByType.ChangeMuteEvent)
                    audioByType.ChangeMuteEvent.RemoveListener(OnMuteChange);
                if(audioByType.ChangeVolumeEvent)
                    audioByType.ChangeVolumeEvent.RemoveListener(OnGlobalVolumeChange);
            }
        }

        public void Initialize(MonoBehaviour owner)
        {
            if (isInited)
                return;
            this.owner = owner;
            audioByType = AudioManager.GetAudioType(Type);
            audioByType.ChangeMuteEvent.AddListener(OnMuteChange);
            audioByType.ChangeVolumeEvent.AddListener(OnGlobalVolumeChange);
            if (Advanced.CustomAudioSource && !Advanced.LocalAudioSource)
                Advanced.LocalAudioSource = NewAudioSource(Clip[0].name + "AudioSource", null);
            isInited = true;
        }

        public AudioClip GetClip(int index = -1)
        {
            if (FindAudioMethod == FindAudioMethods.Name)
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
            if (audioByType.Muted)
                return;
            if(clip == null)
                return;
            if (Advanced.LocalAudioSource)
            {
                var volume = audioByType.Volume * LocalVolume * _volume;
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


        protected virtual void OnMuteChange(bool state)
        {
            isMuted = state;
            if (Advanced.LocalAudioSource)
                Advanced.LocalAudioSource.volume = (state ? 0f : 1f) * audioByType.Volume * LocalVolume;
        }

        protected virtual void OnGlobalVolumeChange(float volume)
        {
            globalVolume = volume;
            if (Advanced.LocalAudioSource)
                Advanced.LocalAudioSource.volume = globalVolume * LocalVolume * (isMuted ? 0f : 1f);
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
        
        /*#region EDITOR

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(AudioField)),CanEditMultipleObjects]
        public class AudioFieldEditor : PropertyDrawer
        {
            SerializedProperty Type;
            SerializedProperty LocalVolume;
            SerializedProperty GlobalAudioSource;
            SerializedProperty OneShot;
            SerializedProperty LocalAudioSource;
            SerializedProperty Clip;
            private bool shown;
            private static float SingleLineHeight => EditorGUIUtility.singleLineHeight + 2f;
            private static float posX => 10;
            private int elements;
            private AudioField target(SerializedProperty property) => fieldInfo.GetValue(property.serializedObject.targetObject) as AudioField;


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                Init(property);
                EditorGUI.BeginProperty(position, label, property);
                
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                General(position, property, label);
                
                EditorGUI.indentLevel = indent;
                EditorGUI.EndProperty();
            }

            private void General(Rect position, SerializedProperty property, GUIContent label)
            {
                var _t = target(property);
                position.height = EditorGUIUtility.singleLineHeight;
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
                if(property.isExpanded)
                { 
                    EditorGUI.PropertyField( new Rect(position.x + posX, position.y + SingleLineHeight, position.width - posX, position.height), Type);
                    EditorGUI.PropertyField(new Rect(position.x + posX, position.y + SingleLineHeight * 2f, position.width - posX, position.height), LocalVolume);
                    //EditorGUI.PropertyField(new Rect(position.x + posX, position.y + SingleLineHeight * 3f, position.width - posX, position.height), GlobalAudioSource);
                    //_t.properties = 3;
                }

            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                if(property.isExpanded)
                {
                    var audioField = fieldInfo.GetValue(property.serializedObject.targetObject) as AudioField;
                    var i = target(property).properties; 
                    target(property).properties = 0;
                    return (SingleLineHeight * i) + SingleLineHeight;
                }
                return base.GetPropertyHeight(property, label);
            }

            private void Init(SerializedProperty property)
            {
                Type = property.FindPropertyRelative("Type");
                LocalVolume = property.FindPropertyRelative("LocalVolume");
                GlobalAudioSource = property.FindPropertyRelative("GlobalAudioSource");
                OneShot = property.FindPropertyRelative("OneShot");
                LocalAudioSource = property.FindPropertyRelative("LocalAudioSource");
                Clip = property.FindPropertyRelative("Clip");
            }
        }
#endif

        #endregion*/
    }
}
