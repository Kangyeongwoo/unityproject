using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossstoneai : MonoBehaviour
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

    public int move;
    public int attack;
    public int movecount;
    // Start is called before the first frame update

    public Vector3 startpoint;
    public Vector3 destination;
    public bool check;

    void Start()
    {

        monster = this.gameObject.GetComponent<Transform>();

        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        monster_child = monster.GetChild(0).gameObject.transform;

        gunpos = monster_child.GetChild(2).gameObject;
        check = true;
        move = 0;
        attack = 0;
        movecount = 0;
    }

    private void Update()
    {
        if (move == 0 && check==true)
        {

            if (movecount == 0)
            {
                Vector3 vec = target.position - monster.position;
                vec.Normalize();
                //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                Quaternion q = Quaternion.LookRotation(vec);
                monster.rotation = q;
                startpoint = monster.position;
                destination = target.position;
                movecount += 1;
                Debug.Log("movecount" + movecount);
            }

            Debug.Log("startpoint:" + startpoint);
            Debug.Log("destination:" + destination);

            monster.position = Vector3.MoveTowards(monster.position, destination, 20f * Time.deltaTime);


            if (monster.position == destination)
            {
                movecount = 0;

                Bullet = Resources.Load<GameObject>("Monster/bossstonebullet");
                Quaternion bullettest1 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y, 0);
                Quaternion bullettest2 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 30f, 0);
                Quaternion bullettest3 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 30f, 0);
                Quaternion bullettest4 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 60f, 0);
                Quaternion bullettest5 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 90f, 0);
                Quaternion bullettest6 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 120f, 0);
                Quaternion bullettest7 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 60f, 0);
                Quaternion bullettest8 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 90f, 0);
                Quaternion bullettest9 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 120f, 0);
                Quaternion bullettest10 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 160f, 0);
                Quaternion bullettest11 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 160f, 0);
                Quaternion bullettest12 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 180f, 0);

                Instantiate(Bullet, gunpos.transform.position, bullettest1);
                Instantiate(Bullet, gunpos.transform.position, bullettest2);
                Instantiate(Bullet, gunpos.transform.position, bullettest3);
                Instantiate(Bullet, gunpos.transform.position, bullettest4);
                Instantiate(Bullet, gunpos.transform.position, bullettest5);
                Instantiate(Bullet, gunpos.transform.position, bullettest6);
                Instantiate(Bullet, gunpos.transform.position, bullettest7);
                Instantiate(Bullet, gunpos.transform.position, bullettest8);
                Instantiate(Bullet, gunpos.transform.position, bullettest9);
                Instantiate(Bullet, gunpos.transform.position, bullettest10);
                Instantiate(Bullet, gunpos.transform.position, bullettest11);
                Instantiate(Bullet, gunpos.transform.position, bullettest12);

                check = false;
                StartCoroutine(WaitForIt());

            }


        }



    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(2.0f); //2초 기다린다.
        // 수행할 액션들 
        check = true;
    }




}
