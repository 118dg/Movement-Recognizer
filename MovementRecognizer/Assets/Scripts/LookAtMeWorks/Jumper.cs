using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Jumper : MonoBehaviour
{
    public XRNode inputSource; //type of XR node : headset, controller, and so on.
    public Transform theCube; //location of the cube

    [SerializeField]
    private TeleportationProvider _teleportationProvider; // component 

    [SerializeField]
    private bool isTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _teleportationProvider = GetComponent<TeleportationProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.triggerButton, out isTrigger); 
        // check if the trigger button of the controller is pressed. If pressed, isTrigger = true
        // out : put the result to the variable
        // 컨트롤러 버튼 누를 때마다 유니티 에디터에서 Jumper 컴포넌트의 IsTrigger 체크박스의 체크가 켜졌다 꺼졌다 함
        // 이떼 middle finger trigger는 다른 teleportation에서 쓴거고 여기선 index finger trigger 누르면 teleport

        if (isTrigger)
        {
            Jump();
        }
    }

    private void Jump()
    {
        var teleportRequest = new TeleportRequest();
        teleportRequest.destinationPosition = theCube.position; //location to teleport
        _teleportationProvider.QueueTeleportRequest(teleportRequest); //request teleportation to teleportation provider. FIFO.
    }
}
