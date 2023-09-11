using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace DredPack.Audio
{
    ///==========================================
    ///         Made in Pair of slippers
    ///              By dredhorse5
    ///         gmail: dima.titov18@gmail.com
    ///         yandex: dredhorse5@yandex.ru
    ///==========================================
    public class AudioPlayer : MonoBehaviour
    {
        public AudioField Field;
        
        private void Start()
        {
            Field.Initialize(this);
        }

        public void Play()
        {
            Field.Play();
        }
    }
}