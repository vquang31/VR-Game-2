using System;
using UnityEngine;

public class NewMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        LoadComponents();
    }
    protected virtual void LoadComponents()
    {

    }

    protected virtual void Reset()
    {
        LoadComponents();
        ResetValues();
    }

    protected virtual void ResetValues()
    {

    }

    protected virtual void Start()
    {

    }


}
