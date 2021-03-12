using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Mover : MonoBehaviour
{
    private InputDevice _device;
    private CharacterController _character;
    private Vector2 _inputAxis; //where to move
    private GameObject _camera;

    // Start is called before the first frame update
    private void Start()
    {
        _device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand); //input source를 그냥 하드코딩으로 넣어줄 수도 있음
        _character = GetComponent<CharacterController>();
        _camera = GetComponent<XRRig>().cameraGameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        _device.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis);
        //check if left joystick is being pressed. if so, sign that info to _inputAxis variable 

        var inputVector = new Vector3(_inputAxis.x, Physics.gravity.y, _inputAxis.y); //direction that controller is being pushed in
        //Physics.gravity.y => apply gravity to character controller so character can actually fall now. no more floating.
        var inputDirection = transform.TransformDirection(inputVector); //transform inputVector to direction
        var lookDirection = new Vector3(0, _camera.transform.eulerAngles.y, 0); //카메라로 어느 방향 보고 있는지
        var newDirection = Quaternion.Euler(lookDirection) * inputDirection; //???????내적??????어떤 연산인지 아직 이해X

        //_character.Move(motion: new Vector3(_inputAxis.x, y: 0, z: _inputAxis.y) * Time.deltaTime * 1f);
        _character.Move(newDirection * Time.deltaTime * 1f);

        //왼쪽 조이스틱을 앞으로 향하게 고정해둔 상태에서 원래는 world 좌표계 기준으로(?) 움직였는데 이제는 고개 돌리면 고개 돌린 방향으로 나아감
    }
}
