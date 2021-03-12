using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SmoothTurning : MonoBehaviour
{

    private InputDevice device;
    private Vector2 inputStick;

    // Start is called before the first frame update
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand); //오른쪽 조이스틱은 회전(왼쪽은 이동)
    }

    // Update is called once per frame
    void Update()
    {
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputStick);

        if(inputStick.x > 0.2f || inputStick.x < -0.2f) //조이스틱 조금 건들인것 말고 확실히 움직였을때만
        {
            Turn();    
        }
    }

    private void Turn()
    {
        var rotationAmount = transform.eulerAngles.y + inputStick.x; //y축 기준 회전
        var directionVector = new Vector3(transform.eulerAngles.x, rotationAmount, transform.eulerAngles.z); //회전한 위치값
        transform.rotation = Quaternion.Euler(directionVector); //회전한 위치값으로 rotate
    }
}
