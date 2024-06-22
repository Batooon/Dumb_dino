using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DrawIfAttribute))]
public class DrawIfPropertyDrawer : PropertyDrawer
{
    private DrawIfAttribute _attribute;
    private SerializedProperty _targetField;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property) == false && _attribute.DisableType == DrawIfAttribute.DisablingType.Hide)
            return 0f;
        
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
        {
            EditorGUI.PropertyField(position, property);
        }
        else if (_attribute.DisableType == DrawIfAttribute.DisablingType.ReadOnly)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property);
            GUI.enabled = true;
        }
    }

    private bool ShouldShow(SerializedProperty property)
    {
        _attribute=attribute as DrawIfAttribute;
        var path = property.propertyPath.Contains(".")
            ? System.IO.Path.ChangeExtension(property.propertyPath, _attribute.ComparedPropertyName)
            : _attribute.ComparedPropertyName;
        _targetField = property.serializedObject.FindProperty(path);
        if (_targetField == null)
        {
            Debug.LogError($"Cannot find property with name: {path}");
            return true;
        }

        if (_targetField.type != "bool")
        {
            Debug.LogError($"Error: {_targetField.type} is not supported of {path}");
            return true;
        }

        return _targetField.boolValue.Equals(_attribute.ComparedValue);
    }
}
