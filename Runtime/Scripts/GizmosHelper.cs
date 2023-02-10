using UnityEngine;

public static class GizmosHelper
{
#if UNITY_EDITOR
    
    public static void DrawString(string text, Vector3 worldPos, Color? color = null)
    {
        UnityEditor.Handles.BeginGUI();
        if (color.HasValue) GUI.color = color.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        UnityEditor.Handles.EndGUI();
    }

    public static void DrawArrow(Vector3 pos, Vector3 direction, Color? color = null, float arrowHeadLength = 0.25f,
        float arrowHeadAngle = 20.0f)
    {
        if (color.HasValue)
            Gizmos.color = color.Value;
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                        new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                       new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }
#endif
}

