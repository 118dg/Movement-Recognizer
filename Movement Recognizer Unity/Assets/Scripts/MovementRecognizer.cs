using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using PDollarGestureRecognizer;
using System.IO;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    public Transform movementSource;

    public float newPositionThresholdDistance = 0.05f;
    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGestureName;

    private List<Gesture> trainingSet = new List<Gesture>();
    private bool isPressed = true; //에러 뜨길래 내가 변수 선언함 ㅋㅋ;
    private bool isMoving = false;

    private List<Vector3> positionsList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);

        //현재 데이터 경로에서 .xml로 끝나는 파일을 모두 읽어옴
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach(var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
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
        //Debug.Log("Start Movement");
        isMoving = true;

        positionsList.Clear(); //모션 하나 그리는거 끝났으니까 Clear
        positionsList.Add(movementSource.position); //시작 위치 저장

        if(debugCubePrefab) //debugCubePrefab가 존재하면
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity),3); //그 cube로 motion보여주기
            //cube가 계속 남아있는게 아니라 사라져야 하므로 Destroy. 3초 뒤에 사라지게 한다.
        //motion 안 보여주고 싶으면 그냥 debugCubePrefab에 아무것도 안 넣으면 된다. (null)
    }

    void EndMovement()
    {
        //Debug.Log("End Movement");
        isMoving = false;

        //Create The Gesture From The Position List
        Point[] pointArray = new Point[positionsList.Count]; //same size with positionsList

        for(int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]); //movement의 x,y좌표(point) 추출
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0); //stroke가 뭐지????

        }

        Gesture newGesture = new Gesture(pointArray); //pointArray배열 안에 있는 point들로 이루어진 gesture

        //Add a new gesture to training set
        if (creationMode) //새로 gesture을 만들고 싶은 경우
        {
            newGesture.Name = newGestureName; //public으로 입력해준 newGestureName의 값을 새 gesture의 이름으로 설정
            trainingSet.Add(newGesture); //Gesture List에 newGesture 넣기

            //생성한 gesture 파일로 저장
            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        //recognize
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray()); //gesture 분류
            //tells us which geture it is, but also the score of the recognition of this element

            Debug.Log(result.GestureClass + " " + result.Score); //결과 출력!
        }
    }

    void UpdateMovement()
    {
        //Debug.Log("Update Movement");
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
