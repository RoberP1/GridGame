using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static event Action Oncomplate;
    public static event Action OnUndoComplate;
    void Start()
    {
        //decirle al game manager que existo
    }

    void Update()
    {
        
    }

    public void complate()
    {
        Oncomplate?.Invoke();
    }
    public void UndoComplate()
    {
        OnUndoComplate?.Invoke();
    }
}
