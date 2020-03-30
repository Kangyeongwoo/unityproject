using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using LitJson;
using SimpleJSON;
using System.Net.Sockets;
using System.Threading;
using Bossdata;
using FlatBuffers;

public class remoteplayercontrol : MonoBehaviour
{
    public ByteBuffer receivemessage3;
    public ByteBuffer remoteplayerbyte;
    public Vector3 destinationpos;
    public Animator anim2;
    public Vector3 playerpos;

    public GameObject monster;
    public GameObject monsterborn;

    public GameObject Bullet;

    public Transform target;

    public static float fireRate = 1f; //총알 지연 시간 설정
    private float nextFire = 0.2f; //다음 총알 발사시간
    private float nextFire1 = 0.2f; //다음 총알 발사시간
    private float nextFire2 = 0.2f; //다음 총알 발사시간

    private void Start()
    {

    }

    private void Update()
    {
        if (bossplayercontrol.serverMessgae2 != null)
        {

            Game gameremoteplayer = new Game();
            Player remoteplayer;
           // Monster raidmonster;
           
            int gametable = 100;
            // 큐 안에 데이터가 있는지 확인한다
            if (bossplayercontrol.receivequeue.Count != 0)
            {

                remoteplayerbyte = bossplayercontrol.receivequeue.Dequeue();

                // pvpenemy = Player.GetRootAsPlayer(pvpenemybyte);
                gameremoteplayer = Game.GetRootAsGame(remoteplayerbyte);

                gametable = gameremoteplayer.Tablenum;

                if (gametable == 0)
                {
                    remoteplayer = (Player)gameremoteplayer.Player;

                    if (remoteplayer.Skillfire == 0)
                    {
                        receivemessage3 = remoteplayerbyte;
                    }
                }
            }
            else
            {
                if (receivemessage3 != null)
                {
                    //  gameenemy = Game.GetRootAsGame(Control_flat.serverMessgae2);
                    gameremoteplayer = Game.GetRootAsGame(receivemessage3);
                    gametable = gameremoteplayer.Tablenum;
                    //  pvpenemy = Player.GetRootAsPlayer(Control_flat.serverMessgae2);
                }
            }


            if (gametable == 0)
            {

                remoteplayer = (Player)gameremoteplayer.Player;

                int remoteplayerkey = remoteplayer.Roomindex;
                //     Debug.Log("remote room index!!!!!!!!!!!!!!!: " + remoteplayerkey);
                if (bossstart.remoteplayerlist.Contains(remoteplayerkey)) { 
                GameObject remote_obj = (GameObject)bossstart.remoteplayerlist[remoteplayerkey];

                //      Debug.Log("remote room index@@@@@@@@@@@@@@@@: "+ remote_obj.transform.Find("remoteplayer").GetComponent<bossremoteplayerinfo>().remoteplayerindex);

                Transform remotetrans = remote_obj.transform.Find("remoteplayer").GetComponent<Transform>();

                GameObject remoterealbody = remote_obj.transform.Find("remoteplayer").gameObject;

                anim2 = remote_obj.transform.Find("remoteplayer").GetComponent<Animator>();

                if (remoteplayer.Attacked == 1)
                {
                    var remotehp = remoterealbody.GetComponent<bossremoteplayerinfo>();

                    remotehp.remoteplayerhp = remoteplayer.Currenthp;

                    if (remotehp.remoteplayerhp <= 0)
                    {
                        remotehp.remoteplayerhp = 0;
                    }
                    string hp = remotehp.remoteplayerhp.ToString();

                    GameObject test = remote_obj.transform.Find("remoteplayerhpfac").gameObject;
                    GameObject test2 = test.transform.GetChild(0).gameObject;
                    Slider remotehpbar = test2.GetComponent<Slider>();
                    Text remotehp_text = test2.transform.Find("playerhptext").GetComponent<Text>();
                    remotehp_text.text = hp;
                    remotehpbar.value = (float)remotehp.remoteplayerhp / (float)remotehp.remoteplayermaxhp;

                    if (remotehp.remoteplayerhp == 0)
                    {

                        Debug.Log("Dead:" + remoteplayerkey);
                        bossstart.remoteplayerlist.Remove(remoteplayerkey);
                        Destroy(remote_obj);

                            if (!bossstart.deadtable.Contains(remoteplayerkey)) {
                                bossstart.deadtable.Add(remoteplayerkey, "dead");
                            }

                     }



                }
                else
                {

                    //움직이는 상태이면
                    if (remoteplayer.Movestate == MoveState.Move)
                    {
                        //      Debug.Log("test4 move");
                        anim2.SetBool("run", true);
                        float roty = remoteplayer.Playerrot;
                        //    Debug.Log(roty);
                        remotetrans.eulerAngles = new Vector3(0, roty, 0);
                        //remotetrans.transform.Translate(Vector3.forward * Time.deltaTime * 10f);
                        //   Debug.Log("test4 remotetrans"+roty);

                        var destinationpos_flat = remoteplayer.Destinationpos.Value;
                        //  Debug.Log("test4 destinationpos_flat" + pvpenemy.Destinationpos.Value);

                        destinationpos = new Vector3(destinationpos_flat.X, destinationpos_flat.Y, destinationpos_flat.Z);
                        // Debug.Log("test4 destinationpos : " + destinationpos_flat.X + ","+ destinationpos_flat.Y + ","+ destinationpos_flat.Z + ",");

                        remotetrans.transform.position = Vector3.MoveTowards(remotetrans.transform.position, destinationpos, 10f * Time.deltaTime);

                        // if (Vector3.Distance(remotetrans.transform.position, destinationpos) <= 0.5f)
                        //  {
                        //      Debug.Log("Vector3.Distance" + Vector3.Distance(remotetrans.transform.position, destinationpos));
                        //      anim2.SetBool("run", false);
                        //  }
                    }
                    else if (remoteplayer.Movestate == MoveState.Stop)
                    {
                        var enemypos_flat = remoteplayer.Playerpos.Value;
                        playerpos = new Vector3(enemypos_flat.X, enemypos_flat.Y, enemypos_flat.Z);
                        //멈추자 마자 공격을 아직 안했을때
                        anim2.SetBool("run", false);
                        if (remoteplayer.Fire == 0)
                        {
                            // Debug.Log("test4 stop");
                            anim2.SetBool("run", false);

                            float roty = remoteplayer.Playerrot;
                            remotetrans.eulerAngles = new Vector3(0, roty, 0);
                                GameObject remotebody = remote_obj.transform.Find("remoteplayer").gameObject;
                                Transform remotegunpos = remotebody.transform.GetChild(2).gameObject.GetComponent<Transform>();
                                Transform remotegunpos2 = remotebody.transform.GetChild(3).gameObject.GetComponent<Transform>();


                                Debug.Log("playerposx : " + enemypos_flat.X + " , playerposy : " + enemypos_flat.Y + " , playerposz : " + enemypos_flat.Z);
                            remotetrans.transform.position = Vector3.MoveTowards(remotetrans.transform.position, playerpos, 10f * Time.deltaTime);

                            if (remoteplayer.Skillfire == 1)
                            {

                                    if (remoterealbody.GetComponent<bossremoteplayerinfo>().remoteplayerskill1id == 1)
                                    {
                                        Vector3 vec = target.position - remotetrans.transform.position;
                                        vec.Normalize();
                                        Debug.Log("Skill!!!!!!!!!!!!!!!!!!.localPosition:" + target.localPosition);

                                        //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                                        Quaternion q = Quaternion.LookRotation(vec);
                                        remotetrans.transform.rotation = q;

                                        GameObject skill1 = Resources.Load<GameObject>("Bossskill/skill1remote");


                                        Quaternion skillrot = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y, 0);

                                        GameObject skill1real = Instantiate(skill1, remotegunpos.transform.position, skillrot);

                                    }
                                //총알을 firepos의 위치에서 적이 있는 방향으로 복제한다


                            }


                        }//공격을 했을 때
                        else if (remoteplayer.Fire == 1)
                        {

                            remotetrans.transform.position = Vector3.MoveTowards(remotetrans.transform.position, playerpos, 10f * Time.deltaTime);
                            target = GameObject.FindWithTag("enemy").GetComponent<Transform>();
                            if (remoteplayerkey == 0)
                            {
                                if (Time.time > nextFire)
                                {
                                    Debug.Log("Fire!!!");
                                    Vector3 vec = target.position - remotetrans.transform.position;
                                    vec.Normalize();
                                    Debug.Log("target.localPosition:" + target.localPosition);

                                    //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                                    Quaternion q = Quaternion.LookRotation(vec);
                                    remotetrans.transform.rotation = q;
                                    nextFire = Time.time + fireRate;
                                    //총알을 firepos의 위치에서 적이 있는 방향으로 복제한다
                                    Bullet = Resources.Load<GameObject>("Bossraid/bossremoteplayerbullet");
                                    GameObject remotebody = remote_obj.transform.Find("remoteplayer").gameObject;
                                    Transform remotegunpos = remotebody.transform.GetChild(2).gameObject.GetComponent<Transform>();
                                    Transform remotegunpos2 = remotebody.transform.GetChild(3).gameObject.GetComponent<Transform>();
                                    
                                    List<int> abilitylist = (List<int>)bossstart.abilitytable[remoteplayerkey];
                                        if (abilitylist.Contains(4))
                                    {
                                        Quaternion bullet2 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y, 0);
                                        Quaternion bullet3 = Quaternion.Euler(90, remotegunpos2.transform.eulerAngles.y, 0);
                                        // Vector3 bulletpos1 = new Vector3(playergunpos.transform.position.x +0.5f, playergunpos.transform.position.y, playergunpos.transform.position.z);
                                        //  Vector3 bulletpos2 = new Vector3(playergunpos.transform.position.x -0.5f, playergunpos.transform.position.y, playergunpos.transform.position.z);

                                        Instantiate(Bullet, remotegunpos.transform.position, bullet2);
                                        Instantiate(Bullet, remotegunpos2.transform.position, bullet3);
                                    }
                                    else
                                    {

                                        //총알 방향을 돌린다
                                        Quaternion bullet = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet);

                                    }

                                    //사선추가
                                    if (abilitylist.Contains(3))
                                    {
                                        Quaternion bullet2 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y - 30, 0);
                                        Quaternion bullet3 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 30, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet2);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet3);
                                    }

                                    //후방 추가
                                    if (abilitylist.Contains(5))
                                    {

                                        Quaternion bullet4 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 180, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet4);
                                    }

                                    //측면 추가
                                    if (abilitylist.Contains(6))
                                    {
                                        Quaternion bullet5 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y - 90, 0);
                                        Quaternion bullet6 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 90, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet5);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet6);
                                    }



                                }
                            }

                            else if (remoteplayerkey == 1)
                            {
                                if (Time.time > nextFire1)
                                {
                                    Debug.Log("Fire!!!");
                                    Vector3 vec = target.position - remotetrans.transform.position;
                                    vec.Normalize();
                                    Debug.Log("target.localPosition:" + target.localPosition);

                                    //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                                    Quaternion q = Quaternion.LookRotation(vec);
                                    remotetrans.transform.rotation = q;
                                    nextFire1 = Time.time + fireRate;
                                    //총알을 firepos의 위치에서 적이 있는 방향으로 복제한다
                                    Bullet = Resources.Load<GameObject>("Bossraid/bossremoteplayerbullet");
                                    GameObject remotebody = remote_obj.transform.Find("remoteplayer").gameObject;
                                    Transform remotegunpos = remotebody.transform.GetChild(2).gameObject.GetComponent<Transform>();
                                    Transform remotegunpos2 = remotebody.transform.GetChild(3).gameObject.GetComponent<Transform>();

                                        List<int> abilitylist = (List<int>)bossstart.abilitytable[remoteplayerkey];

                                        if (abilitylist.Contains(4))
                                    {
                                        Quaternion bullet2 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y, 0);
                                        Quaternion bullet3 = Quaternion.Euler(90, remotegunpos2.transform.eulerAngles.y, 0);
                                        // Vector3 bulletpos1 = new Vector3(playergunpos.transform.position.x +0.5f, playergunpos.transform.position.y, playergunpos.transform.position.z);
                                        //  Vector3 bulletpos2 = new Vector3(playergunpos.transform.position.x -0.5f, playergunpos.transform.position.y, playergunpos.transform.position.z);

                                        Instantiate(Bullet, remotegunpos.transform.position, bullet2);
                                        Instantiate(Bullet, remotegunpos2.transform.position, bullet3);
                                    }
                                    else
                                    {

                                        //총알 방향을 돌린다
                                        Quaternion bullet = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet);

                                    }

                                    //사선추가
                                    if (abilitylist.Contains(3))
                                    {
                                        Quaternion bullet2 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y - 30, 0);
                                        Quaternion bullet3 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 30, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet2);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet3);
                                    }

                                    //후방 추가
                                    if (abilitylist.Contains(5))
                                    {

                                        Quaternion bullet4 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 180, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet4);
                                    }

                                    //측면 추가
                                    if (abilitylist.Contains(6))
                                    {
                                        Quaternion bullet5 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y - 90, 0);
                                        Quaternion bullet6 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 90, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet5);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet6);
                                    }


                                }


                            }
                            else if (remoteplayerkey == 2)
                            {
                                if (Time.time > nextFire2)
                                {
                                    Debug.Log("Fire!!!");
                                    Vector3 vec = target.position - remotetrans.transform.position;
                                    vec.Normalize();
                                    Debug.Log("target.localPosition:" + target.localPosition);

                                    //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                                    Quaternion q = Quaternion.LookRotation(vec);
                                    remotetrans.transform.rotation = q;
                                    nextFire2 = Time.time + fireRate;
                                    //총알을 firepos의 위치에서 적이 있는 방향으로 복제한다
                                    Bullet = Resources.Load<GameObject>("Bossraid/bossremoteplayerbullet");
                                    GameObject remotebody = remote_obj.transform.Find("remoteplayer").gameObject;
                                    Transform remotegunpos = remotebody.transform.GetChild(2).gameObject.GetComponent<Transform>();
                                    Transform remotegunpos2 = remotebody.transform.GetChild(3).gameObject.GetComponent<Transform>();
                                        List<int> abilitylist = (List<int>)bossstart.abilitytable[remoteplayerkey];

                                        if (abilitylist.Contains(4))
                                    {
                                        Quaternion bullet2 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y, 0);
                                        Quaternion bullet3 = Quaternion.Euler(90, remotegunpos2.transform.eulerAngles.y, 0);
                                        // Vector3 bulletpos1 = new Vector3(playergunpos.transform.position.x +0.5f, playergunpos.transform.position.y, playergunpos.transform.position.z);
                                        //  Vector3 bulletpos2 = new Vector3(playergunpos.transform.position.x -0.5f, playergunpos.transform.position.y, playergunpos.transform.position.z);

                                        Instantiate(Bullet, remotegunpos.transform.position, bullet2);
                                        Instantiate(Bullet, remotegunpos2.transform.position, bullet3);
                                    }
                                    else
                                    {

                                        //총알 방향을 돌린다
                                        Quaternion bullet = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet);

                                    }

                                    //사선추가
                                    if (abilitylist.Contains(3))
                                    {
                                        Quaternion bullet2 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y - 30, 0);
                                        Quaternion bullet3 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 30, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet2);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet3);
                                    }

                                    //후방 추가
                                    if (abilitylist.Contains(5))
                                    {

                                        Quaternion bullet4 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 180, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet4);
                                    }

                                    //측면 추가
                                    if (abilitylist.Contains(6))
                                    {
                                        Quaternion bullet5 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y - 90, 0);
                                        Quaternion bullet6 = Quaternion.Euler(90, remotegunpos.transform.eulerAngles.y + 90, 0);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet5);
                                        Instantiate(Bullet, remotegunpos.transform.position, bullet6);
                                    }



                                }

                            }

                        }






                    }




                }
            }

            }else if (gametable == 1) {
                /*
                raidmonster = (Monster)gameremoteplayer.Monster(0);
                Debug.Log("monsterpower:"+raidmonster.Power);
                if (raidmonster.Monsterstate == 1)
                {

                    monster = Resources.Load<GameObject>("Bossraid/raidbossfac");

                    monsterborn = Instantiate(monster, new Vector3(0f, 0f, 0f), Quaternion.identity);


                }
                else
                {



                }
                */
            }



        }

    }
}
