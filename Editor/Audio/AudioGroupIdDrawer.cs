using System.Linq;
using DredPack.Audio;
using DredPack.Audio.Help;
using UnityEditor;
using UnityEngine;

namespace DredPack.DredpackEditor.Audio
{
    [CustomPropertyDrawer(typeof(AudioGroupAttribute))]
    public class AudioGroupIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var audioManager = AudioManager.Instance;
            var sel = EditorGUI.Popup(position,label.text, audioManager.audioGroupNames.ToList().IndexOf(property.stringValue), audioManager.audioGroupNames);
            if (sel < 0)
                sel = 0;

            property.stringValue = audioManager.audioGroupNames[sel];
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}