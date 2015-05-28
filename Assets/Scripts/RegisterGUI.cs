﻿using UnityEngine;
using System.Collections;

public class RegisterGUI : MonoBehaviour
{
    [SerializeField]
    private GUIManager manager;

    //add a registergui script to any element that wants to register itself with another gameobject
    void Awake()
    {
        try
        {
            manager.Register(gameObject);
        }
        catch
        {
            Debug.LogError("Could not register " + gameObject.name + " with ");
        }
    }

    

}