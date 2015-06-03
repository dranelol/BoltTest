using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ServerManager: MonoBehaviour
{
    private static ServerManager _instance;

    private List<CredentialToken> connectedUsers = new List<CredentialToken>();

    public List<CredentialToken> ConnectedUsers
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

        Messenger.AddListener<CredentialToken>("UserAddedServer", AddToLobby);
        
        Messenger.AddListener<CredentialToken>("UserRemovedServer", RemoveFromLobby);

        Messenger.MakePermanent("UserAddedServer");
        Messenger.MakePermanent("UserRemovedServer");
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

        //Messenger.Broadcast("UpdateLobby");
    }
    public void RemoveFromLobby(CredentialToken user)
    {
        if (connectedUsers.Contains(user) == true)
        {
            connectedUsers.Remove(user);
        }

        //Messenger.Broadcast("UpdateLobby");
    }

    public bool CheckUsedDisplayName(string name)
    {
        return true;
    }

    public CredentialToken GetConnectedTokenByIP(string IP)
    {
        CredentialToken token = new CredentialToken();

        token = connectedUsers.Find(x => x.IP == IP);

        return token;
    }

}
