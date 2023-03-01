using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DredPack.Audio
{
    public class AudioButton : MonoBehaviour
    {
        public AudioField Audio;

        private void Start()
        {
            if (TryGetComponent(out Button btn))
                btn.onClick.AddListener(PlayAudio);
            if (TryGetComponent(out Toggle tgl))
                tgl.onValueChanged.AddListener(arg1 => { PlayAudio(); });
        }

        public void PlayAudio()
        {
            Audio.Play();
        }
    }
}