using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bolt;

public class PlayerManager : MonoBehaviour 
{
    private static PlayerManager _instance;

    //private Dictionary<string, Bolt.>

    private void Awake()
    {
        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public void test()
    {
        
    }
}
