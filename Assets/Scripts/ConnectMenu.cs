using UnityEngine;
using System.Collections;
using UdpKit;
using Bolt;

public class ConnectMenu : MonoBehaviour 
{
    void OnGUI()
    {

        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));

        if (GUILayout.Button("Start Server", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            // START SERVER

            

           // SessionData sessionData = new SessionData(description, BoltNetwork.maxConnections);

           // BoltNetwork.SetHostInfo(serverName, sessionData);

            //IProtocolToken token = 

            //BoltNetwork.SetHostInfo("TestServer", token);

            

            BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse("127.0.0.1:27000"));

            BoltNetwork.SetHostInfo("Hello, World", null);

            Zeus.RequestSessionList();

            BoltNetwork.LoadScene("main");
        }

        if (GUILayout.Button("Start Client", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            // START CLIENT
            BoltLauncher.StartClient();

            var sessions = BoltNetwork.SessionList;

            foreach (var sessionPair in sessions)
            {
                UdpKit.UdpSession session = sessionPair.Value;

                Debug.Log("connecting to: " + session.HostName.ToString());

                BoltNetwork.Connect(session);

                break;
            }

            //BoltNetwork.Connect(UdpKit.UdpEndPoint.Parse("127.0.0.1:27000"));
        }

        GUILayout.EndArea();

        
    }
}
