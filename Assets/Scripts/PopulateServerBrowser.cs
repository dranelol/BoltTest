using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Bolt;

public class PopulateServerBrowser : MonoBehaviour 
{
    [SerializeField]
    private GameObject ServerList;

    [SerializeField]
    private GameObject ServerPrefab;

    public void Populate()
    {
        foreach (Transform child in ServerList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var session in BoltNetwork.SessionList)
        {
            /*
            GUILayout.BeginHorizontal();

            GUILayout.Label(session.Value.HostName + " " + session.Value.WanEndPoint);

            if (GUILayout.Button("Join"))
            {
                // transition to "log in" gui state

                selectedSession = session.Value;
                state = State.SelectCredentials;

                // build a credentials token to connect to the server with
                //BoltNetwork.Connect(session.Value, token);


                //BoltNetwork.Connect(session.Value);
            }

            GUILayout.EndHorizontal();
             */

            GameObject server = (GameObject)Instantiate(ServerPrefab, Vector3.zero, Quaternion.identity);

            server.transform.parent = ServerList.transform;

            server.transform.GetChild(0).GetComponent<Text>().text = session.Value.HostName + " " + session.Value.WanEndPoint;

            Debug.Log("adding server to list: " + session.Value.HostName + " " + session.Value.WanEndPoint);

            server.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Connect);

            server.transform.GetChild(1).GetComponent<ServerButtonSessionInfo>().SelectedSession = session.Value;
        }
    }

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
        

        UdpKit.UdpSession selectedSession = GetComponent<ServerButtonSessionInfo>().SelectedSession;
        

        BoltNetwork.Connect(selectedSession, token);

        StartMenuGUIManager.Instance.SetServerLobbyTitle("Server Lobby: " + selectedSession.HostName);
        StartMenuGUIManager.Instance.Show("ServerLobby");
    }
}
