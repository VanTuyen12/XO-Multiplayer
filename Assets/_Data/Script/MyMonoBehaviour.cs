using System;
using UnityEngine;

public class MyMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void Start()
    {
        //For Override
    }
    protected virtual void Reset()
    {
        this.LoadComponents();
        this.ResetValue();
    }
    
    protected virtual void LoadComponents()
    {
        //For Override
    }
    
    protected virtual void ResetValue()
    {
        //For Override
    }

    public virtual void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }
}
