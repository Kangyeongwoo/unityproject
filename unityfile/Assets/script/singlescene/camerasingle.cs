using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerasingle : MonoBehaviour
{
   //메인카메라 변수
    public Transform maincamera;

    //플레이어 변
    public Transform player;       // Reference to the player's transform.

    private void Start()
    {
        //플레이어의 transform 정보
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        maincamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }


    public void Update()
    {
       

        maincamera.position = new Vector3(maincamera.transform.position.x, maincamera.transform.position.y, player.position.z-10f);



    }
   
}
