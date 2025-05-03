using System; // Necessary for attributes
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)] // This ensures it only applies to methods
public class ButtonAttribute : PropertyAttribute
{
    public string ButtonName;

    public ButtonAttribute(string buttonName = null)
    {
        ButtonName = buttonName;
    }
}