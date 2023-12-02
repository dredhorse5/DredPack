using DredPack.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if  UNITY_EDITOR
namespace DredPack
{
    public class MenuItemEditor : MonoBehaviour
    {
        private static string fontPath = "Fonts/Troika_Regular_Font";
        private static string btnSpritePath = "Sprites/Btn_MainButton_White";
        private static string swithcerPrefabPath = "Prefabs/Switcher";
        private static string buttonPrefabPath = "Prefabs/Button";



        [MenuItem("GameObject/DredPackUI/Window", false, 10)]
        public static void CreateObject_Window(MenuCommand menuCommand)
        {
            var rectTransform = CreateUiElement("Window");


            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;

            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            rectTransform.sizeDelta = Vector2.zero;

            rectTransform.anchoredPosition = Vector2.zero;


            rectTransform.gameObject.AddComponent<Window>();


            var image = rectTransform.gameObject.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.5f);


            GameObjectUtility.SetParentAndAlign(rectTransform.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(rectTransform.gameObject, "Create " + rectTransform.gameObject.name);

            Selection.activeObject = rectTransform.gameObject;
        }

        #region Buttons Creator
        
        [MenuItem("GameObject/DredPackUI/Button", false, 10)]
        static void CreateButton(MenuCommand menuCommand)
        {
            var gameObject = InstantiateUiPrefab(buttonPrefabPath, menuCommand);

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeObject = gameObject;
        }
        #endregion

        [MenuItem("GameObject/DredPackUI/Switcher", false, 10)]
        static void CreateSwitcher(MenuCommand menuCommand)
        {
            var switcher = InstantiateUiPrefab(swithcerPrefabPath, menuCommand);
            
            Undo.RegisterCreatedObjectUndo(switcher, "Create " + switcher.name);
            Selection.activeObject = switcher;
        }

        [MenuItem("Edit/Toggle DredWindow #&q")]
        static void ToggleSelectedWindow()
        {
            if ((Selection.activeObject is GameObject gm))
            {
                if (gm.TryGetComponent(out Window window))
                    window.Switch("Instantly");
            }
        }

        #region Help

        public static RectTransform CreateUiElement(string name, Transform parent = null)
        {
            GameObject go = new GameObject(name);
            go.layer = 5;
            if (!parent)
            {
                //CanvasSettings
                var canvas = GetCanvas();
                go.transform.parent = canvas.transform;
            }
            else
            {
                go.transform.parent = parent;
            }

            //rect transform settings
            var rectTransform = go.AddComponent<RectTransform>();

            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;


            return rectTransform;
        }

        private static Canvas GetCanvas()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (!canvas)
            {
                GameObject canvasGO = new GameObject("Canvas");
                canvasGO.layer = 5;
                canvas = canvasGO.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGO.AddComponent<CanvasScaler>();
                canvasGO.AddComponent<GraphicRaycaster>();
            }

            return canvas;
        }

        private static Transform GetContextCanvasParent(MenuCommand menuCommand)
        {
            var parent = menuCommand.context ? (menuCommand.context as GameObject).transform : null;
            return parent ?? GetCanvas().transform;
        }

        private static GameObject InstantiateUiPrefab(string path, MenuCommand menuCommand, Vector3 pos = new Vector3(), bool unpack = true)
        {
            var prefab = Resources.Load(path) as GameObject;
            var gameobject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if(unpack)
                PrefabUtility.UnpackPrefabInstance(gameobject,PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            
            gameobject.transform.SetParent(GetContextCanvasParent(menuCommand));
            gameobject.transform.localPosition = pos;
            gameobject.transform.localScale = Vector3.one;
            
            return gameobject;
        }
        

        #endregion
    }
}

#endif
