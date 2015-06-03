﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void BoltStarted()
    {
        BoltNetwork.RegisterTokenClass<CredentialToken>();
        BoltNetwork.RegisterTokenClass<PlayerCustomizationToken>();
    }

    public override void ZeusConnected(UdpKit.UdpEndPoint endpoint)
    {
        Bolt.Zeus.RequestSessionList();
    }


    public override void SceneLoadLocalDone(string map)
    {
        // randomize a position
        var pos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

        // instantiate cube
        BoltNetwork.Instantiate(BoltPrefabs.Robot, pos, Quaternion.identity);
    }

    List<string> logMessages = new List<string>();

    public override void OnEvent(LogEvent evnt)
    {
        logMessages.Insert(0, evnt.Message);
    }

    public override void OnEvent(UserJoinedLobby evnt)
    {
        Messenger.Broadcast("UserAddedToLobby", evnt.UserDisplayName);
    }

    public override void OnEvent(UserDisconnectedLobby evnt)
    {
        Messenger.Broadcast("UserRemovedFromLobby", evnt.UserDisplayName);
    }

    

    void OnGUI()
    {
        // only display max the 5 latest log messages
        int maxMessages = Mathf.Min(5, logMessages.Count);

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 400, Screen.height - 100, 400, 100), GUI.skin.box);

        for (int i = 0; i < maxMessages; ++i)
        {
            GUILayout.Label(logMessages[i]);
        }

        GUILayout.EndArea();
    }

    
}
