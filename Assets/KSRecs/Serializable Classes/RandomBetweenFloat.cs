using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif


[Serializable]
public class RandomBetweenFloats 
{
    
    [SerializeField] protected float minimum;
    [SerializeField] protected float maximum;

    public float value
    {
        get
        {
            float factor = Random.value;
            return factor * maximum + (1 - factor) * minimum;
        }
    }
    public static implicit operator float(RandomBetweenFloats randomBetween) => randomBetween.value;
}

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
public class RandomBtwnRangeAttribute : PropertyAttribute
{
    public float minimum = -1f;
    public float maximum = 1f;

    
    
    public RandomBtwnRangeAttribute(float minimum, float maximum)
    {
        this.minimum = Mathf.Min(minimum);
        this.maximum = Mathf.Max(maximum);
    }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(RandomBtwnRangeAttribute))]
public class RangeForRandomBetweenDrawer : PropertyDrawer
{
    private float ONE_LINE = 18f;
    Rect theRect;
    private float minVal;
    private float maxVal;
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ONE_LINE;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty startingPoint = FindPropertyChild(property, "minimum"); 
        SerializedProperty endingPoint = FindPropertyChild(property, "maximum"); 
        minVal = startingPoint.floatValue;
        maxVal = endingPoint.floatValue;
        
        theRect = new Rect(position.x, position.y, position.width, ONE_LINE);
        theRect.width -= 63;
        
        EditorGUI.BeginChangeCheck();
        RandomBtwnRangeAttribute atter = (RandomBtwnRangeAttribute)attribute;
        EditorGUI.MinMaxSlider(theRect, label, ref minVal, ref maxVal, atter.minimum, atter.maximum);
        theRect.x += theRect.width + 3;
        theRect.width = 60f;
        EditorGUI.LabelField(theRect, $"{minVal:0.00} : {maxVal:0.00}");
        if (EditorGUI.EndChangeCheck())
        {
            startingPoint.floatValue = minVal;
            endingPoint.floatValue = maxVal;
        }
    }

    public static SerializedProperty FindPropertyChild(SerializedProperty property, string childName)
    {
        string parentPath = property.propertyPath;
        SerializedProperty iterator = property.Copy();
        while (iterator.Next(true))
        {
            if (iterator.name == childName && iterator.propertyPath.Contains(parentPath))
            {
                return iterator;
            }
        }

        return null;
    }
}


[CustomPropertyDrawer(typeof(RandomBetweenFloats))]
public class RandomBetweenFloatsDrawer : PropertyDrawer
{
    private float ONE_LINE = 18f;
    Rect theRect;
    private float minVal;
    private float maxVal;
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ONE_LINE;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty startingPoint = FindPropertyChild(property, "minimum"); 
        SerializedProperty endingPoint = FindPropertyChild(property, "maximum");
        float size = (position.width - EditorGUIUtility.labelWidth - 23f) / 2f;
        theRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, ONE_LINE);
        EditorGUI.LabelField(theRect, label);
        
        theRect.x += EditorGUIUtility.labelWidth;
        theRect.width = size;
        EditorGUI.BeginChangeCheck();
        startingPoint.floatValue = EditorGUI.FloatField(theRect, startingPoint.floatValue);
        if (EditorGUI.EndChangeCheck() && startingPoint.floatValue > endingPoint.floatValue)
        {
            startingPoint.floatValue = endingPoint.floatValue;
        }
        theRect.x += size + 4f;
        theRect.width = 10f;
        EditorGUI.LabelField(theRect, " - ");

        theRect.width = size;
        theRect.x += 19f;
        EditorGUI.BeginChangeCheck();
        endingPoint.floatValue = EditorGUI.FloatField(theRect, endingPoint.floatValue);
        if (EditorGUI.EndChangeCheck() && startingPoint.floatValue > endingPoint.floatValue)
        {
            endingPoint.floatValue = startingPoint.floatValue;
        }
    }

    public static SerializedProperty FindPropertyChild(SerializedProperty property, string childName)
    {
        string parentPath = property.propertyPath;
        SerializedProperty iterator = property.Copy();
        while (iterator.Next(true))
        {
            if (iterator.name == childName && iterator.propertyPath.Contains(parentPath))
            {
                return iterator;
            }
        }

        return null;
    }
}
#endif