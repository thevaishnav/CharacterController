using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using KSRecs.Editor.Extensions;
using UnityEditor;
#endif


[ExecuteAlways]
public class ArrangeChildren : MonoBehaviour
{
    public enum ArrangeMode
    {
        Position,
        Rotation,
        Scale
    }

    [SerializeField] private ArrangeMode actsOn = ArrangeMode.Position;
    [SerializeField] public ArrangeOnAxis X, Y, Z;
    private Action updateAction;

    public ArrangeMode ActOn
    {
        get => actsOn;
        set
        {
            this.actsOn = value;
            SetUpdateAction();
        }
    }

    private void Start()
    {
        SetUpdateAction();
        Debug.Log($"Update Action: {updateAction}");
    }

    private void Update()
    {
        #if UNITY_EDITOR
        SetUpdateAction();
        #endif
        Debug.Log($"Update Action: {updateAction}");
        updateAction.Invoke();
    }

    private void SetUpdateAction()
    {
        if (this.actsOn == ArrangeMode.Position) updateAction = ActPosition;
        else if (this.actsOn == ArrangeMode.Rotation) updateAction = ActRotation;
        else if (this.actsOn == ArrangeMode.Scale) updateAction = ActScale;
    }

    private void ActPosition()
    {
        int cc = transform.childCount;
        int counter = 0;
        Vector3 current;
        foreach (Transform child in transform)
        {
            current = child.localPosition;
            current.x = X.GetNextValue(counter, cc, current.x);
            current.y = Y.GetNextValue(counter, cc, current.y);
            current.z = Z.GetNextValue(counter, cc, current.z);
            child.localPosition = current;
            counter++;
        }
        X.isScattering = false;
        Y.isScattering = false;
        Z.isScattering = false;
    }

    private void ActRotation()
    {
        int cc = transform.childCount;
        int counter = 0;
        Vector3 current;
        foreach (Transform child in transform)
        {
            current = child.localRotation.eulerAngles;
            current.x = X.GetNextValue(counter, cc, current.x);
            current.y = Y.GetNextValue(counter, cc, current.y);
            current.z = Z.GetNextValue(counter, cc, current.z);
            child.localRotation = Quaternion.Euler(current);
            counter++;
        }
        X.isScattering = false;
        Y.isScattering = false;
        Z.isScattering = false;
    }

    private void ActScale()
    {
        int cc = transform.childCount;
        int counter = 0;
        Vector3 current;
        foreach (Transform child in transform)
        {
            current = child.localScale;
            current.x = X.GetNextValue(counter, cc, current.x);
            current.y = Y.GetNextValue(counter, cc, current.y);
            current.z = Z.GetNextValue(counter, cc, current.z);
            child.localScale = current;
            counter++;
        }

        X.isScattering = false;
        Y.isScattering = false;
        Z.isScattering = false;
    }
}


[System.Serializable]
public class ArrangeOnAxis
{
    public bool isScattering;

    public enum ArrangeMode
    {
        DontArrange,
        ZigZag,
        StartStep,
        StartEnd,
        CenterStep,
        Scatter
    }

    public ArrangeMode mode = ArrangeMode.DontArrange;
    public float val1;
    public float val2;
    public int alternate;

    public float GetNextValue(int counter, int totalCount, float defVal)
    {
        if (alternate > 1) counter /= alternate;
        if (isScattering)
        {
            return Random.Range(val1, val2);
        }

        if (mode == ArrangeMode.DontArrange) return defVal;
        if (mode == ArrangeMode.ZigZag) return ((((float)counter) % 2f) == 0f) ? val1 : val2;
        if (mode == ArrangeMode.StartStep) return val2 * counter + val1;
        if (mode == ArrangeMode.StartEnd) return ((val2 - val1) * counter / totalCount) + val1;
        if (mode == ArrangeMode.CenterStep) return val1 + (val2 * (counter + 0.5f - ((float)totalCount) / 2f));
        return defVal;
    }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ArrangeOnAxis))]
public class ArrangeOnAxisDrawer : PropertyDrawer
{
    private static float LINE_HEIGHT = 18f;
    private static float LINE_GAP = 3f;
    SerializedProperty mode;
    SerializedProperty val1;
    SerializedProperty val2;
    SerializedProperty alternate;
    SerializedProperty isScattering;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (mode == null) mode = property.FindPropertyChild("mode");
        if (val1 == null) val1 = property.FindPropertyChild("val1");
        if (val2 == null) val2 = property.FindPropertyChild("val2");
        if (alternate == null) alternate = property.FindPropertyChild("alternate");
        if (isScattering == null) isScattering = property.FindPropertyChild("isScattering");
        if (property.isExpanded)
        {
            if (mode.enumValueIndex == 5) return LINE_HEIGHT * 6 + LINE_GAP * 5;
            return LINE_HEIGHT * 5 + LINE_GAP * 4;
        }

        return LINE_HEIGHT;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Rect rect = new Rect(position.x, position.y, position.width, LINE_HEIGHT);
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label);
        if (!property.isExpanded) return;
        rect.y += LINE_HEIGHT + LINE_GAP;
        EditorGUI.PropertyField(rect, mode);
        rect.y += LINE_HEIGHT + LINE_GAP;
        EditorGUI.PropertyField(rect, val1);
        rect.y += LINE_HEIGHT + LINE_GAP;
        EditorGUI.PropertyField(rect, val2);
        rect.y += LINE_HEIGHT + LINE_GAP;
        EditorGUI.PropertyField(rect, alternate);
        rect.y += LINE_HEIGHT + LINE_GAP;
        if (mode.enumValueIndex == 5 && GUI.Button(rect, "Scatter"))
        {
            isScattering.boolValue = true;
        }

        EditorGUI.EndProperty();
    }
}
#endif