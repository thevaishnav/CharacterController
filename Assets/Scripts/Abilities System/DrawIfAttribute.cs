using UnityEngine;
using System;
# if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DrawIfEnumEqualAttribute : PropertyAttribute
{
    public enum ComparisonType
    {
        Equal,
        NotEqual   
    }
    
    public string enumName;
    public int comparedValue;
    public Func<int, bool> CompairTo;

    public DrawIfEnumEqualAttribute(string enumName, int comparedValue)
    {
        this.enumName = enumName;
        this.comparedValue = comparedValue;
        this.CompairTo = (i => i == comparedValue);
    }
    
    public DrawIfEnumEqualAttribute(string enumName, int comparedValue, ComparisonType comparisonType)
    {
        this.enumName = enumName;
        this.comparedValue = comparedValue;
        if (comparisonType == ComparisonType.Equal)
        {
            this.CompairTo = (i => i == comparedValue);
        }
        else
        {
            this.CompairTo = (i => i != comparedValue);
        }
    }
    
}


# if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DrawIfEnumEqualAttribute))]
public class DrawIfPropertyDrawer : PropertyDrawer
{
    private DrawIfEnumEqualAttribute drawIf;
    private SerializedProperty compaired;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (drawIf == null) drawIf = (DrawIfEnumEqualAttribute) attribute;
        if (compaired == null) compaired = property.serializedObject.FindProperty(drawIf.enumName);
        
        if (drawIf.CompairTo(compaired.enumValueIndex))
            return base.GetPropertyHeight(property, label);
        return 0f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (drawIf.CompairTo(compaired.enumValueIndex))
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
# endif
