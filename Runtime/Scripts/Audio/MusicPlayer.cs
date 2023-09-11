using System;
using System.Collections;
using UnityEngine;

namespace DredPack.Audio
{
    ///==========================================
    ///         Made in Pair of slippers
    ///              By dredhorse5
    ///         gmail: dima.titov18@gmail.com
    ///         yandex: dredhorse5@yandex.ru
    ///==========================================
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : GeneralSingleton<MusicPlayer>
    {
        public AudioField Field;
        public float DelayBetweenClips = 2f;

        private AudioSource _audioSource;
        private Coroutine playCor;
        private AudioField nowAudioField;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            Field.Initialize(this);
            Field.Advanced.LocalAudioSource = _audioSource;
            Field.Advanced.OneShot = false;
        }

        private void Update()
        {
            if(playCor == null && !_audioSource.isPlaying)
                Play(DelayBetweenClips);
        }

        public void Play(float delay)
        {
            Play(nowAudioField ?? Field, delay);
        }

        public void Play(AudioField field, float delay)
        {
            if(playCor != null)
                StopCoroutine(playCor);
            playCor = StartCoroutine(IE(delay));
            IEnumerator IE(float delay)
            {
                if(_audioSource.isPlaying)
                    _audioSource.Stop();
                yield return new WaitForSeconds(delay);
                nowAudioField = field;
                nowAudioField.Initialize(this);
                nowAudioField.Advanced.LocalAudioSource = _audioSource;
                nowAudioField.Advanced.OneShot = true;
                nowAudioField.Play();
            }
        }
    }
}