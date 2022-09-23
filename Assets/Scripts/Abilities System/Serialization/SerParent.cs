using System;
using UnityEngine;using UnityEngine.Rendering;

[Serializable]
public class SerParent 
{
    [SerializeField] private int parInt = 25;
    [SerializeField] private float parFloat = 25f;
    [SerializeField] private string parString = "parString";
    [SerializeField] private string[] parStringArray = new string[]{"S1", "S2", "S3"};


    public void Serialize()
    {
        Debug.Log(GetType());
        Debug.Log(JsonUtility.ToJson(this));
    }
    
}


public class SerChild : SerParent
{
    [SerializeField] private int chiInt = 2;
    [SerializeField] private float chiFloat = 5f;
    [SerializeField] private string chiString = "ChiString";
    [SerializeField] private string[] chiStringArray = new string[]{"c1", "c2", "c3"};
}