using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterhpbarfix : MonoBehaviour
{
    //체력바 전체
    public GameObject hpcanvas;

    //플레이어 또는 monster
    public GameObject hpbaroner;



    private void Update()
    {


        float x = hpbaroner.transform.position.x;
        float y = hpbaroner.transform.position.y;
        float z = hpbaroner.transform.position.z;


        //체력바 전체의 위치를 항상 케릭터 머리위에 고정 회전은 안되게 한
        //     var playerhpbarpos = hpbaroner.GetComponent<Playerinfo>();
        // 1p와 2p는 체력이 보이는 방향이 다르기 때문에 1p이면 위쪽에 2p이면 아래쪽에 배치
        //       if (playerhpbarpos.playerindex == 0)
        //      {
        hpcanvas.transform.position = new Vector3(x, y + 2.2f, z + 1.5f);
        //      }
        //      else
        //      {
        //         hpcanvas.transform.position = new Vector3(x, y + 2.2f, z - 1.5f);
        //     }

    }
}
