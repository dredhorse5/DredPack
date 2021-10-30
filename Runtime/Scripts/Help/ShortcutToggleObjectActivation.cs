using UnityEngine;
using System.Collections;
using UnityEditor;

public static class ShortcutToggleObjectActivation 
{
    // LEFT ALT + LEFT SHIFT + A to toggle active state of selected GameObjects
    [MenuItem("Shortcuts/Toggle Selected GameObjects Active #&a")]
    static void SaveGameOpenFolder()
    {
        if (Selection.gameObjects.Length > 1) {
            foreach (var item in Selection.gameObjects) { 
                item.SetActive(!item.activeSelf);
            }
        }
	else if (Selection.activeGameObject != null) {
            Selection.activeGameObject.SetActive(!Selection.activeGameObject.activeSelf);
        }
    }
}
