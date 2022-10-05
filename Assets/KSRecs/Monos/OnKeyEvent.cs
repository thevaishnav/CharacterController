using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KSRecs.Monos
{
    public class OnKeyEvent : MonoBehaviour
    {
        public enum CheckMode
        {
            KeyDown,
            KeyHold,
            KeyUp
        }

        [SerializeField] private CheckMode checkMode;
        [SerializeField] private KeyCode keycode;
        [SerializeField] private UnityEvent onPress;



        private Func<KeyCode, bool> checkFunc;
        private void Start()
        {
            if (checkMode == CheckMode.KeyDown) checkFunc = Input.GetKeyDown;
            else if (checkMode == CheckMode.KeyHold) checkFunc = Input.GetKey;
            if (checkMode == CheckMode.KeyUp) checkFunc = Input.GetKeyUp;
        }

        void Update()
        {
            if (checkFunc.Invoke(keycode))
            {
                onPress?.Invoke();
            }
        }
    }


    public abstract class KeyEventListener : MonoBehaviour
    {
        public virtual void OnKeyEvent() { }
        public virtual void OnKeyEvent(KeyCode code) { }
        public virtual void OnKeyEvent(KeyCode code, OnKeyEvent.CheckMode checkMode) { }
        public virtual void OnKeyEvent(OnKeyEvent.CheckMode checkMode) { }
    }
}