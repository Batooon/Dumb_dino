using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property|AttributeTargets.Field, AllowMultiple = true)]
public class DrawIfAttribute : PropertyAttribute
{
    public enum DisablingType
    {
        ReadOnly,
        Hide
    }
    
    public string ComparedPropertyName { get; private set; }
    public object ComparedValue { get; private set; }
    public DisablingType DisableType { get; private set; }

    public DrawIfAttribute(string comparedPropertyName, object comparedValue,
        DisablingType disablingType = DisablingType.Hide)

    {
        ComparedPropertyName = comparedPropertyName;
        ComparedValue = comparedValue;
        DisableType = disablingType;
    }
}
