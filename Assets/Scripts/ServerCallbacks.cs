using UnityEngine;
using System.Collections;
using Bolt;


[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class ServerCallbacks : GlobalEventListener
{
    public override void Connected(BoltConnection connection, IProtocolToken acceptToken, IProtocolToken connectToken)
    {
        var log = LogEvent.Create();
        //log.Message = string.Format("{0} connected", connection.RemoteEndPoint);
        

        // connected to server
        CredentialToken token = (CredentialToken)connectToken;

        log.Message += "connected: " + token.LoginName;
        log.Send();

        Debug.Log("token connected: " + token.LoginName);

        // if not added to server's lobby, add to lobby

        // this will broadcast server-side only
        Messenger.Broadcast("UserAddedServer", token);

        // tell all clients that a new user is joining the lobby so that they may update their GUIs and such

        var userJoined = UserJoinedLobby.Create();

        userJoined.UserDisplayName = token.DisplayName;

        userJoined.Send();

    }

    public override void Disconnected(BoltConnection connection, IProtocolToken token)
    {
        var log = LogEvent.Create();
        log.Message = string.Format("{0} disconnected", connection.RemoteEndPoint.Address.ToString());
        log.Send();

        Debug.Log("token disconnected: " + connection.RemoteEndPoint.Address.ToString());

        // get token connected with connection's ip


        CredentialToken disconnectToken = ServerManager.Instance.GetConnectedTokenByIP(connection.RemoteEndPoint.Address.ToString());

        // this will broadcast server-side only
        Messenger.Broadcast("UserRemovedServer", disconnectToken);

        // tell all clients that a user is leaving the lobby so that they may update their GUIs and such

        var userDisconnected = UserDisconnectedLobby.Create();

        userDisconnected.UserDisplayName = disconnectToken.DisplayName;
        userDisconnected.Send();
    }
}
