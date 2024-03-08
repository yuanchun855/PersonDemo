using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager<T> where T:new()
{
    private static T instance;
    public static T Instance {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }

    public virtual void OnAddListener(){}
    public virtual void OnRemoveListener(){}
    
    public virtual void OnDispose(){}
}
