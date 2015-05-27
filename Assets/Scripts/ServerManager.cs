using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager: MonoBehaviour
{
    private static ServerManager _instance;

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

    public static ServerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ServerManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

}
