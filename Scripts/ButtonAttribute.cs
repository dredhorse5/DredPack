using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ButtonAttribute : PropertyAttribute
{
	public readonly string MethodName;

	public ButtonAttribute(string MethodName)
	{
		this.MethodName = MethodName;
	}
}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(ButtonAttribute))]
	public class InspectorButtonPropertyDrawer : PropertyDrawer
	{
		private MethodInfo _eventMethodInfo = null;

		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
		ButtonAttribute inspectorButtonAttribute = (ButtonAttribute)attribute;

            float buttonLength = position.width;
            Rect buttonRect = new Rect(position.x, position.y, buttonLength, position.height);
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;

			if (GUI.Button(buttonRect, inspectorButtonAttribute.MethodName))
			{
				System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();
				string eventName = inspectorButtonAttribute.MethodName;

				if (_eventMethodInfo == null)
				{
					_eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}

				if (_eventMethodInfo != null)
				{
					_eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
				}
				else
				{
					Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
				}
			}
		}
	}
#endif