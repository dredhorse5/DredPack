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

        #region Text Creator

        //[MenuItem("GameObject/DredPackUI/Text/White", false, 10)]
        public static void CreateObject_WhiteText(MenuCommand menuCommand)
        {
            CreateObject_Text(menuCommand, Color.white); //
        }

        //[MenuItem("GameObject/DredPackUI/Text/Black", false, 10)]
        public static void CreateObject_BlackText(MenuCommand menuCommand)
        {
            CreateObject_Text(menuCommand, Color.black);
        }

        public static void CreateObject_Text(MenuCommand menuCommand, Color color)
        {
            var rectTransform = CreateUiElement("Text");

            //rect transform settings
            rectTransform.sizeDelta = new Vector2(360, 60);

            rectTransform.anchorMin = Vector2.one / 2;
            rectTransform.anchorMax = Vector2.one / 2;


            //text settings
            var text = rectTransform.gameObject.AddComponent<Text>();
            text.font = Resources.Load<Font>(fontPath);
            text.text = "New Text";
            text.fontSize = 50;
            text.color = color;
            text.alignment = TextAnchor.MiddleCenter;


            GameObjectUtility.SetParentAndAlign(rectTransform.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(rectTransform.gameObject, "Create " + rectTransform.gameObject.name);
            Selection.activeObject = rectTransform.gameObject;
        }


        #endregion

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
            var switcher = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if(unpack)
                PrefabUtility.UnpackPrefabInstance(switcher,PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            
            switcher.transform.SetParent(GetContextCanvasParent(menuCommand));
            switcher.transform.localPosition = pos;
            return switcher;
        }
        

        #endregion
    }
}

#endif
