using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private Transform theDoor; //location of the door

    private float startX;
    private bool isOpen; // open or close

    // Start is called before the first frame update
    private void Start()
    {
        startX = theDoor.position.x; //시작할 때 문 x값
    }

    // Update is called once per frame
    private void Update()
    {
        //참고로 미닫이문임. 슬라이드식.
        if (isOpen && theDoor.position.x < startX + 1f) //문 열건데 문 x값이 (시작 x값 + 1.0)보다 작으면
        {
            theDoor.position += theDoor.right * Time.deltaTime; //문 옆으로 밀어서 열기!
        }

        if (!isOpen && theDoor.position.x > startX)
        {
            theDoor.position -= theDoor.right * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) //파라미터 other : trigger 작동시킨 것
    {
        //만약 어떤 key에 의해서만 문이 열리게 하고 싶고(trigger object가 작동하도록 하고 싶고)
        //그 key를 right hand라고 한다면 XR rig/Camera offset/Righthand Controller/Cube에 Tag: key 추가하고
        //rigidbody 추가 후 isKinematic 체크. 단, Gravity는 끄기.
        if (other.gameObject.tag == "key")
        {
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "key")
        {
            isOpen = false;
        }
    }
}
