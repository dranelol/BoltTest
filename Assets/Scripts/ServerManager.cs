using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager: MonoBehaviour
{
    private static ServerManager _instance;

    private HashSet<CredentialToken> connectedUsers = new HashSet<CredentialToken>();

    public HashSet<CredentialToken> ConnectedUsers
    {
        get
        {
            return connectedUsers;
        }
    }

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

        Messenger.AddListener<CredentialToken>("UserAddedToLobby", AddToLobby);
        Messenger.AddListener<CredentialToken>("UserRemovedFromLobby", AddToLobby);
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

    public void AddToLobby(CredentialToken user)
    {
        if(connectedUsers.Contains(user) == false)
        {
            connectedUsers.Add(user);
        }
    }
    public void RemoveFromLobby(CredentialToken user)
    {
        if (connectedUsers.Contains(user) == true)
        {
            connectedUsers.Remove(user);
        }
    }

}
