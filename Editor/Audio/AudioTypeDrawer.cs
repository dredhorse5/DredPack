using System;
using System.Linq;
using DredPack.Audio;
using DredPack.Audio.Help;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PropertyAttribute = NUnit.Framework.PropertyAttribute;

namespace DredPack.DredpackEditor.Audio
{
    [CustomPropertyDrawer(typeof(AudioTypeAttribute))]
    public class AudioTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var audioManager = AudioManager.Instance;
            var sel = EditorGUI.Popup(position,label.text, audioManager.audioTypesNames.ToList().IndexOf(property.stringValue), audioManager.audioTypesNames);
            if (sel < 0)
                sel = 0;

            property.stringValue = audioManager.audioTypesNames[sel];
            property.serializedObject.ApplyModifiedProperties();
        }
    }

}