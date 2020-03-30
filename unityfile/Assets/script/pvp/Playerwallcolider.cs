using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerwallcolider : MonoBehaviour
{

    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }



    void OnCollisionEnter(Collision other)
    {   //플레이어가 enemy와 충돌 했을 때
        if (other.gameObject.tag == "wall")
        {
            Debug.Log("Collisionwall:"+ other.gameObject.tag);
            //player의 강체 관리
            Rigidbody rb = player.GetComponent<Rigidbody>();

            //물리엔진이 작동하지 않게 한다 (이러면 안넘어진다)
            rb.isKinematic = false;

        }
        Debug.Log("Collisionwall2:" + other.gameObject.tag);
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("Collisionwall3:" + other.gameObject.tag);
        Rigidbody rb = player.GetComponent<Rigidbody>();
        //강체의 물리엔진이 다시 작동 하도록 한다
        rb.isKinematic = true;
    }



}
