using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleskillfire : MonoBehaviour
{
    Transform playergunpos;

    int skill1id;

    GameObject skill1real;

    GameObject skill1;

    GameObject playergun;

    public static int skilled;

    Transform player;
    Transform target;
    private void Start()
    {
        skilled = 0;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        var playerinfo = GameObject.FindWithTag("Player").GetComponent<singleplayerinfo>();
        skill1id = playerinfo.playerskill1id;
        playergun = GameObject.FindWithTag("Playergun");


    }


    public void skill()
    {
        if (Mathf.Approximately(singlecontrl.fillPercentage, 1f))

        {
            Debug.Log("skillstart");
            if (skill1id == 1)
            {
                target = GameObject.FindWithTag("Player").GetComponent<singleplayerfire>().target;
                Debug.Log("skillstart2");
                skilled = 1;
                //skillfire = 1 데이터 전송
                string skillname = "skill" + skill1id.ToString();

                Vector3 vec = target.position - player.position;
                vec.Normalize();
                Debug.Log("target.localPosition:" + target.localPosition);

                //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                Quaternion q = Quaternion.LookRotation(vec);
                player.rotation = q;
                Debug.Log("player.rotation:" + q);


                Debug.Log("skillplayerrot1:" + player.eulerAngles.y);
                Debug.Log("skillplayerrot2:" + player.localEulerAngles.y);
                Debug.Log("skillplayerrot3:" + player.rotation.y);
                Debug.Log("skillplayerrot4:" + player.localRotation.y);


                //총구에서 스킬이 나가도록
                playergunpos = GameObject.FindWithTag("Playergun").GetComponent<Transform>();

                skill1 = Resources.Load<GameObject>("Skill/" + skillname);

              
                Quaternion skillrot = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y, 0);

                skill1real = Instantiate(skill1, playergunpos.transform.position, skillrot);

                singlecontrl.currentValue = 0f;
                singlecontrl.fillImage.fillAmount = 0;
                singlecontrl.fillPercentage = 0;

                skill1real.transform.SetParent(playergun.transform);

                StartCoroutine(delay());


            }

        }
    }


    public IEnumerator delay()
    {

        yield return new WaitForSeconds(1f);
        Destroy(skill1real);
        yield return new WaitForSeconds(0.2f);
        skilled = 0;

    }
}
