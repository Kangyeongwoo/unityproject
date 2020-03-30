using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towermonsterai : MonoBehaviour
{
    //총알 오브젝트
    public GameObject Bullet;

    //총알 발사 위치 변수
    public Transform playergunpos;

    //총알 발사 간격
  //  public static float fireRate = 5f; //총알 지연 시간 설정
  //  private float nextFire = 0.2f; //다음 총알 발사시간

    //조준할 타겟 위치
    public Transform target;
    //플레이어 위치
    public Transform monster;

    public Transform monster_child;

    public GameObject gunpos;
    public void Start()
    {
        monster = this.gameObject.GetComponent<Transform>();

        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        monster_child = monster.GetChild(0).gameObject.transform;

        gunpos = monster_child.GetChild(1).gameObject;

        StartCoroutine(monsterstart());
    }

    IEnumerator monsterstart() {
        while (true) {
            yield return new WaitForSeconds(1.6f);

            Vector3 vec = target.position - monster.position;
            vec.Normalize();
            Debug.Log("target.localPosition:" + target.localPosition);

            //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
            Quaternion q = Quaternion.LookRotation(vec);
            monster.rotation = q;

            Bullet = Resources.Load<GameObject>("Monster/towermonsterbullet");
            Quaternion bullettest = Quaternion.Euler(0, gunpos.transform.eulerAngles.y, 0);
            Quaternion bullettest2 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y+30f, 0);
            Quaternion bullettest3 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y-30f, 0);
            Instantiate(Bullet, gunpos.transform.position, bullettest);
            Instantiate(Bullet, gunpos.transform.position, bullettest2);
            Instantiate(Bullet, gunpos.transform.position, bullettest3);
        }

    }
}
