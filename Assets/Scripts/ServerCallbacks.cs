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

        log.Message += "connected: " + token.LoginName + " " + token.Password + " " + token.IP;
        log.Send();

    }

    public override void Disconnected(BoltConnection connection, IProtocolToken token)
    {
        var log = LogEvent.Create();
        log.Message = string.Format("{0} disconnected", connection.RemoteEndPoint);
        log.Send();
    }
}
