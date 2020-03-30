using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
//using LitJson;
using SimpleJSON;
using System.Net.Sockets;
using System.Threading;
using Pvpdata;
using FlatBuffers;

public class Enemyfire : MonoBehaviour
{
    public ByteBuffer pvpenemybyte;
    //총알 발사 간격
    private float fireRate = 0.8f; //총알 지연 시간 설정
    private float nextFire = 0.2f; //다음 총알 발사시간
    private Transform enemy;
    private Transform target;
    private Transform enemygunpos;
    public GameObject Bullet;

    void Update()
    {
        //총알 발사 합수 시작
        ShootMissile();


    }

    void ShootMissile()
    {
        //  Debug.Log("test2" + Control_flat.serverMessgae2);
        Player pvpenemy;
        //     Player pvpenemy = Player.GetRootAsPlayer(Control_flat.serverMessgae2);

        //queue에 데이터가 있는지 확인
        if (Control_flat.receivequeue.Count != 0)
        {
            //큐가 있으면 그걸 가지고 와서 player를 만든다
            pvpenemybyte = Control_flat.receivequeue.Dequeue();
            pvpenemy = Player.GetRootAsPlayer(pvpenemybyte);

        }
        else
        {
            //큐가 없으면 마지막껄 가지고 플레이어를 만든다
            pvpenemy = Player.GetRootAsPlayer(Control_flat.serverMessgae2);
        }

        if(pvpenemy.Movestate == MoveState.Stop) {

            enemy = GameObject.FindWithTag("enemy").GetComponent<Transform>();
            target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            enemygunpos = GameObject.FindWithTag("enemygun").GetComponent<Transform>();

            if(Time.time > nextFire) {

                try
                {
                    Debug.Log("fire 2 :손가락을 뗄떼");
                    //타겟과 플레이어 사이의 벡터를 구하고 정규화 한다
                    Vector3 vec = target.position - enemy.position;
                    vec.Normalize();
                    Debug.Log("target.localPosition:" + target.localPosition);

                    //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                    Quaternion q = Quaternion.LookRotation(vec);
                    enemy.rotation = q;
                    Debug.Log("player.rotation:" + q);

                    //플레이어의 총알 발사구인 firepos의 xyz를 정한다 y는 0으로 고정
                    //    Vector3 firepostrans = new Vector3(playergunpos.transform.position.x, 0.0f, playergunpos.transform.position.z);
                    //발사구의 위치를 적용
                    //   playergunpos.transform.position = firepostrans;

                    //다음 발사는 지금 시간 + 발사 간격
                    nextFire = Time.time + fireRate;
                    //총알을 firepos의 위치에서 적이 있는 방향으로 복제한다
                    Bullet = Resources.Load<GameObject>("Enemybullet");



                  //  Quaternion bullettest = Quaternion.Euler(90, enemygunpos.transform.eulerAngles.y, 0);
                    //  Debug.Log("bullettest:"+ player.transform.eulerAngles.y);
                    // Quaternion bullettest = Quaternion.Euler(270, playergunpos.transform.eulerAngles.y, 0);
                    //총알을 총알발사 위치의 방향에 생성
                     Instantiate(Bullet, enemygunpos.transform.position, enemygunpos.transform.rotation);
                   // Instantiate(Bullet, enemygunpos.transform.position, bullettest);

                    Debug.Log("enemy.transform.position:" + enemygunpos.transform.position + "enemy.transform.rotation:" + enemygunpos.transform.rotation);
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
