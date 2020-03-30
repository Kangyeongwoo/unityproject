using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raidmonsterbulletcolider : MonoBehaviour
{
    public GameObject bullet;

    void OnTriggerEnter(Collider other)
    
    {

        if (other.gameObject.tag == "wall" || other.gameObject.tag == "Player" || other.gameObject.tag == "remoteplayer")
        {
            Destroy(bullet);
        }



    }



}
