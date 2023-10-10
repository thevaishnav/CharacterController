using UnityEngine;
using System;

namespace KS.CharaCon.Utils
{
    /// <summary> Property attribute that only draws the property in inspector if a certain condition is met.  </summary>
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

        /// <summary> Draw this property in inspector only if the given enum property's value is equal to required value </summary>
        /// <param name="enumName"> Name of (enum) property </param>
        /// <param name="comparedValue"> Required value for that property </param>
        /// <example>
        /// The following code will only display startKeyCode property in inspector
        /// if startType == AbilityStartType.KeyDown (index of KeyDown = 2)
        /// <code>
        /// public enum AbilityStartType { Automatic, Manual, KeyDown }
        /// public class Ability : MonoBehaviour
        /// {
        ///     [SerializeField] AbilityStartType startType;
        ///     [SerializeField, DrawIfEnumEqual("startType", 2)] private KeyCode startKeyCode;
        /// }
        /// </code>
        /// </example>
        public DrawIfEnumEqualAttribute(string enumName, int comparedValue)
        {
            this.enumName = enumName;
            this.comparedValue = comparedValue;
            this.CompairTo = (i => i == comparedValue);
        }


        /// <summary> Draw this property in inspector only if the given enum property's satisfied given criteria </summary>
        /// <param name="enumName"> Name of (enum) property </param>
        /// <param name="comparedValue"> Required value for that property </param>
        /// <param name="comparisonType"> What type of comparison should be done </param>
        /// <example>
        /// The following code will only display
        /// startKeyCode property if startType == AbilityStartType.KeyDown (index of KeyDown = 2)
        /// And
        /// endKeyCode property if startType != AbilityStartType.Manual (index of Manual = 1)
        /// <code>
        /// public enum AbilityStartType { Automatic, Manual, KeyDown }
        /// public class Ability : MonoBehaviour
        /// {
        ///     [SerializeField] AbilityStartType startType;
        ///     [SerializeField, DrawIfEnumEqual("startType", 2, ComparisonType.Equal)] private KeyCode startKeyCode;
        ///     [SerializeField, DrawIfEnumEqual("startType", 1, ComparisonType.NotEqual)] private KeyCode endKeyCode;
        /// }
        /// </code>
        /// </example>
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
}