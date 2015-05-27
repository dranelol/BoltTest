using UnityEngine;
using System.Collections;
using Bolt;

public class Menu : MonoBehaviour
{

    enum State
    {
        SelectPeer,
        ServerBrowser,
        SelectCredentials,
        LoggingIn,
        LoggedIn
    }

    State state = State.SelectPeer;

    private UdpKit.UdpSession selectedSession = null;

    private string inputUserName = "";
    private string inputPassword = "";

    private string inputDisplayName;

    

    void SelectPeer()
    {
        if (GUILayout.Button("Start Server", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            Instantiate(GameManager.Instance.ServerManagerGO);
            //BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse("127.0.0.1:27000"));
            BoltLauncher.StartServer(UdpKit.UdpEndPoint.Any);

            BoltNetwork.SetHostInfo("Here You Go", null);
            BoltNetwork.LoadScene("Tutorial1");
        }

        if (GUILayout.Button("Start Client", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            Instantiate(GameManager.Instance.ClientManagerGO);
            BoltLauncher.StartClient();
            state = State.ServerBrowser;
        }

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(10, (Screen.height / 2) + 10, Screen.width / 2, (Screen.height / 2) - 20), GUI.skin.box);
        GUILayout.Label(string.Format("Sessions: {0}", BoltNetwork.SessionList.Count));

        foreach (var kvp in BoltNetwork.SessionList)
        {
            GUILayout.Label(kvp.Value.HostName);
        }
    }

    void ServerBrowser()
    {
        selectedSession = null;

        GUILayout.Label("Server Browser");

        GUILayout.BeginVertical(GUI.skin.box);

        foreach (var session in BoltNetwork.SessionList)
        {
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
        }

        GUILayout.EndVertical();
    }

    void SelectCredentials()
    {
        GUILayout.Label("Log in: " + selectedSession.HostName + " | " + selectedSession.WanEndPoint.Address);

        GUILayout.BeginVertical(GUI.skin.box);

        inputUserName = GUILayout.TextField(inputUserName, 20);
        inputPassword = GUILayout.TextField(inputPassword, 20);

        if (GUILayout.Button("Log in"))
        {
            Debug.Log(inputUserName);
            Debug.Log(inputPassword);

            StartCoroutine(ClientManager.Instance.SetPublicIP());

            state = State.LoggingIn;
        }
 
        GUILayout.EndVertical();
    }

    void LoggingIn()
    {
        GUILayout.BeginVertical(GUI.skin.box);

        if (ClientManager.Instance.SettingIP == true)
        {
            GUILayout.Label("Logging in...");
        }

        else
        {
            // build a credentials token to connect to the server with
            state = State.LoggedIn;

            CredentialToken token = new CredentialToken();
            token.AuthLevel = 0;
            token.DisplayName = "";
            token.LoginName = inputUserName;
            token.Password = inputPassword;
            token.IP = ClientManager.Instance.loginIP;

            BoltNetwork.Connect(selectedSession, token);
        }

        GUILayout.EndVertical();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, (Screen.height / 2) - 20));

        switch (state)
        {
            case State.SelectPeer: SelectPeer(); break;
            case State.ServerBrowser: ServerBrowser(); break;
            case State.SelectCredentials: SelectCredentials(); break;
            case State.LoggingIn: LoggingIn(); break;
            
        }

        GUILayout.EndArea();
    }
}
