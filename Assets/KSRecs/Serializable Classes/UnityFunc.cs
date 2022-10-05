using System;
using System.Reflection;
using KSRecs.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using KSRecs.Editor.Extensions;
#endif

[Serializable]
public struct UnityFunc<T> where T : new()
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string targetCompFunc;
    [SerializeField] private ArgumentCache defaultArgument;

    private MethodInfo methodInfo;
    private Component component;
    
    #if UNITY_EDITOR
    [SerializeField] private T ___EDITOR_ONLY_VARIABLE___;
    [SerializeField] private int currentIndex;
    #endif

    private bool Init()
    {
        try
        {
            string targetComponent, targetFunction;

            if (string.IsNullOrEmpty(targetCompFunc)) 
                return false;

            
            int index = targetCompFunc.IndexOf("/");
            targetComponent = targetCompFunc.Substring(0, index);
            targetFunction = targetCompFunc.Substring(index + 1);
            targetFunction = targetFunction.Substring(0, targetFunction.LastIndexOf(" "));

            component = targetObject.GetComponent(targetComponent);
            methodInfo = component.GetType().GetMethod(targetFunction);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public T Invoke()
    {
        TryInvoke(out T t);
        return t;
    }

    public bool TryInvoke(out T value)
    {
        if (methodInfo == null || component == null)
        {
            bool isSuccess = Init();
            if (isSuccess) value = (T) methodInfo.Invoke(component, defaultArgument.GetArray());
            else value = new T();
            return isSuccess;
        }
        
        value = (T) methodInfo.Invoke(component, defaultArgument.GetArray());
        return true;
    }
}


[Serializable]
public class ArgumentCache
{
    public int intArgument;
    public float floatArgument;
    public string stringArgument;
    public bool boolArgument;
    public Object objectArgument;
    public ArgumentReferanceType ReferanceType;

    public object[] GetArray()
    {
        if (ReferanceType == ArgumentReferanceType.Void) return null;
        if (ReferanceType == ArgumentReferanceType.Int) return new object[] {intArgument};
        if (ReferanceType == ArgumentReferanceType.Float) return new object[] {floatArgument};
        if (ReferanceType == ArgumentReferanceType.String) return new object[] {stringArgument};
        if (ReferanceType == ArgumentReferanceType.Bool) return new object[] {boolArgument};
        if (ReferanceType == ArgumentReferanceType.UnityObject) return new object[] {objectArgument};
        return null;
    }
}

public enum ArgumentReferanceType
{
    Void = 0,
    Int = 1,
    Float = 2,
    String = 3,
    Bool = 4,
    UnityObject = 5
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(UnityFunc<>))]
public class UnityFuncDrawer : PropertyDrawer
{
    private interface IMethodArgInfo
    {
        public void Draw(Rect rect, SerializedProperty defaultArgument, SerializedProperty referanceType);
        public void SetDefault(SerializedProperty property);
    }

    private struct MethodArgsInfo<T> : IMethodArgInfo
    {
        public T currentValue;
        public Func<Rect, string, T, T> DrawerAction;
        public Action<SerializedProperty, T> ValueSetterAction;
        public string label;
        public int enumReferanceValue;

        public MethodArgsInfo(int enumReferanceValue, string label, T currentValue, Func<Rect, string, T, T> drawerAction, Action<SerializedProperty, T> valueSetterAction)
        {
            this.enumReferanceValue = enumReferanceValue;
            this.label = label;
            this.currentValue = currentValue;
            DrawerAction = drawerAction;
            ValueSetterAction = valueSetterAction;
        }

        public void Draw(Rect rect, SerializedProperty defaultArgument, SerializedProperty referanceType)
        {
            EditorGUI.BeginChangeCheck();
            currentValue = DrawerAction.Invoke(rect, label, currentValue);
            if (EditorGUI.EndChangeCheck()) ValueSetterAction.Invoke(defaultArgument, currentValue);
            referanceType.enumValueIndex = enumReferanceValue;
            LogThis($"MethodArgsInfo.Draw({(ArgumentReferanceType) enumReferanceValue})");
        }

        public void SetDefault(SerializedProperty property)
        {
            ValueSetterAction.Invoke(property, currentValue);
            LogThis($"MethodArgsInfo.SetDefault({property.displayName} set to {currentValue})");
        }
    }

    private static class ValueSetters
    {
        public static void Set_int(SerializedProperty property, int value)
        {
            if (property == null)
            {
                LogThis("ValueSetter.Set_int(Setter got null prop)");
                return;
            }

            property.FindPropertyChild("intArgument").intValue = value;
            LogThis("ValueSetter.Set_int(Value Set int)");
        }

        public static void Set_float(SerializedProperty property, float value)
        {
            if (property == null)
            {
                LogThis("ValueSetter.Set_float(Setter got null prop)");
                return;
            }

            property.FindPropertyChild("floatArgument").floatValue = value;
            LogThis("ValueSetter.Set_float(Value Set float)");
        }

        public static void Set_string(SerializedProperty property, string value)
        {
            if (property == null)
            {
                LogThis("ValueSetter.Set_string(Setter got null prop)");
                return;
            }

            property.FindPropertyChild("stringArgument").stringValue = value;
            LogThis("ValueSetter.Set_string(Value Set string)");
        }

        public static void Set_bool(SerializedProperty property, bool value)
        {
            if (property == null)
            {
                LogThis("ValueSetter.Set_bool(Setter got null prop)");
                return;
            }

            property.FindPropertyChild("boolArgument").boolValue = value;
            LogThis("ValueSetter.Set_bool(Value Set bool)");
        }

        public static void Set_Object(SerializedProperty property, Object value)
        {
            if (property == null)
            {
                LogThis("ValueSetter.Set_Object(Setter got null prop)");
                return;
            }

            property.FindPropertyChild("objectArgument").objectReferenceValue = value;
            LogThis("ValueSetter.Set_Object(Value Set Object)");
        }
    }


    public static readonly bool Debugging = false;
    private static float LINE_HEIGHT => 16f;
    private static float LINE_SPACE => 4f;

    SerializedProperty targetObject;
    SerializedProperty targetCompFunc;
    SerializedProperty genericTypeProperty;
    SerializedProperty defaultArgument;
    SerializedProperty referanceType;
    SerializedProperty currentIndex;
    private string[] funcOptions;
    private Dictionary<int, IMethodArgInfo> invokeWithArgOptions;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        targetObject = property.FindPropertyChild("targetObject");
        targetCompFunc = property.FindPropertyChild("targetCompFunc");
        currentIndex = property.FindPropertyChild("currentIndex");
        defaultArgument = property.FindPropertyChild("defaultArgument");
        referanceType = property.FindPropertyChild("ReferanceType");
        genericTypeProperty = property.FindPropertyChild("___EDITOR_ONLY_VARIABLE___");

        if (property.isExpanded)
        {
            if (referanceType != null && referanceType.enumValueIndex != 0) return LINE_HEIGHT * 4 + LINE_SPACE * 3;
            return LINE_HEIGHT * 3 + LINE_SPACE * 2;
        }

        return LINE_HEIGHT;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect foldoutRect = new Rect(position.x, position.y, position.width, LINE_HEIGHT);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);
        if (!property.isExpanded)
        {
            LogThis("FuncDrawer.OnGUI(Not Expanded return)");
            return;
        }

        Rect ofRect = new Rect(position.x + 10f, position.y + LINE_HEIGHT + LINE_SPACE, position.width - 10f, LINE_HEIGHT);
        EditorGUI.BeginChangeCheck();
        EditorGUI.ObjectField(ofRect, targetObject);
        UpdateFuncOptions();
        // if (EditorGUI.EndChangeCheck() || funcOptions == null)
        // {
        // }

        if (currentIndex.intValue < 0)
        {
            currentIndex.intValue = Array.IndexOf(funcOptions, targetCompFunc.stringValue);
            LogThis($"FuncDrawer.OnGUI(Updated Current Index to {currentIndex.intValue})");
        }

        ofRect.y += LINE_HEIGHT + LINE_SPACE;
        EditorGUI.BeginChangeCheck();
        currentIndex.intValue = EditorGUI.Popup(ofRect, "Method", currentIndex.intValue, funcOptions);
        LogThis($"FuncDrawer.OnGUIPopup({currentIndex.intValue}, {funcOptions.Length})");
        if (EditorGUI.EndChangeCheck() || funcOptions == null) UpdateMethodName();
        if (currentIndex.intValue >= 0 && invokeWithArgOptions != null && invokeWithArgOptions.ContainsKey(currentIndex.intValue))
        {
            ofRect.y += LINE_HEIGHT + LINE_SPACE;
            ofRect.x += LINE_HEIGHT;
            ofRect.width -= LINE_HEIGHT;
            invokeWithArgOptions[currentIndex.intValue].Draw(ofRect, defaultArgument, referanceType);
        }
        else
        {
            referanceType.enumValueIndex = 0;
            LogThis($"FuncDrawer.OnGUI(referanceType set to {ArgumentReferanceType.Void})");
        }

        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }

    private void UpdateMethodName()
    {
        if (currentIndex.intValue > 0)
        {
            targetCompFunc.stringValue = funcOptions[currentIndex.intValue];
            LogThis($"FuncDrawer.UpdateMethodName(Updated to {targetCompFunc.stringValue})");
        }

        LogThis($"FuncDrawer.UpdateMethodName(Not Updated)");
    }

    private void UpdateFuncOptions()
    {
        if (targetObject.objectReferenceValue == null)
        {
            LogThis("FuncDrawer.UpdateFuncOptions(Null return)");
            funcOptions = new string[0];
            return;
        }

        List<string> supportedMethods = new List<string>();
        if (invokeWithArgOptions == null) invokeWithArgOptions = new Dictionary<int, IMethodArgInfo>();
        else invokeWithArgOptions.Clear();

        Type propertyTypeChecker = null;
        try
        {
            propertyTypeChecker = genericTypeProperty.GetProperType();
        }
        catch
        {
        }

        if (propertyTypeChecker == null || !propertyTypeChecker.IsSerializable)
        {
            Debug.LogError("Type not supported for UnityFunc must be Serializable");
            return;
        }


        Type exclude1 = typeof(System.Object);
        Type exclude2 = typeof(UnityEngine.Component);
        Type exclude3 = typeof(UnityEngine.Object);
        Type exclude4 = typeof(UnityEngine.MonoBehaviour);
        Type exclude5 = typeof(UnityEngine.Behaviour);

        foreach (Component component in ((GameObject) targetObject.objectReferenceValue).GetComponents<Component>())
        {
            foreach (Type type in ReflectionUtils.AllBaseTypes(component.GetType()))
            {
                if (type == exclude1 || type == exclude2 || type == exclude3 || type == exclude4 || type == exclude5) continue;
                foreach (MethodInfo methodInfo in type.GetMethods())
                {
                    if (propertyTypeChecker.IsAssignableFrom(methodInfo.ReturnType))
                    {
                        ParameterInfo[] parameterInfo = methodInfo.GetParameters();
                        int paramCount = parameterInfo.Length;
                        if (paramCount == 0)
                        {
                            supportedMethods.Add($"{type}/{methodInfo.Name} ()");
                        }
                        else if (paramCount == 1)
                        {
                            (string, IMethodArgInfo) methodArgInfo = GetMethodArgInfo(parameterInfo[0]);
                            if (methodArgInfo.Item2 != null)
                            {
                                supportedMethods.Add($"{type}/{methodInfo.Name} ({methodArgInfo.Item1})");
                                invokeWithArgOptions.Add(supportedMethods.Count - 1, methodArgInfo.Item2);
                                // methodArgInfo.Item2.SetDefault(defaultArgument);
                            }
                        }
                    }
                }
            }
        }

        funcOptions = supportedMethods.ToArray();
        // LogThis($"FuncDrawer.UpdateFuncOptions(Looking for {lookingFor}, found {funcOptions.Length}, {invokeWithArgOptions.Count})");
    }

    /*private static Func<Type, bool> GetPropertyTypeChecker(SerializedProperty property)
    {
        if (property == null) return null;
        switch (property.propertyType)
        {
            case (SerializedPropertyType.Integer): return type => typeof(int) == type;
            case (SerializedPropertyType.Boolean): return type => typeof(bool) == type;
            case (SerializedPropertyType.Float): return type => typeof(float) == type;
            case (SerializedPropertyType.String): return type => typeof(string) == type;
            case (SerializedPropertyType.Character): return type => typeof(char) == type;
            case (SerializedPropertyType.Color): return type => typeof(Color) == type;
            case (SerializedPropertyType.LayerMask): return type => typeof(LayerMask) == type;
            case (SerializedPropertyType.Vector2): return type => typeof(Vector2) == type;
            case (SerializedPropertyType.Vector3): return type => typeof(Vector3) == type;
            case (SerializedPropertyType.Vector4): return type => typeof(Vector4) == type;
            case (SerializedPropertyType.Rect): return type => typeof(Rect) == type;
            case (SerializedPropertyType.AnimationCurve): return type => typeof(AnimationCurve) == type;
            case (SerializedPropertyType.Bounds): return type => typeof(Bounds) == type;
            case (SerializedPropertyType.Gradient): return type => typeof(Gradient) == type;
            case (SerializedPropertyType.Quaternion): return type => typeof(Quaternion) == type;
            case (SerializedPropertyType.Vector2Int): return type => typeof(Vector2Int) == type;
            case (SerializedPropertyType.Vector3Int): return type => typeof(Vector3Int) == type;
            case (SerializedPropertyType.RectInt): return type => typeof(RectInt) == type;
            case (SerializedPropertyType.BoundsInt): return type => typeof(BoundsInt) == type;
            case (SerializedPropertyType.ObjectReference): return type => typeof(Object).IsAssignableFrom(type);
            default: return null;
        }
    }*/

    private (string, IMethodArgInfo) GetMethodArgInfo(ParameterInfo parameterInfo)
    {
        Type paraType = parameterInfo.ParameterType;
        string label = parameterInfo.Name;

        // @formatter:off
        if (paraType == typeof(int))                   return ("int",    new MethodArgsInfo<int>   (enumReferanceValue: 1, label: label, currentValue: defaultArgument.FindPropertyChild("intArgument").intValue,                drawerAction: EditorGUI.IntField,   valueSetterAction: ValueSetters.Set_int            ));
        if (paraType == typeof(float))                 return ("float",  new MethodArgsInfo<float> (enumReferanceValue: 2, label: label, currentValue: defaultArgument.FindPropertyChild("floatArgument").floatValue,            drawerAction: EditorGUI.FloatField, valueSetterAction: ValueSetters.Set_float          ));
        if (paraType == typeof(string))                return ("string", new MethodArgsInfo<string>(enumReferanceValue: 3, label: label, currentValue: defaultArgument.FindPropertyChild("stringArgument").stringValue,          drawerAction: EditorGUI.TextField,  valueSetterAction: ValueSetters.Set_string         ));
        if (paraType == typeof(bool))                  return ("bool",   new MethodArgsInfo<bool>  (enumReferanceValue: 4, label: label, currentValue: defaultArgument.FindPropertyChild("boolArgument").boolValue,              drawerAction: EditorGUI.Toggle,     valueSetterAction: ValueSetters.Set_bool           ));
        if (typeof(Object).IsAssignableFrom(paraType)) return ("Object", new MethodArgsInfo<Object>(enumReferanceValue: 5, label: label, currentValue: defaultArgument.FindPropertyChild("objectArgument").objectReferenceValue, drawerAction: ((rect, str, refer) => EditorGUI.ObjectField(rect, str, refer, paraType, true)), valueSetterAction: ValueSetters.Set_Object));
        // @formatter:on

        return ("Not Supported", null);
    }

    public static void LogThis(object obj)
    {
        if (Debugging)
        {
            Debug.Log(obj);
        }
    }
}
#endif