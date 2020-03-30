using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pvpdata;
using FlatBuffers;
using System.Net.Sockets;
using System.Threading;

public class Playerfire2 : MonoBehaviour
{
    //총알 오브젝트
    public GameObject Bullet;

    //총알 발사 위치 변수
    public Transform playergunpos;

    //총알 발사 간격
    private float fireRate = 0.8f; //총알 지연 시간 설정
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

   
    public void ShootMissile2()
    {

        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        target = GameObject.FindWithTag("enemy").GetComponent<Transform>();
        playergunpos = GameObject.FindWithTag("Playergun").GetComponent<Transform>();
        //마우스를 누를때 그리고 발사할 시간이 되었을때 다음을 진행
          try
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
                        Player.AddDestinationpos(builder, Vec3.CreateVec3(builder, player.position.x, player.position.y, player.position.z));
                        Player.AddAttacked(builder, 0);
                        Player.AddFire(builder, 1);

                        Offset<Player> pypplayer = Player.EndPlayer(builder);
                        builder.Finish(pypplayer.Value);

                        sendBuffer = builder.SizedByteArray();
                        sendbb = new ByteBuffer(sendBuffer);

                        Debug.Log("Client send ");
                        //  NetworkStream stream = socketConnection.GetStream();


                        /*
                        if (stream.CanWrite)
                        {
                            stream.Write(sendBuffer, 0, sendBuffer.Length);
                            stream.Flush();
                        }
                        */
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

                        //총알 방향을 돌린다
                        Quaternion bullettest = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y, 0);
                        //  Debug.Log("bullettest:"+ player.transform.eulerAngles.y);
                        // Quaternion bullettest = Quaternion.Euler(270, playergunpos.transform.eulerAngles.y, 0);
                        // Instantiate(Bullet, playergunpos.transform.position, playergunpos.transform.rotation);
                        Instantiate(Bullet, playergunpos.transform.position, bullettest);
                        // Debug.Log("FirePos.transform.position:" + playergunpos.transform.position + "FirePos.transform.rotation:" + playergunpos.transform.rotation);

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
