using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using KSRecs.Utils;
using UnityEngine;

// public static Coroutine StartCoroutine(IEnumerator routine, Action onComplete = null) => new GameObject(nameof(routine) + " Coroutine").AddComponent<UtilsObject>().StartCustomCoroutine(routine, onComplete);

namespace KSRecs.Initialization
{
    public static class Initialization
    {
        public static GameObject Create(params M.IObjectInitParam[] paras)
        {
            Transform go = new GameObject().transform;
            foreach (M.IObjectInitParam para in paras)
            {
                para.Operate(go.transform);
            }
            return go.gameObject;
        }

        public static T Create<T>(params M.IObjectInitParam[] paras) where T : Component
            => Create(paras).AddComponent<T>();

        public static T Init<T, T0>(T0 arg0, params M.IObjectInitParam[] paras) where T : Component, IInit<T0>
        {
            T go = Create(paras).AddComponent<T>();
            go.Init(arg0);
            return go;
        }

        public static T Init<T, T0, T1>(T0 arg0, T1 arg1, params M.IObjectInitParam[] paras) where T : Component, IInit<T0, T1>
        {
            T go = Create(paras).AddComponent<T>();
            go.Init(arg0, arg1);
            return go;
        }

        public static T Init<T, T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2, params M.IObjectInitParam[] paras) where T : Component, IInit<T0, T1, T2>
        {
            T go = Create(paras).AddComponent<T>();
            go.Init(arg0, arg1, arg2);
            return go;
        }

        public static T Init<T, T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, params M.IObjectInitParam[] paras) where T : Component, IInit<T0, T1, T2, T3>
        {
            T go = Create(paras).AddComponent<T>();
            go.Init(arg0, arg1, arg2, arg3);
            return go;
        }

        public static T Init<T, T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, params M.IObjectInitParam[] paras) where T : Component, IInit<T0, T1, T2, T3, T4>
        {
            T go = Create(paras).AddComponent<T>();
            go.Init(arg0, arg1, arg2, arg3, arg4);
            return go;
        }

        public static T Start<T, T0>(T0 arg0, params M.IObjectInitParam[] paras) where T : MonoBehaviour, IStart<T0>
        {
            T go = Create(paras).AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0));
            return go;
        }

        public static T Start<T, T0, T1>(T0 arg0, T1 arg1, params M.IObjectInitParam[] paras) where T : MonoBehaviour, IStart<T0, T1>
        {
            T go = Create(paras).AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1));
            return go;
        }

        public static T Start<T, T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2, params M.IObjectInitParam[] paras) where T : MonoBehaviour, IStart<T0, T1, T2>
        {
            T go = Create(paras).AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2));
            return go;
        }

        public static T Start<T, T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, params M.IObjectInitParam[] paras) where T : MonoBehaviour, IStart<T0, T1, T2, T3>
        {
            T go = Create(paras).AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2, arg3));
            return go;
        }

        public static T Start<T, T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, params M.IObjectInitParam[] paras) where T : MonoBehaviour, IStart<T0, T1, T2, T3, T4>
        {
            T go = Create(paras).AddComponent<T>();
            go.StartCoroutine(go.OnStart(arg0, arg1, arg2, arg3, arg4));
            return go;
        }


        // @formatter:off
        public static GameObject CreateObject              (this Component _, params M.IObjectInitParam[] param)                                                                                                  => Create(param);
        public static T CreateObject<T>                    (this Component _, params M.IObjectInitParam[] param) where T : Component                                                                              => Create(param).AddComponent<T>();
        public static T InitObject<T, T0>                  (this Component _, T0 arg0,                                     params M.IObjectInitParam[] param) where T : Component, IInit<T0>                      => Init<T, T0>                 (arg0, param);
        public static T InitObject<T, T0, T1>              (this Component _, T0 arg0, T1 arg1,                            params M.IObjectInitParam[] param) where T : Component, IInit<T0, T1>                  => Init<T, T0, T1>             (arg0, arg1, param);
        public static T InitObject<T, T0, T1, T2>          (this Component _, T0 arg0, T1 arg1, T2 arg2,                   params M.IObjectInitParam[] param) where T : Component, IInit<T0, T1, T2>              => Init<T, T0, T1, T2>         (arg0, arg1, arg2, param);
        public static T InitObject<T, T0, T1, T2, T3>      (this Component _, T0 arg0, T1 arg1, T2 arg2, T3 arg3,          params M.IObjectInitParam[] param) where T : Component, IInit<T0, T1, T2, T3>          => Init<T, T0, T1, T2, T3>     (arg0, arg1, arg2, arg3, param);
        public static T InitObject<T, T0, T1, T2, T3, T4>  (this Component _, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, params M.IObjectInitParam[] param) where T : Component, IInit<T0, T1, T2, T3, T4>      => Init<T, T0, T1, T2, T3, T4> (arg0, arg1, arg2, arg3, arg4, param);
        public static T StartObject<T, T0>                 (this Component _, T0 arg0,                                     params M.IObjectInitParam[] param) where T : MonoBehaviour, IStart<T0>                 => Start<T, T0>                (arg0, param);
        public static T StartObject<T, T0, T1>             (this Component _, T0 arg0, T1 arg1,                            params M.IObjectInitParam[] param) where T : MonoBehaviour, IStart<T0, T1>             => Start<T, T0, T1>            (arg0, arg1, param);
        public static T StartObject<T, T0, T1, T2>         (this Component _, T0 arg0, T1 arg1, T2 arg2,                   params M.IObjectInitParam[] param) where T : MonoBehaviour, IStart<T0, T1, T2>         => Start<T, T0, T1, T2>        (arg0, arg1, arg2, param);
        public static T StartObject<T, T0, T1, T2, T3>     (this Component _, T0 arg0, T1 arg1, T2 arg2, T3 arg3,          params M.IObjectInitParam[] param) where T : MonoBehaviour, IStart<T0, T1, T2, T3>     => Start<T, T0, T1, T2, T3>    (arg0, arg1, arg2, arg3, param);
        public static T StartObject<T, T0, T1, T2, T3, T4> (this Component _, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, params M.IObjectInitParam[] param) where T : MonoBehaviour, IStart<T0, T1, T2, T3, T4> => Start<T, T0, T1, T2, T3, T4>(arg0, arg1, arg2, arg3, arg4, param);
        // @formatter:on
    }

    public static class M
    {
        public static IObjectInitParam Name(string value) => new MName(value);
        public static IObjectInitParam Parent(Transform value) => new MParent(value);
        public static IObjectInitParam Pos(Vector3 value) => new MPos(value);
        public static IObjectInitParam LocPos(Vector3 value) => new MLocPos(value);
        public static IObjectInitParam Rot(Quaternion value) => new MRot(value);
        public static IObjectInitParam Rot(Vector3 value) => new MRot(value);
        public static IObjectInitParam LocRot(Quaternion value) => new MLocRot(value);
        public static IObjectInitParam LocRot(Vector3 value) => new MLocRot(value);
        public static IObjectInitParam LocScale(Vector3 value) => new MLocScale(value);
        public static IObjectInitParam Components(Type[] value) => new MComponents(value);
        
        public interface IObjectInitParam
        {
            public void Operate(Transform transform);
        }

        public struct MName : IObjectInitParam
        {
            private string value;
            public MName(string value) => this.value = value;
            public void Operate(Transform transform) => transform.name = value;
        }

        public struct MParent : IObjectInitParam
        {
            private Transform value;
            public MParent(Transform value) => this.value = value;
            public void Operate(Transform transform) => transform.parent = value;
        }

        public struct MPos : IObjectInitParam
        {
            private Vector3 value;
            public MPos(Vector3 value) => this.value = value;
            public void Operate(Transform transform) => transform.position = value;
        }

        public struct MLocPos : IObjectInitParam
        {
            private Vector3 value;
            public MLocPos(Vector3 value) => this.value = value;
            public void Operate(Transform transform) => transform.localPosition = value;
        }

        public struct MRot : IObjectInitParam
        {
            private Quaternion value;
            public MRot(Vector3 value) => this.value = Quaternion.Euler(value);
            public MRot(Quaternion value) => this.value = value;
            public void Operate(Transform transform) => transform.rotation = value;
        }

        public struct MLocRot : IObjectInitParam
        {
            private Quaternion value;
            public MLocRot(Vector3 value) => this.value = Quaternion.Euler(value);
            public MLocRot(Quaternion value) => this.value = value;
            public void Operate(Transform transform) => transform.localRotation = value;
        }

        public struct MLocScale : IObjectInitParam
        {
            private Vector3 value;
            public MLocScale(Vector3 value) => this.value = value;
            public void Operate(Transform transform) => transform.localScale = value;
        }

        public struct MComponents : IObjectInitParam
        {
            private Type[] value;
            public MComponents(params Type[] value) => this.value = value;
            public void Operate(Transform transform)
            {
                GameObject go = transform.gameObject;
                foreach (Type behaviour in value)
                {
                    go.AddComponent(behaviour);
                }
            }
        }

    }


    public interface IInit<T0>
    {
        public void Init(T0 arg0);
    }

    public interface IInit<T0, T1>
    {
        public void Init(T0 arg0, T1 arg1);
    }

    public interface IInit<T0, T1, T2>
    {
        public void Init(T0 arg0, T1 arg1, T2 arg2);
    }

    public interface IInit<T0, T1, T2, T3>
    {
        public void Init(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    }

    public interface IInit<T0, T1, T2, T3, T4>
    {
        public void Init(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }


    public interface IStart<T0>
    {
        public IEnumerator OnStart(T0 arg0);
    }

    public interface IStart<T0, T1>
    {
        public IEnumerator OnStart(T0 arg0, T1 arg1);
    }

    public interface IStart<T0, T1, T2>
    {
        public IEnumerator OnStart(T0 arg0, T1 arg1, T2 arg2);
    }

    public interface IStart<T0, T1, T2, T3>
    {
        public IEnumerator OnStart(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    }

    public interface IStart<T0, T1, T2, T3, T4>
    {
        public IEnumerator OnStart(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}