using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using KSRecs.Editor.Extensions;
using UnityEditor;
#endif


[AttributeUsage(AttributeTargets.Field)]
public class RangForRandomBetweenAttribute : PropertyAttribute
{
    public float minLower {get; private set;}
    public float minUpper {get; private set;}
    public float maxLower {get; private set;}
    public float maxUpper {get; private set;}

    public RangForRandomBetweenAttribute(float bothLower, float bothUpper)
    {
        this.minLower = bothLower;
        this.minUpper = bothUpper;
        this.minLower = bothLower;
        this.minUpper = bothUpper;
    }

    public RangForRandomBetweenAttribute(float minLower, float minUpper, float maxLower, float maxUpper)
    {
        this.minLower = minLower;
        this.minUpper = minUpper;
        this.maxLower = maxLower;
        this.maxUpper = maxUpper;
    }
}


public abstract class RandomBetween<T, TRet>
{
    [SerializeField] protected T minimum;
    [SerializeField] protected T maximum;
    [SerializeField] protected AnimationCurve distribution;

    public abstract TRet value { get; }
}


[Serializable]
public class RandomBetweenInt : RandomBetween<int, int>
{
    public override int value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return (int) (factor * maximum + (1 - factor) * minimum);
        }
    }

    public static implicit operator int(RandomBetweenInt randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenFloat : RandomBetween<float, float>
{
    public override float value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return factor * maximum + (1 - factor) * minimum;
        }
    }

    public static implicit operator float(RandomBetweenFloat randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenDouble : RandomBetween<double, double>
{
    public override double value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return factor * maximum + (1 - factor) * minimum;
        }
    }

    public static implicit operator double(RandomBetweenDouble randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenLong : RandomBetween<long, long>
{
    public override long value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return (long) (factor * maximum + (1 - factor) * minimum);
        }
    }

    public static implicit operator long(RandomBetweenLong randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenV2 : RandomBetween<Vector2, Vector2>
{
    public override Vector2 value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return factor * maximum + (1 - factor) * minimum;
        }
    }

    public static implicit operator Vector2(RandomBetweenV2 randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenV3 : RandomBetween<Vector3, Vector3>
{
    public override Vector3 value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return factor * maximum + (1 - factor) * minimum;
        }
    }

    public static implicit operator Vector3(RandomBetweenV3 randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenV4 : RandomBetween<Vector4, Vector4>
{
    public override Vector4 value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return factor * maximum + (1 - factor) * minimum;
        }
    }

    public static implicit operator Vector4(RandomBetweenV4 randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenColor : RandomBetween<Color, Color>
{
    public override Color value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return factor * maximum + (1 - factor) * minimum;
        }
    }

    public static implicit operator Color(RandomBetweenColor randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenPosition : RandomBetween<Transform, Vector3>
{
    public override Vector3 value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return factor * maximum.position + (1 - factor) * minimum.position;
        }
    }

    public static implicit operator Vector3(RandomBetweenPosition randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenRotations : RandomBetween<Transform, Quaternion>
{
    public override Quaternion value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return Quaternion.Euler(factor * maximum.rotation.eulerAngles + (1 - factor) * minimum.rotation.eulerAngles);
        }
    }

    public static implicit operator Quaternion(RandomBetweenRotations randomBetween) => randomBetween.value;
}


[Serializable]
public class RandomBetweenScales : RandomBetween<Transform, Vector3>
{
    public override Vector3 value
    {
        get
        {
            float factor = distribution.Evaluate(Random.value);
            return factor * maximum.localScale + (1 - factor) * minimum.localScale;
        }
    }

    public static implicit operator Vector3(RandomBetweenScales randomBetween) => randomBetween.value;
}


#if UNITY_EDITOR

// [CustomPropertyDrawer(typeof(RangForRandomBetweenAttribute), true)]
// public class RandomBetweenDrawerProperty : PropertyDrawer
// {
//     SerializedProperty minimum;
//     SerializedProperty maximum;
//     SerializedProperty distribution;
//     
//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         
//         return 18f;
//     }
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//
//         minimum = property.FindPropertyChild("minimum");
//         maximum = property.FindPropertyChild("maximum");
//         distribution = property.FindPropertyChild("distribution");
//
//         EditorGUI.BeginProperty(position, label, property);
//         float oneWidth = (position.width / 5f);
//         Rect rect = new Rect(position.x, position.y, oneWidth, position.height);
//         EditorGUI.PrefixLabel(rect, label);
//         RangForRandomBetweenAttribute rfAttrib = (RangForRandomBetweenAttribute) attribute;
//         if (rfAttrib == null)
//         {
//             DrawDefaults(rect, oneWidth);
//             return;
//         }
//         
//         switch (minimum.propertyType)
//         {
//             case (SerializedPropertyType.Float):
//             {
//                 DrawFloats(rect, oneWidth);
//                 return;
//             }
//             
//             case (SerializedPropertyType.Integer):
//             {
//                 DrawInts(rect, oneWidth);
//                 return;
//             }
//
//             default:
//             {
//                 DrawDefaults(rect, oneWidth);
//                 return;
//             }
//         }
//     }
//
//     private void DrawInts(Rect rect, float oneWidth)
//     {
//         RangForRandomBetweenAttribute rfAttrib = (RangForRandomBetweenAttribute) attribute;
//         
//         rect.x += oneWidth*2 + 5f;
//         rect.width -= 4f;
//         minimum.intValue = EditorGUI.IntSlider(rect, minimum.intValue, (int)rfAttrib.minLower, (int)rfAttrib.minUpper);
//         rect.x += oneWidth;
//         maximum.intValue = EditorGUI.IntSlider(rect, maximum.intValue, (int)rfAttrib.maxLower, (int)rfAttrib.maxUpper);
//         rect.x += oneWidth;
//         EditorGUI.PropertyField(rect, distribution, GUIContent.none);
//         EditorGUI.EndProperty();
//     }
//
//     private void DrawFloats(Rect rect, float oneWidth)
//     {
//         RangForRandomBetweenAttribute rfAttrib = (RangForRandomBetweenAttribute) attribute;
//         
//         rect.x += oneWidth*2 + 5f;
//         rect.width -= 4f;
//         minimum.floatValue = EditorGUI.Slider(rect, minimum.floatValue, rfAttrib.minLower, rfAttrib.minUpper);
//         rect.x += oneWidth;
//         maximum.floatValue = EditorGUI.Slider(rect, maximum.floatValue, rfAttrib.maxLower, rfAttrib.maxUpper);
//         rect.x += oneWidth;
//         EditorGUI.PropertyField(rect, distribution, GUIContent.none);
//         EditorGUI.EndProperty();
//     }
//
//     private void DrawDefaults(Rect rect, float oneWidth)
//     {
//         rect.x += oneWidth*2 + 5f;
//         rect.width -= 4f;
//         EditorGUI.PropertyField(rect, minimum, GUIContent.none);
//         rect.x += oneWidth;
//         EditorGUI.PropertyField(rect, maximum, GUIContent.none);
//         rect.x += oneWidth;
//         EditorGUI.PropertyField(rect, distribution, GUIContent.none);
//     }
// }


[CustomPropertyDrawer(typeof(RandomBetween<,>), true)]
public class RandomBetweenDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty minimum = property.FindPropertyChild("minimum");
        SerializedProperty maximum = property.FindPropertyChild("maximum");
        SerializedProperty distribution = property.FindPropertyChild("distribution");

        EditorGUI.BeginProperty(position, label, property);
        float oneWidth = (position.width / 5f);
        Rect rect = new Rect(position.x, position.y, oneWidth, position.height);
        EditorGUI.PrefixLabel(rect, label);
        rect.x += oneWidth*2 + 5f;
        rect.width -= 4f;
        EditorGUI.PropertyField(rect, minimum, GUIContent.none);
        rect.x += oneWidth;
        EditorGUI.PropertyField(rect, maximum, GUIContent.none);
        rect.x += oneWidth;
        EditorGUI.PropertyField(rect, distribution, GUIContent.none);
        EditorGUI.EndProperty();
    }
}
#endif