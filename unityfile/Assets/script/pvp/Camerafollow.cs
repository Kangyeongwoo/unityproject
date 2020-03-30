using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    //메인카메라 변수
    public Camera maincamera;

    //플레이어 변
    public Transform player;       // Reference to the player's transform.

    private void Start()
    {
        //플레이어의 transform 정보
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }


    public void Update()
    {
        //카메라가 플레이어를 쫓아서 위아래로 움직이게 한다
        //플레이어의 상하 이동 시 카메라 보다 위쪽으로 올라가면 카메라도 위쪽으로 이동
        if (player.position.z > maincamera.transform.position.z)
        {


            if (maincamera.transform.position.z < 5)
            {
                //좌표가 5 이하이면 카메라의 z 좌표가 플레이어 z 좌표가 된다
                maincamera.transform.position = new Vector3(maincamera.transform.position.x, maincamera.transform.position.y, player.position.z);
            }
            else
            {
                //카메라가 화면을 너무 벗어나지 않게 제한을 준다 캐릭터를 쫓아가도 좌표가 5 이상으로 올라가진 않는다
            }

        }
        else if (player.position.z < maincamera.transform.position.z)
        {//플레이어가 카메라 보다 아래로 내려가면
            if (maincamera.transform.position.z > -9)
            {
                //좌표가 -9이상일 때만 카메라의 z 좌표가 플레이어 z 좌표가 된다
                maincamera.transform.position = new Vector3(maincamera.transform.position.x, maincamera.transform.position.y, player.position.z);
            }
            else
            {
                //카메라가 화면을 너무 벗어나지 않게 제한을 준다 캐릭터를 쫓아가도 좌표가 -9 이하로 내려가진 않는다
            }
        }

    }
}
