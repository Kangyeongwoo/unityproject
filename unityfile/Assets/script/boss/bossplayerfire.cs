using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bossdata;
using FlatBuffers;
using System.Net.Sockets;
using System.Threading;

public class bossplayerfire : MonoBehaviour
{

    //총알 오브젝트
    public GameObject Bullet;

    //총알 발사 위치 변수
    public Transform playergunpos;
    public Transform playergunpos2;

    //총알 발사 간격
    public static float fireRate = 1f; //총알 지연 시간 설정
    private float nextFire = 0.2f; //다음 총알 발사시간

    //조준할 타겟 위치
    public Transform target;
    //플레이어 위치
    public Transform player;

    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;

    public bossplayerinfo playerinfo;

    // Start is called before the first frame update
    void Start()
    {
        playerinfo = GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>();
    }

    // Update is called once per frame
    void Update()
    {

        if (bossplayercontrol.clickcount == 1)
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

        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        target = GameObject.FindWithTag("enemy").GetComponent<Transform>();
        playergunpos = GameObject.FindWithTag("Playergun").GetComponent<Transform>();
        playergunpos2 = GameObject.FindWithTag("Playergun2").GetComponent<Transform>();
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
                    if (distance <= 100)
                    {
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
                        Player.AddRoomindex(builder, playerinfo.playerindex);
                        Player.AddDestinationpos(builder, Vec3.CreateVec3(builder, player.position.x, player.position.y, player.position.z));
                        Player.AddAttacked(builder, 0);
                        Player.AddSkillfire(builder, 0);
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



                        if (findteam.stream.CanWrite)
                        {
                            findteam.stream.Write(sendBuffer, 0, sendBuffer.Length);
                            findteam.stream.Flush();
                        }

                        //플레이어의 총알 발사구인 firepos의 xyz를 정한다 y는 0으로 고정
                        //    Vector3 firepostrans = new Vector3(playergunpos.transform.position.x, 0.0f, playergunpos.transform.position.z);
                        //발사구의 위치를 적용
                        //   playergunpos.transform.position = firepostrans;

                        //다음 발사는 지금 시간 + 발사 간격
                        nextFire = Time.time + fireRate;
                        //총알을 firepos의 위치에서 적이 있는 방향으로 복제한다
                        Bullet = Resources.Load<GameObject>("Bossraid/bossplayerbullet");


                        List<int> abilitylist = (List<int>)bossstart.abilitytable[playerinfo.playerindex];

                        if (abilitylist.Contains(4))
                        {
                            Quaternion bullet2 = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y, 0);
                            Quaternion bullet3 = Quaternion.Euler(90, playergunpos2.transform.eulerAngles.y, 0);
                            // Vector3 bulletpos1 = new Vector3(playergunpos.transform.position.x +0.5f, playergunpos.transform.position.y, playergunpos.transform.position.z);
                            //  Vector3 bulletpos2 = new Vector3(playergunpos.transform.position.x -0.5f, playergunpos.transform.position.y, playergunpos.transform.position.z);

                            Instantiate(Bullet, playergunpos.transform.position, bullet2);
                            Instantiate(Bullet, playergunpos2.transform.position, bullet3);
                        }
                        else
                        {

                            //총알 방향을 돌린다
                            Quaternion bullet = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y, 0);
                            Instantiate(Bullet, playergunpos.transform.position, bullet);

                        }

                        //사선추가
                        if (abilitylist.Contains(3))
                        {
                            Quaternion bullet2 = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y - 30, 0);
                            Quaternion bullet3 = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y + 30, 0);
                            Instantiate(Bullet, playergunpos.transform.position, bullet2);
                            Instantiate(Bullet, playergunpos.transform.position, bullet3);
                        }

                        //후방 추가
                        if (abilitylist.Contains(5))
                        {

                            Quaternion bullet4 = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y + 180, 0);
                            Instantiate(Bullet, playergunpos.transform.position, bullet4);
                        }

                        //측면 추가
                        if (abilitylist.Contains(6))
                        {
                            Quaternion bullet5 = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y - 90, 0);
                            Quaternion bullet6 = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y + 90, 0);
                            Instantiate(Bullet, playergunpos.transform.position, bullet5);
                            Instantiate(Bullet, playergunpos.transform.position, bullet6);
                        }

                    }



                }
                catch (UnassignedReferenceException e1)
                {
                    //적이 하나도 없으면 이 에러가 발생하며 총을 쏘지 않는다
                    Debug.Log("fire 2 :에러" + e1);
                }
                catch (MissingReferenceException e2)
                {
                    //적이 하나도 없으면 이 에러가 발생하며 총을 쏘지 않는다
                    Debug.Log("fire 2 :에러" + e2);
                }
            }
        }



    }







}
