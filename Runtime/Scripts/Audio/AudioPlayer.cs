using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace DredPack.Audio
{
    ///==========================================
    ///         Made by dredhorse5
    ///         gmail: dima.titov18@gmail.com
    ///         yandex: dredhorse5@yandex.ru
    ///==========================================
    public class AudioPlayer : MonoBehaviour
    {
        public bool PlayOnAwake = false;
        public float Delay = 0f;
        public AudioField Field;
        
        private void Start()
        {
            Field.Initialize(this);
            if(PlayOnAwake)
                Play();
        }
        
        private void OnEnable()
        {
            if(PlayOnAwake)
                Play();
        }

        [Button()]
        public void Play()
        {
            if (Delay > 0.001f)
                PlayDelayed(Delay);
            else
                Field.Play();
        }

        public void PlayDelayed(float delay)
        {
            StartCoroutine(IE(delay));

            IEnumerator IE(float delay)
            {
                if(Delay > 0.001f)
                    yield return new WaitForSeconds(delay);
                Field.Play();
            }
        }
    }
}
