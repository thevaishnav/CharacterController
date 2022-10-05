using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Vector3 value;
    [SerializeField] private ForceMode forceMode;
    
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(value, forceMode);       
    }
}
