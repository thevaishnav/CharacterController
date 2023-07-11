using System;
using UnityEngine;
using Random = UnityEngine.Random;



[ExecuteAlways]
public class ArrangeChildren : MonoBehaviour, ISerializationCallbackReceiver
{
    public enum ArrangeMode
    {
        Position,
        Rotation,
        Scale
    }

    [SerializeField] private ArrangeMode actsOn = ArrangeMode.Position;
    [SerializeField] private bool arrangeEnabledOnly;
    [SerializeField] private ArrangeOnAxis X, Y, Z;

    private Action updateAction;

    public ArrangeMode ActsOn => actsOn;
    public bool ArrangeEnabledOnly => arrangeEnabledOnly;
    
    private void Start()
    {
        SetUpdateAction();
        Debug.Log($"Update Action: {updateAction}");
    }

    private void Update()
    {
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
            if (arrangeEnabledOnly && !child.gameObject.activeSelf) continue;
            current = child.localPosition;
            current.x = X.GetNextValue(counter, cc, current.x);
            current.y = Y.GetNextValue(counter, cc, current.y);
            current.z = Z.GetNextValue(counter, cc, current.z);
            child.localPosition = current;
            counter++;
        }
    }

    private void ActRotation()
    {
        int cc = transform.childCount;
        int counter = 0;
        Vector3 current;
        foreach (Transform child in transform)
        {
            if (arrangeEnabledOnly && !child.gameObject.activeSelf) continue;
            
            current = child.localRotation.eulerAngles;
            current.x = X.GetNextValue(counter, cc, current.x);
            current.y = Y.GetNextValue(counter, cc, current.y);
            current.z = Z.GetNextValue(counter, cc, current.z);
            child.localRotation = Quaternion.Euler(current);
            counter++;
        }
    }

    private void ActScale()
    {
        int cc = transform.childCount;
        int counter = 0;
        Vector3 current;
        foreach (Transform child in transform)
        {
            if (arrangeEnabledOnly && !child.gameObject.activeSelf) continue;
            
            current = child.localScale;
            current.x = X.GetNextValue(counter, cc, current.x);
            current.y = Y.GetNextValue(counter, cc, current.y);
            current.z = Z.GetNextValue(counter, cc, current.z);
            child.localScale = current;
            counter++;
        }
    }
    
    public void OnBeforeSerialize()
    {
    }
    public void OnAfterDeserialize()
    {
        SetUpdateAction();
    }
}