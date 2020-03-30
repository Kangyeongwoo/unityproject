using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillacept : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (currentability.abilitylist.Contains(7)) {

           double plusatk = GameObject.FindWithTag("Player").GetComponent<singleplayerinfo>().playerpower * 1.2;

            GameObject.FindWithTag("Player").GetComponent<singleplayerinfo>().playerpower = (int) plusatk;

        }
    }

}
