using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    void Update()
    {
        //프레임마다 오브젝트를 로컬좌표상에서 앞으로 1의 힘만큼 날아가라
        //y축에서의 포지션은 변하지 말아라 (위로 안날아가게한다)
      //  Vector3 test = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //총알의 포지션이 위로 안가게 바꿔준다
     //   transform.position = test;
        //  Debug.Log("transformtest");

        //총알이 로컬좌표상에서 앞으로 1의 힘만큼 날아가게 한다
        transform.Translate(Vector3.up * 0.8f);
       // Debug.Log("transform :" + transform.position.x + "," + transform.position.y + "," + transform.position.z + ",bullet:" + Vector3.forward * 1f);
    }
}
