using System.Collections;
using System.Collections.Generic;
using MDCSharpClient;
using UnityEngine;
using Common;
public class ServerListController : ControllerBase {
    public override byte OpCode { get { return (byte)OperationCode.Server; } }
    protected override void Start()
    {
        base.Start();

        SocketEngine.Instance.OnConnectToServer += Instance_OnConnectToServer;
    }

    private void Instance_OnConnectToServer()
    {
        RequestServerList();
    }

    public override void OnEvent(EventData eventData)
    {
        Log("OnEvent: eventData.Code=" + eventData.Code);
        foreach (var parameter in eventData.Parameters)
        {
            Log(parameter.Key + ":" + parameter.Value.ToString());
        }
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        Log("OnOperationResponse=" + operationResponse.OperationCode + " ReturnCode=" + operationResponse.ReturnCode);
        foreach (var parameter in operationResponse.Parameters)
        {
            Log(parameter.Key + ":" + parameter.Value.ToString());
        }
    }



    public void RequestServerList()
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add(10, "Hello 我是客户端！");
        SocketEngine.Instance.SendRequest((byte)OperationCode.Server, parameters);
    }

    void Log(string s)
    {
        Debug.Log("ServerListController:" + s);
    }
}
