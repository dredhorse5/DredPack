using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemEditor : MonoBehaviour
{
    private static string fontPath = "Fonts/Troika_Regular_Font";
    private static string btnSpritePath = "Sprites/Btn_MainButton_White";


    
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
    
    [MenuItem("GameObject/DredPackUI/Text/White", false, 10)]
    public static void CreateObject_WhiteText(MenuCommand menuCommand)
    {
        CreateObject_Text(menuCommand, Color.white);//
    }
    [MenuItem("GameObject/DredPackUI/Text/Black", false, 10)]
    public static void CreateObject_BlackText(MenuCommand menuCommand)
    {
        CreateObject_Text(menuCommand, Color.black);
    }
    
    public static void CreateObject_Text(MenuCommand menuCommand, Color color)
    {
        var rectTransform = CreateUiElement("Text");
        
        //rect transform settings
        rectTransform.sizeDelta = new Vector2(360, 60);
        
        rectTransform.anchorMin = Vector2.one/2;
        rectTransform.anchorMax = Vector2.one/2;
        
        
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

    
    [MenuItem("GameObject/DredPackUI/Button/White", false, 10)]
    static void CreateObjectButtonWhite(MenuCommand menuCommand)
    {
        CreateObejctButton(menuCommand, Color.white,Color.black, "Button");
    }
    [MenuItem("GameObject/DredPackUI/Button/Red", false, 10)]
    static void CreateObejctButtonRed(MenuCommand menuCommand)
    {
        CreateObejctButton(menuCommand, Color.red,Color.white,"RedButton");
    }
    [MenuItem("GameObject/DredPackUI/Button/Green", false, 10)]
    static void CreateObejctButtonGreen(MenuCommand menuCommand)
    {
        CreateObejctButton(menuCommand, Color.green,Color.white, "GreenButton");
    }
    [MenuItem("GameObject/DredPackUI/Button/Blue", false, 10)]
    static void CreateObejctButtonBlue(MenuCommand menuCommand)
    {
        CreateObejctButton(menuCommand, Color.blue,Color.white, "BlueButton");
    }
    [MenuItem("GameObject/DredPackUI/Button/Light Blue", false, 10)]
    static void CreateObejctButtonLightBlue(MenuCommand menuCommand)
    {
        CreateObejctButton(menuCommand, new Color(0,255,255,255),Color.white, "LightBlueButton");
    }
    [MenuItem("GameObject/DredPackUI/Button/Yellow", false, 10)]
    static void CreateObejctButtonYellow(MenuCommand menuCommand)
    {
        CreateObejctButton(menuCommand, Color.yellow,Color.white,"YellowButton");
    }
    [MenuItem("GameObject/DredPackUI/Button/Purple", false, 10)]
    static void CreateObejctButtonPurple(MenuCommand menuCommand)
    {
        CreateObejctButton(menuCommand, new Color(255,0,255,255),Color.white, "PurpleButton");
    }
    
    
    static void CreateObejctButton(MenuCommand menuCommand, Color btnColor, Color textColor, string name)
    {
        var rectTransform = CreateUiElement(name);
        
        //rect transform settings
        rectTransform.sizeDelta = new Vector2(400, 100);
        
        rectTransform.anchorMin = Vector2.one/2;
        rectTransform.anchorMax = Vector2.one/2;

        
        //Image Settings
        var image = rectTransform.gameObject.AddComponent<Image>();
        image.sprite = Resources.Load<Sprite>(btnSpritePath);
        image.type = Image.Type.Sliced;
        image.color = btnColor;
        
        //button settings
        var button = rectTransform.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        
        
        //text settings
        var rectTransformText = CreateUiElement("Text", rectTransform.transform);
        rectTransformText.anchorMin = Vector2.zero;
        rectTransformText.anchorMax = Vector2.one;
        rectTransformText.offsetMin = new Vector2(0,15);
        rectTransformText.offsetMax = Vector2.zero;
        
        var text = rectTransformText.gameObject.AddComponent<Text>();
        text.font = Resources.Load<Font>(fontPath);
        text.text = "Button";
        text.fontSize = 50;
        text.color = textColor;
        text.alignment = TextAnchor.MiddleCenter;
        
        
        GameObjectUtility.SetParentAndAlign(rectTransform.gameObject, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(rectTransform.gameObject, "Create " + rectTransform.gameObject.name);
        Selection.activeObject = rectTransform.gameObject;
    }

    #endregion

    public static RectTransform CreateUiElement(string name, Transform parent = null)
    {
        GameObject go = new GameObject(name);
        go.layer = 5;
        if (!parent)
        {
            //CanvasSettings
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
    
    
    
    
    /*[MenuItem("MyMenu/Do Something with a Shortcut Key %g")]
    static void DoSomethingWithAShortcutKey()
    {
        Debug.Log("Doing something with a Shortcut Key...");
    }*/
}
