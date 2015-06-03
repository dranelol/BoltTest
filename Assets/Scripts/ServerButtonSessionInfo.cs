using UnityEngine;
using System.Collections;

public class ServerButtonSessionInfo : MonoBehaviour 
{
    public UdpKit.UdpSession SelectedSession;

    public void Connect()
    {
        // get credential token from clientmanager
        CredentialToken token = ClientManager.Instance.GetToken();
        Debug.Log("connecting with token: ");
        Debug.Log(token.LoginName);
        Debug.Log(token.DisplayName);
        Debug.Log(token.Password);
        Debug.Log(token.IP);
        Debug.Log(token.AuthLevel);
        // connect to selected server

        // connect client here??????




        BoltNetwork.Connect(SelectedSession, token);

        StartMenuGUIManager.Instance.SetServerLobbyTitle("Server Lobby: " + SelectedSession.HostName);
        StartMenuGUIManager.Instance.Show("ServerLobby");
    }
}
