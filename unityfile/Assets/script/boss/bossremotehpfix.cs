using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossremotehpfix : MonoBehaviour
{

    //체력바 전체
    public GameObject hpcanvas;

    //플레이어 또는 monster
    public GameObject hpbaroner;
    public GameObject bossplayer;

    // Start is called before the first frame update
    void Start()
    {
       // bossplayer = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        float x = hpbaroner.transform.position.x;
        float y = hpbaroner.transform.position.y;
        float z = hpbaroner.transform.position.z;


      //  var playerhpbarpos = bossplayer.GetComponent<bossplayerinfo>();

        /*
          if (playerhpbarpos.playerindex == 0)
         {
             hpcanvas.transform.position = new Vector3(x, y + 2.2f, z - 1.5f);
         }
         else
         {
             hpcanvas.transform.position = new Vector3(x, y + 2.2f, z + 1.5f);
         }
         */
        hpcanvas.transform.position = new Vector3(x, y + 2.2f, z + 1.5f);
    }
}
