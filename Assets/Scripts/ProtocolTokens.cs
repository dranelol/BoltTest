using UnityEngine;
using System.Collections;
using Bolt;
using UdpKit;

public class CredentialToken : IProtocolToken
{
    public string LoginName;
    public string DisplayName;
    public string Password;
    public string IP;
    public int AuthLevel;

    public void Write(UdpPacket packet)
    {
        packet.WriteString(LoginName);
        packet.WriteString(DisplayName);
        packet.WriteString(Password);
        packet.WriteString(IP);
        packet.WriteInt(AuthLevel);
    }

    public void Read(UdpPacket packet)
    {
        LoginName = packet.ReadString();
        DisplayName = packet.ReadString();
        Password = packet.ReadString();
        IP = packet.ReadString();
        AuthLevel = packet.ReadInt();
    }
}

public class PlayerCustomizationToken : IProtocolToken
{
    public Color MatColor;

    public void Write(UdpPacket packet)
    {
        packet.WriteColorRGBA(MatColor);
    }

    public void Read(UdpPacket packet)
    {
        MatColor = packet.ReadColorRGBA();
    }
}
