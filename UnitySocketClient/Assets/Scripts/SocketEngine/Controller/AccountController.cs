using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.Tool;
using Common.Modal;
public class AccountController : ControllerBase {

    protected override void Start()
    {
        base.Start();

        SocketEngine.Instance.OnConnectToServer += Instance_OnConnectToServer;
    }

    private void Instance_OnConnectToServer()
    {
        User user = new User() { Accesstoken = "1001", Password="1001"};
        Login(user);
    }
    public override byte OpCode
    {
        get { return (byte)OperationCode.Account; }
    }
 
    public override void OnOperationResponse(MDCSharpClient.OperationResponse operationResponse)
    {
        SubCode subcode = ParameterTool.GetSubcode(operationResponse.Parameters);
        Debug.Log("AccountController.OnOperationRespnse.subcode="+subcode);

        switch (subcode) { 
            case SubCode.AccLogin:{
                if (operationResponse.ReturnCode == (short)ReturnCode.Success) {
                        User user = ParameterTool.GetParameter<User>(operationResponse.Parameters, ParameterCode.User);
                    Debug.Log("登陆成功！User.accesstoken="+user.Accesstoken);
                }
               
            }break;
            case SubCode.AccRegister: {
                if (operationResponse.ReturnCode == (short)ReturnCode.Success)
                {
                    Debug.Log("注册成功！");
                }
            } break;
        }
    }

    public void Login(User user)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddSubcode(parameters, SubCode.AccLogin);
        ParameterTool.AddParameter<User>(parameters, ParameterCode.User, user);
        foreach (var v in parameters) {
            Debug.Log("key="+v.Key+",value="+v.Value);
        }
        SocketEngine.Instance.SendRequest(OpCode, parameters);
    }
    public void Register(User user)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddSubcode(parameters, SubCode.AccRegister);
        ParameterTool.AddParameter<User>(parameters, ParameterCode.User, user);
        SocketEngine.Instance.SendRequest(OpCode, parameters);
    }
}
