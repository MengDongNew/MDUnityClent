using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MDCSharpClient;

public class SocketEngine : MonoBehaviour, MDCSharpClient.IPeerListener {
    static SocketEngine instance;
    public static SocketEngine Instance { get { return instance; } }
    private ClientPeer peer { get; set; }
    private Dictionary<byte, ControllerBase> controllers;

    //连接服务器事件
    public delegate void OnConntctToServerEvent();
    public event OnConntctToServerEvent OnConnectToServer;


    void Awake()
    {
        instance = this;
        controllers = new Dictionary<byte, ControllerBase>();
        peer = new ClientPeer(this);
        peer.Connect("127.0.0.1", "26680");
    }
    void OnDestroy() { instance = null; }
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (peer != null) peer.Service();
    }
    //注册
    public void RegistController(byte opCode, ControllerBase controller)
    {
        if (!controllers.ContainsKey(opCode))
        {
            controllers.Add(opCode, controller);
        }
        else
        {
            Log("Operation Code Repeat!! code=" + opCode);
        }
    }
    //注销
    public void UnRegistController(byte opCode)
    {
        if (controllers.ContainsKey(opCode))
        {
            controllers.Remove(opCode);
        }
    }

    //发起请求
    public void SendRequest(byte opCode, Dictionary<byte, object> parameters)
    {
        Log("sendrequest to server , opcode : " + opCode);
        peer.OpCustom((byte)opCode, parameters);
    }
    #region Interface
    public void DebugReturn(DebugLevel level, string message)
    {
        Log("DebugReturn:" + level + " " + message);
    }

    public void OnEvent(EventData eventData)
    {
        Log("OnEvent:" + eventData.Code);
        ControllerBase controller;
        if (controllers.TryGetValue(eventData.Code, out controller))
        {
            controller.OnEvent(eventData);
        }
        else
        {
            Log("Reveive a unkonwn eventData. EventCode ：" + eventData.Code);
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Log("OnOperationResponse:" + operationResponse.OperationCode);
        ControllerBase controller;
        if (controllers.TryGetValue(operationResponse.OperationCode, out controller))
        {
            controller.OnOperationResponse(operationResponse);
        }
        else
        {
            Log("Receive a unknown response . OperationCode :" + operationResponse.OperationCode);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Log("OnStatusChanged:" + statusCode);
        switch (statusCode)
        {
            case StatusCode.Connect:
                {
                    //isConnected = true;
                    if (OnConnectToServer != null)
                        OnConnectToServer();
                }
                break;
            case StatusCode.Disconnect:
                {
                    //todo
                }
                break;
            default:
                //isConnected = false;
                break;
        }
    }

    #endregion

    void Log(string s)
    {
        Debug.Log("SocketEngine:" + s);
    }
}
