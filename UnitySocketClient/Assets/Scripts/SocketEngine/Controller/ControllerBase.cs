using MDCSharpClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour {

	// Use this for initialization
	protected virtual void Start () {
        SocketEngine.Instance.RegistController(OpCode, this);
    }


    protected virtual void OnDestroy()
    {
        SocketEngine.Instance.UnRegistController(OpCode);
    }
    public virtual void OnEvent(EventData eventData)
    {

    }
    public abstract byte OpCode { get; }
    public abstract void OnOperationResponse(OperationResponse operationResponse);

}
