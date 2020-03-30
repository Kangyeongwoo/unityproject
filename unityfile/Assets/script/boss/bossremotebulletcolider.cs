using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossremotebulletcolider : MonoBehaviour
{

    public GameObject bullet;
    public int colcount = 0;

    //총알 오브젝트가 colider 끼리 부딪혔을 때 is trigger로 통과가 가능하다
    void OnTriggerEnter(Collider other)
    // void OnCollisionEnter(Collision other)
    {
        //other는 부딪힌 오브젝트 벽 또는 적에 맞으면 총알이 사라진다
        if (other.gameObject.tag == "wall")
        {
            if (currentability.abilitylist.Contains(8))
            {
                if (colcount < 3)
                {

                    Debug.Log("colidername1:" + other.gameObject.tag);
                    Quaternion test = Quaternion.Euler(bullet.GetComponent<Transform>().eulerAngles.x, bullet.GetComponent<Transform>().eulerAngles.y + 90, bullet.GetComponent<Transform>().eulerAngles.z);
                    bullet.GetComponent<Transform>().rotation = test;
                    colcount += 1;
                }
                else
                {
                    Destroy(bullet);
                }
            }
            else
            {

                Destroy(bullet);

            }
            //복제했던 총알이 사라짐


        }
        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "enemybody")
        {
            Debug.Log("colidername1:" + other.gameObject.tag);
            if (currentability.abilitylist.Contains(9))
            {

            }
            else
            {
                Destroy(bullet);
            }
            //복제했던 총알이 사라짐


        }
    }

}
