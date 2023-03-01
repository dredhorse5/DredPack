using UnityEngine;

namespace DredPack.Help
{
    

    public class ConditionallyVisibleAttribute : MonoBehaviour
    {
        public string propertyName { get; }

        public ConditionallyVisibleAttribute(string propName)
        {
            propertyName = propName;
        }
    }

}