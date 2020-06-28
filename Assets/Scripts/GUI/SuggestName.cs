using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SuggestName : GUIWindow {

    public Text changeNameText;

    public void Awake()
    {
        if(PlayerBoxController.currentPlayer.playerSuggestedNameAlready)
        {
            GUIManagement.LoadGUISetupSingular("CantSubmitAnotherName");
        }
    }

	public void Suggest(InputField suggestion)
    {
        bool trueE = SendString("parchment.play001.net", 25563, SystemInfo.deviceUniqueIdentifier + ";" + suggestion.text);
        if (trueE) { GUIManagement.LoadGUISetupSingular("NameSubmitted"); PlayerBoxController.currentPlayer.playerSuggestedNameAlready = true; }
        else { GUIManagement.LoadGUISetupSingular("NameSubmissionFailure"); }
    }

    public bool SendString(string ip, int port, string toSend)
    {
        try
        {
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(ip).AddressList[0], port);
            soc.Connect(remoteEP);
            soc.Send(Encoding.ASCII.GetBytes(toSend));
            soc.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
