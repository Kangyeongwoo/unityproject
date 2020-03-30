using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pvpdata;
using FlatBuffers;
using System.Net.Sockets;
using System.Threading;

public class Playerfire : MonoBehaviour
{
    //총알 오브젝트
    public GameObject Bullet;

    //총알 발사 위치 변수
    public Transform playergunpos;

    //총알 발사 간격
    public static float fireRate =1f; //총알 지연 시간 설정
    private float nextFire = 0.2f; //다음 총알 발사시간

    //조준할 타겟 위치
    public Transform target;
    //플레이어 위치
    public Transform player;

    //모든 타겟을 담는 배열
    // public GameObject[] targets;
    // public GameObject targetobj;
    //모든 타겟의 위치 정보를 담는 배열
    // public Transform[] targettrs;
    //거리 배열 정렬을 위한 숫자
    //  public int count = 0;
    //플레이어와 타겟의 거리를 담는 배열
    //  public float[] dis;
    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;
    //    public static NetworkStream stream = Findenemy_flat.stream;


  

    void Update()
    {
        if (Control_flat.clickcount == 1)
        {
            if (skillfire.skilled == 0)
            {
                //총알 발사 합수 시작
                ShootMissile();
            }
        }


    }



    void ShootMissile()
    {
        /*
        //tag가 enemy인 모든 오브젝트를 배열에 저장
        targets = GameObject.FindGameObjectsWithTag("enemy");
        //   Debug.Log("targetsLength"+ targets.Length);

        //적의 수 만큼 target위치 배열  , 플레이어와 타겟의 거리 배열의 크기를 정함
        targettrs = new Transform[targets.Length];
        dis = new float[targets.Length];


        for (int i = 0; i < targets.Length; i++)
        {
            //타겟 오브젝트 들의 위치 정보를 배열에 저장
            targettrs[i] = targets[i].GetComponent<Transform>();

            //       Debug.Log("x : "+ targettrs[i].position.x+", y : "+ targettrs[i].position.y + ", z : "+ targettrs[i].position.z);
        }

        //   Debug.Log("plx : " + player.position.x + ", y : " + player.position.y + ", z : " + player.position.z);

        for (int i = 0; i < targets.Length; i++)
        {

            //타겟과 플레이어의 거리를 빠르게 계산해서 배열에 담기
            dis[i] = (targettrs[i].position - player.position).sqrMagnitude;

        }

        if (targets.Length == 0)
        {

            //타겟이 없으면 아무 것도 하지 않는다
        }
        else if (targets.Length == 1)

        {
            //타겟이 하나면 그것을 조준한다
            target = targettrs[0];

        }
        else
        {
            //타겟이 여러개면 반복해서 비교한다
            for (int i = 0; i < targets.Length - 1; i++)
            {
                //미리지정해둔 숫자와 그보다 하나 큰 것을 비교한다
                if (dis[count] <= dis[i + 1])
                {
                    //거리가 더 작은 것을 타겟으로 한.
                    target = targettrs[count];
                }
                else
                {
                    //다음 것이 더 크다면 다음 것을 타겟으로 한다
                    count = i + 1;
                    target = targettrs[count];

                }


            }


        }
        //비교가 끝나면 카운트는 초기화 한.
        count = 0;
        */
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        target = GameObject.FindWithTag("enemy").GetComponent<Transform>();
        playergunpos = GameObject.FindWithTag("Playergun").GetComponent<Transform>();
        //playergunpos = GameObject.FindWithTag("gun").GetComponent<Transform>();
        //마우스를 누를때 그리고 발사할 시간이 되었을때 다음을 진행
        if (!Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            Debug.Log("fire 1 :마우스 누를때 진행");
            //마우스를 눌렀을때나 누르고 있을때는 아무 행동도 하지 않는다
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                Debug.Log("fire 2 :눌려있을때 누르고 있을때");
            }
            else
            { //마우스에서 손가락을 떼고 있을 때 타겟을 조준하고 총알을 발사한.
                try
                {
                    float distance = Vector3.Distance(target.position, player.position);
                     if (distance <= 20) {
                        Debug.Log("fire 2 :손가락을 뗄떼");
                        //타겟과 플레이어 사이의 벡터를 구하고 정규화 한다
                        Vector3 vec = target.position - player.position;
                        vec.Normalize();
                        Debug.Log("target.localPosition:" + target.localPosition);

                        //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                        Quaternion q = Quaternion.LookRotation(vec);
                        player.rotation = q;
                        Debug.Log("player.rotation:" + q);


                        //보내는 데이터 버퍼 (id, nickname, userindex, 위치, 방향, 목적지, 공격상태 , 이동 상태 ,맞은 상태  ,)
                        builder = new FlatBufferBuilder(1024);
                        var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
                        var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));
                        Player.StartPlayer(builder);
                        Player.AddStartstate(builder, PlayerStart.Play);
                        Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
                        Player.AddId(builder, idoffset);
                        Player.AddNickname(builder, nicknameoffset);
                        Player.AddPlayerpos(builder, Vec3.CreateVec3(builder, player.position.x, player.position.y, player.position.z));
                        Player.AddPlayerrot(builder, player.eulerAngles.y);
                        Player.AddMovestate(builder, MoveState.Stop);
                        Player.AddDestinationpos(builder, Vec3.CreateVec3(builder, player.position.x, player.position.y, player.position.z));
                        Player.AddAttacked(builder, 0);
                        Player.AddSkillfire(builder,0);
                        Player.AddFire(builder, 1);

                        Offset<Player> pypplayer = Player.EndPlayer(builder);
                     //   builder.Finish(pypplayer.Value);

                     //   sendBuffer = builder.SizedByteArray();
                     //   sendbb = new ByteBuffer(sendBuffer);


                    Game.StartGame(builder);
                    Game.AddPlayer(builder, pypplayer);
                    Game.AddTablenum(builder, 0);
                    Offset<Game> game = Game.EndGame(builder);

                    builder.Finish(game.Value);
                    sendBuffer = builder.SizedByteArray();
                    sendbb = new ByteBuffer(sendBuffer);

                    Debug.Log("Client send ");
                        //  NetworkStream stream = socketConnection.GetStream();


                     
                        if (Findenemy_flat.stream.CanWrite)
                        {
                            Findenemy_flat.stream.Write(sendBuffer, 0, sendBuffer.Length);
                            Findenemy_flat.stream.Flush();
                        }

                        //플레이어의 총알 발사구인 firepos의 xyz를 정한다 y는 0으로 고정
                        //    Vector3 firepostrans = new Vector3(playergunpos.transform.position.x, 0.0f, playergunpos.transform.position.z);
                        //발사구의 위치를 적용
                        //   playergunpos.transform.position = firepostrans;

                        //다음 발사는 지금 시간 + 발사 간격
                        nextFire = Time.time + fireRate;
                        //총알을 firepos의 위치에서 적이 있는 방향으로 복제한다
                        Bullet = Resources.Load<GameObject>("Playerbullet");



                        if (PlayerPrefs.GetString("gunid").Equals("0")|| PlayerPrefs.GetString("gunid").Equals("1")) {
                            //총알 방향을 돌린다
                            Quaternion bullettest = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y, 0);
                             
                            Instantiate(Bullet, playergunpos.transform.position, bullettest);
                            // Debug.Log("FirePos.transform.position:" + playergunpos.transform.position + "FirePos.transform.rotation:" + playergunpos.transform.rotation);
                           // Quaternion bullet2 = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y - 30, 0);
                           // Quaternion bullet3 = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y + 30, 0);
                           // Instantiate(Bullet, playergunpos.transform.position, bullet2);
                           // Instantiate(Bullet, playergunpos.transform.position, bullet3);
                        }

                    }



                }
                catch (UnassignedReferenceException e1)
                {
                    //적이 하나도 없으면 이 에러가 발생하며 총을 쏘지 않는다
                    Debug.Log("fire 2 :에러"+ e1);
                }
                catch (MissingReferenceException e2)
                {
                    //적이 하나도 없으면 이 에러가 발생하며 총을 쏘지 않는다
                    Debug.Log("fire 2 :에러"+ e2);
                }
            }
        }

    }







}
