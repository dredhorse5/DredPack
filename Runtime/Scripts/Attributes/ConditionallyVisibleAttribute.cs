using UnityEngine;

public class ConditionallyVisibleAttribute : MonoBehaviour
{
    public string propertyName { get; }
        
    public ConditionallyVisibleAttribute(string propName)
    {
        propertyName = propName;
    }
}
