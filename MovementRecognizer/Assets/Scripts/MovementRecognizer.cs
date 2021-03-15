using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    public Transform movementSource;

    public float newPositionThresholdDistance = 0.05f;
    public GameObject debugCubePrefab;

    private bool isPressed = true; //에러 뜨길래 내가 변수 선언함 ㅋㅋ;
    private bool isMoving = false;

    private List<Vector3> positionsList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out isPressed, inputThreshold);

        //Start The movement
        if(!isMoving && isPressed)
        {
            StartMovement();
        }

        //Ending The Movement
        else if(isMoving && !isPressed)
        {
            EndMovement();
        }

        //Updateing The Movement
        else if(isMoving && isPressed)
        {
            UpdateMovement();
        }
    }

    void StartMovement()
    {
        Debug.Log("Start Movement");
        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position); //시작 위치 저장

        if(debugCubePrefab) //debugCubePrefab가 존재하면
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity),3); //그 cube로 motion보여주기
            //cube가 계속 남아있는게 아니라 사라져야 하므로 Destroy. 3초 뒤에 사라지게 한다.
        //motion 안 보여주고 싶으면 그냥 debugCubePrefab에 아무것도 안 넣으면 된다. (null)
    }

    void EndMovement()
    {
        Debug.Log("End Movement");
        isMoving = false;
    }

    void UpdateMovement()
    {
        Debug.Log("Update Movement");
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        
        // 마지막 위치보다 거리가 0.05f(5cm)이상 벌어지면 그 위치를 리스트에 추가
        if(Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            positionsList.Add(movementSource.position);

            if (debugCubePrefab)
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
        }
            
    }
}
