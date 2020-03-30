using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hpbarfix_enemy : MonoBehaviour
{
    //체력바 전체
    public GameObject hpcanvas;

    //플레이어 또는 monster
    public GameObject hpbaroner;

    private void Update()
    {
        // hpcanvas.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        // hpcanvas.transform.forward = fixobj.transform.forward;
        // hpbar.transform.position = hpcanvas.transform.position;

        float x = hpbaroner.transform.position.x;
        float y = hpbaroner.transform.position.y;
        float z = hpbaroner.transform.position.z;


        //체력바 전체의 위치를 항상 케릭터 머리위에 고정 회전은 안되게 한
        var enemyhpbarpos = hpbaroner.GetComponent<Enemyinfo>();
        // 1p와 2p는 체력이 보이는 방향이 다르기 때문에 1p이면 위쪽에 2p이면 아래쪽에 배치
        // enemy에 대한 체력
        if (enemyhpbarpos.enemyindex == 0)
        {
            hpcanvas.transform.position = new Vector3(x, y + 2.2f, z - 1.5f);
        }
        else
        {
            hpcanvas.transform.position = new Vector3(x, y + 2.2f, z + 1.5f);
        }



    }
}
