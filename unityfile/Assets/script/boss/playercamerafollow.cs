using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercamerafollow : MonoBehaviour
{

    //메인카메라 변수
    public Camera maincamera;

    //플레이어 변
    public Transform player;       // Reference to the player's transform.


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camerafollowvec = new Vector3(player.position.x, maincamera.transform.position.y, player.position.z);


        maincamera.transform.position = camerafollowvec;
    }
}
