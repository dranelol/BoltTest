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
        Messenger.Broadcast("UserAddedToLobby", token);

    }

    public override void Disconnected(BoltConnection connection, IProtocolToken token)
    {
        var log = LogEvent.Create();
        log.Message = string.Format("{0} disconnected", connection.RemoteEndPoint);
        log.Send();

        CredentialToken currentToken = ClientManager.Instance.GetToken();

        Debug.Log("token disconnected: " + currentToken.LoginName);

        Messenger.Broadcast("UserRemovedFromLobby", currentToken);

        // if added to server's lobby, remove from lobby
    }
}
