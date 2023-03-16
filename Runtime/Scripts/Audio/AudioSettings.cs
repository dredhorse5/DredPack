using System;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.Audio
{
    ///==========================================
    ///         Made in Pair of slippers
    ///              By dredhorse5
    ///         gmail: dima.titov18@gmail.com
    ///         yandex: dredhorse5@yandex.ru
    ///==========================================
    public class AudioSettings : MonoBehaviour
    {
        public AudioManager.AudioTypes Type;
        [Space]
        public Toggle MuteToggle;
        public DredPack.UI.Switcher MuteSwitcher;
        public Slider VolumeSlider;

        private AudioManager.AudioByType audioByType;
        
        private void Start()
        {
            audioByType = AudioManager.GetAudioType(Type);
            if (MuteSwitcher)
            {
                MuteSwitcher.SwitchEvent.AddListener(SetMute);
                audioByType.ChangeMuteEvent.AddListener(UpdateSwitcher);
                UpdateSwitcher();
            }
            if(MuteToggle)
            {
                MuteToggle.onValueChanged.AddListener(SetMute);
                audioByType.ChangeMuteEvent.AddListener(UpdateToggle);
                UpdateToggle();
            }
            if(VolumeSlider)
            {
                VolumeSlider.onValueChanged.AddListener(SetVolume);
                audioByType.ChangeVolumeEvent.AddListener(UpdateSlider);
                UpdateSlider();
            }
        }

        public void SetMute(bool state)
        {
            audioByType.SetMute(state);
            UpdateToggle();
            UpdateSwitcher();
        }

        public void SetVolume(float volume)
        {
            audioByType.SetVolume(volume);
            UpdateSlider();
        }

        public void UpdateToggle(bool _ = false)
        {
            if(MuteToggle)
                MuteToggle.SetIsOnWithoutNotify(audioByType.Muted);
        }

        public void UpdateSwitcher(bool _ = false)
        {
            if(MuteSwitcher)
                MuteSwitcher.SwitchWithoutNotification(audioByType.Muted);
        }

        public void UpdateSlider(float _ = 1f)
        {
            VolumeSlider.SetValueWithoutNotify(audioByType.Volume);
        }
    }
}
