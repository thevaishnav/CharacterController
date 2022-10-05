using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KSRecs.Utils
{
    public static class UnityObjectUtils
    {
        private static bool Requires(Type obj, Type requirement)
        {
            return Attribute.IsDefined(obj, typeof(RequireComponent)) &&
                   Attribute.GetCustomAttributes(obj, typeof(RequireComponent)).OfType<RequireComponent>()
                       .Any(rc => rc.m_Type0.IsAssignableFrom(requirement));
        }

        public static bool CanDestroy(GameObject targetGo, Type componentToDestroy)
        {
            if (!componentToDestroy.IsAssignableFrom(typeof(Component))) return true;
            return !targetGo.GetComponents<Component>().Any(c => Requires(c.GetType(), componentToDestroy));
        }

        public static string ClassName(Component component)
        {
            string fullName = component.GetType().ToString();
            int index = fullName.LastIndexOf(".");
            if (index > 0)
            {
                return $"{fullName.Substring(index + 1)} ({fullName.Substring(0, index)})";
            }

            return $"{fullName}";
        }

        public static string CopyComponentTo(Component sourceComp, GameObject targetObject, bool copyNonSerializedFieldsToo)
        {
            if (sourceComp is Transform sourceTran)
            {
                Transform tar = targetObject.transform;
                tar.position = sourceTran.position;
                tar.rotation = sourceTran.rotation;
                tar.localScale = sourceTran.localScale;
                return "Transform: Position, Rotation, Scale";
            }

            StringBuilder info = new StringBuilder();
            Type comType = sourceComp.GetType();
            Component targetComp = targetObject.GetComponent(comType);
            if (targetComp == null) targetComp = targetObject.AddComponent(comType);
            FieldInfo[] targetProps = comType.GetFields(BindingFlags.Default | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            info.Append($"{comType}: ");
            foreach (FieldInfo fieldInfo in targetProps)
            {
                if (!copyNonSerializedFieldsToo)
                {
                    if (!(fieldInfo.IsPublic || fieldInfo.GetCustomAttribute<SerializeField>() != null)
                        || (fieldInfo.GetCustomAttribute<NonSerializedAttribute>() != null))
                    {
                        continue;
                    }
                }

                if (fieldInfo.FieldType.IsAssignableFrom(typeof(Component)))
                {
                    Component sourceValue = (Component) fieldInfo.GetValue(sourceComp);
                    if (sourceValue.transform.IsChildOf(sourceComp.transform))
                    {
                        Component targetValue = targetComp.GetComponentInChildren(sourceValue.GetType());
                        if (targetValue != null)
                        {
                            fieldInfo.SetValue(targetComp, targetValue);
                            info.Append($"{fieldInfo.Name}, ");
                            continue;
                        }
                    }
                }

                fieldInfo.SetValue(targetComp, fieldInfo.GetValue(sourceComp));
                info.Append($"{fieldInfo.Name}, ");
            }

            return info.ToString();
        }
    }
}