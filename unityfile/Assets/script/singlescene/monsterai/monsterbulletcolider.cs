using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterbulletcolider : MonoBehaviour
{
    public GameObject bullet;

    //총알 오브젝트가 colider 끼리 부딪혔을 때 is trigger로 통과가 가능하다
    void OnTriggerEnter(Collider other)
    // void OnCollisionEnter(Collision other)
    {
        //other는 부딪힌 오브젝트 벽 또는 적에 맞으면 총알이 사라진다
        if (other.gameObject.tag == "wall" || other.gameObject.tag == "Player")
        {
            Debug.Log("colidername1:" + other.gameObject.tag);

            //복제했던 총알이 사라짐
            Destroy(bullet);

        }

    }


}
