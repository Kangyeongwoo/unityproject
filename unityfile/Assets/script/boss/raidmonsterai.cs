using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using LitJson;
using SimpleJSON;
using System.Net.Sockets;
using System.Threading;
using Bossdata;
using FlatBuffers;

public class raidmonsterai : MonoBehaviour
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
    public GameObject targetobj;
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


    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;
    public static float fireRate = 1f; //총알 지연 시간 설정
    private float nextFire = 0.5f; //다음 총알 발사시간


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
        if (move == 0 && check == true)
        {

            if (movecount == 0)
            {

                if (bossstart.remoteplayerlist.Contains(0))
                {
                    targetobj= (GameObject)bossstart.remoteplayerlist[0];
                    target = targetobj.transform.GetChild(0).GetComponent<Transform>();
                }
                else if(bossstart.remoteplayerlist.Contains(1))
                {

                    targetobj = (GameObject)bossstart.remoteplayerlist[1];
                    target = targetobj.transform.GetChild(0).GetComponent<Transform>();
                }
                else if (bossstart.remoteplayerlist.Contains(2)) {

                    targetobj = (GameObject)bossstart.remoteplayerlist[2];
                    target = targetobj.transform.GetChild(0).GetComponent<Transform>();
                }

                // GameObject remote_obj = (GameObject)bossstart.remoteplayerlist[i];


                Vector3 vec = target.position - monster.position;
                vec.Normalize();
                //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                Quaternion q = Quaternion.LookRotation(vec);
                monster.rotation = q;
                startpoint = monster.position;
                destination = target.position;
                movecount += 1;
                Debug.Log("movecount" + movecount);
                float yrot = monster.eulerAngles.y;
               

                builder = new FlatBufferBuilder(1024);

                Monster.StartMonster(builder);
                Monster.AddMonsterstate(builder, 0);
                Monster.AddPower(builder, 0);
                Monster.AddHp(builder, 0);
                Monster.AddMovestate(builder, MoveState.Move);
                Monster.AddFire(builder,0);
                Monster.AddMonsterpos(builder, Vec3.CreateVec3(builder, destination.x, destination.y, destination.z));
                Monster.AddMonsterrot(builder, yrot);
                Offset<Monster> raidbossmonster = Monster.EndMonster(builder);

                var monsters = new Offset<Monster>[1];
                monsters[0] = raidbossmonster;
                var monstersOffset = Game.CreateMonsterVector(builder, monsters);

                Game.StartGame(builder);
                Game.AddMonster(builder, monstersOffset);
                Game.AddTablenum(builder, 1);
                Offset<Game> game = Game.EndGame(builder);

                builder.Finish(game.Value);
                sendBuffer = builder.SizedByteArray();
                sendbb = new ByteBuffer(sendBuffer);


                if (findteam.stream.CanWrite)
                {
                    //데이터를 서버에 스트림으로 보낸다 bytearray
                    findteam.stream.Write(sendBuffer, 0, sendBuffer.Length);
                    findteam.stream.Flush();
                }


            }

         //   Debug.Log("startpoint:" + startpoint);
          //  Debug.Log("destination:" + destination);

            monster.position = Vector3.MoveTowards(monster.position, destination, 20f * Time.deltaTime);


            //목적지 보내기  목적지 받으면 그쪽으로 이동하게 하기


            if (monster.position == destination)
              //  if (Time.time > nextFire)
                {
                movecount = 0;

                //공격 신호 보내
                builder = new FlatBufferBuilder(1024);

                Monster.StartMonster(builder);
                Monster.AddMonsterstate(builder, 0);
                Monster.AddPower(builder, 0);
                Monster.AddHp(builder, 0);
                Monster.AddFire(builder, 1);
                Monster.AddMovestate(builder, MoveState.Stop);
                Monster.AddMonsterpos(builder, Vec3.CreateVec3(builder, monster.position.x, monster.position.y, monster.position.z));
                Monster.AddMonsterrot(builder, monster.eulerAngles.y);
                Offset<Monster> raidbossmonster = Monster.EndMonster(builder);

                var monsters = new Offset<Monster>[1];
                monsters[0] = raidbossmonster;
                var monstersOffset = Game.CreateMonsterVector(builder, monsters);

                Game.StartGame(builder);
                Game.AddMonster(builder, monstersOffset);
                Game.AddTablenum(builder, 1);
                Offset<Game> game = Game.EndGame(builder);

                builder.Finish(game.Value);
                sendBuffer = builder.SizedByteArray();
                sendbb = new ByteBuffer(sendBuffer);


                if (findteam.stream.CanWrite)
                {
                    //데이터를 서버에 스트림으로 보낸다 bytearray
                    findteam.stream.Write(sendBuffer, 0, sendBuffer.Length);
                    findteam.stream.Flush();
                }

                nextFire += fireRate;

                Bullet = Resources.Load<GameObject>("Bossraid/raidmonsterbullet");
                Quaternion bullettest1 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y, 0);
                Quaternion bullettest5 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 90f, 0);
                Quaternion bullettest8 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 90f, 0);
                Quaternion bullettest12 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 180f, 0);

                Quaternion bullettest2 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 30f, 0);
                Quaternion bullettest3 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 30f, 0);
                Quaternion bullettest4 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 60f, 0);
              
                Quaternion bullettest6 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 120f, 0);
                Quaternion bullettest7 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 60f, 0);
               
                Quaternion bullettest9 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 120f, 0);
                Quaternion bullettest10 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y + 160f, 0);
                Quaternion bullettest11 = Quaternion.Euler(0, gunpos.transform.eulerAngles.y - 160f, 0);
               


                Instantiate(Bullet, gunpos.transform.position, bullettest1);
                Instantiate(Bullet, gunpos.transform.position, bullettest5);
                Instantiate(Bullet, gunpos.transform.position, bullettest8);
                Instantiate(Bullet, gunpos.transform.position, bullettest12);

                  Instantiate(Bullet, gunpos.transform.position, bullettest2);
                 Instantiate(Bullet, gunpos.transform.position, bullettest3);
                 Instantiate(Bullet, gunpos.transform.position, bullettest4);

                 Instantiate(Bullet, gunpos.transform.position, bullettest6);
                 Instantiate(Bullet, gunpos.transform.position, bullettest7);
                
                 Instantiate(Bullet, gunpos.transform.position, bullettest9);
                 Instantiate(Bullet, gunpos.transform.position, bullettest10);
                 Instantiate(Bullet, gunpos.transform.position, bullettest11);
                
                 
                check = false;
                StartCoroutine(WaitForIt());

                //공격 데이터 보내기

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
