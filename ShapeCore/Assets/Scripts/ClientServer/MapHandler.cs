using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MapHandler : NetworkBehaviour
{
    int msg = 0;
    [SerializeField] TMP_Text Text;
    [SerializeField] GameObject Ui;
    private static Action<string> OnMessage;


    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Ui.SetActive(true);
        OnMessage += HandleNewMessage;
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!isOwned) { return; }
        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string msg)
    {
        Text.text = msg;
    }
    void Update()
    {
        SendMsg();
    }

    [Client]
    public void SendMsg()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOwned)
        {
            msg++;
            CmdSendMsg(msg.ToString());
        }
    }

    [Command]
    public void CmdSendMsg(string msg)
    {
        RpcHandleSendMsg(msg);
    }
    [ClientRpc]
    public void RpcHandleSendMsg(string msg)
    {
        OnMessage?.Invoke(msg);
    }
}
