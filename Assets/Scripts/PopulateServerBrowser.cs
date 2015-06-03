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

            server.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            Vector3 position = server.GetComponent<RectTransform>().anchoredPosition3D;

            position.z = 0.0f;

            server.GetComponent<RectTransform>().anchoredPosition3D = position;
           

            server.transform.GetChild(0).GetComponent<Text>().text = session.Value.HostName + " " + session.Value.WanEndPoint.Address;

            Debug.Log("adding server to list: " + session.Value.HostName + " " + session.Value.WanEndPoint);

            ServerButtonSessionInfo sessionInfo = server.transform.GetChild(1).GetComponent<ServerButtonSessionInfo>();

            sessionInfo.SelectedSession = session.Value;

            server.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(sessionInfo.Connect);
        }
    }

    
}
