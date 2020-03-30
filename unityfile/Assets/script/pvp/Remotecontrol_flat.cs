using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
//using LitJson;
using SimpleJSON;
using System.Net.Sockets;
using System.Threading;
using Pvpdata;
using FlatBuffers;

public class Remotecontrol_flat : MonoBehaviour
{
    public Vector3 playerpos;

    public Vector3 destinationpos;

    public Quaternion playerrot;

    //  public Vector3 playerrot;

    public Transform remotetrans;

    public Rigidbody remoterigid;

    static public Socket clientSocket = null;

    public Animator anim2;

    public ByteBuffer pvpenemybyte;

    public Transform target;

    public static float fireRate =1f ; //총알 지연 시간 설정
    private float nextFire = 0.2f; //다음 총알 발사시간
    public GameObject Bullet;
    public Transform enemygunpos;

    public Enemyinfo enemyinfo;
    public int enemyhp;
    public int enemymaxhp;
    public Text enemyhp_text;
    public Slider enemyhpbar;

    public GameObject fire;
    public GameObject floorpac;
    public List<GameObject> floorlist = new List<GameObject>();

    public GameObject hprestore;

    public ByteBuffer receivemessage3;

    GameObject skill1;

    GameObject skill1real;

    int skillstate;

    public void Start()
    {
        //상대 애니메이션
        anim2 = GameObject.FindWithTag("enemy").GetComponent<Animator>();
        //상대 위치
        remotetrans = GameObject.FindWithTag("enemy").GetComponent<Transform>();
        //상대 강체
        remoterigid = GameObject.FindWithTag("enemy").GetComponent<Rigidbody>();
        //플레이어 위치
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        enemyinfo = GameObject.FindWithTag("enemy").GetComponent<Enemyinfo>();

        //상대 총구
        enemygunpos = GameObject.FindWithTag("enemygun").GetComponent<Transform>();
        //상대 최대 hp
        enemymaxhp = GameObject.FindWithTag("enemy").GetComponent<Enemyinfo>().enemymaxhp;
        //상대 hptext

        enemyhp_text = GameObject.FindWithTag("enemy").GetComponent<Enemyhpbar>().enemyhp_text;
        //상대 hpbar
        enemyhpbar = GameObject.FindWithTag("enemy").GetComponent<Enemyhpbar>().enemyhpbar;

        floorpac = GameObject.Find("floorpac");
        for (int i = 0; i < 20; i++)
        {

            floorlist.Add(floorpac.transform.GetChild(i).gameObject);

        }

        hprestore = Resources.Load<GameObject>("hprestore");

        skillstate = 0;
    }

    private void Update()
    {

        try
        {


            if (Control_flat.serverMessgae2 != null)
            {
                //  Debug.Log("test2" + Control_flat.serverMessgae2);
                Game gameenemy;
                Player pvpenemy;
                Item item;
                Mapeffect effect;
                int gametable;
                // Player pvpenemy = Player.GetRootAsPlayer(Control_flat.serverMessgae2);
                if (Control_flat.receivequeue.Count != 0)
                {

                    pvpenemybyte = Control_flat.receivequeue.Dequeue();

                    // pvpenemy = Player.GetRootAsPlayer(pvpenemybyte);
                    gameenemy = Game.GetRootAsGame(pvpenemybyte);
                 
                    gametable = gameenemy.Tablenum;

                    if (gametable == 0)
                    {
                        pvpenemy = (Player)gameenemy.Player;

                        if (pvpenemy.Skillfire == 0)
                        {
                            receivemessage3 = pvpenemybyte;
                        }
                    }                
                    }
                else
                {
                    //  gameenemy = Game.GetRootAsGame(Control_flat.serverMessgae2);
                    gameenemy = Game.GetRootAsGame(receivemessage3);
                    gametable = gameenemy.Tablenum;
                    //  pvpenemy = Player.GetRootAsPlayer(Control_flat.serverMessgae2);
                }

                //  Player pvpenemy2 = Player.GetRootAsPlayer(pvpenemybyte);
                //    Debug.Log("test3" + pvpenemy2.Playerrot);
                //   Debug.Log("test3" + pvpenemy2.Movestate);

                //현재 있는 원격 플레이어의 transform 받기
                //enemey가 공격을 받았으면
                Debug.Log("gametable:"+gametable);

                if(gametable == 0) {
                    pvpenemy = (Player)gameenemy.Player;

                    if (pvpenemy.Attacked == 1) {
                    //현재 채력을 확인해서 hpbar에 반영한다 
                    enemyhp = pvpenemy.Currenthp;
                    Debug.Log("enemyattack");
                    if (enemyhp <= 0) {
                        enemyhp = 0;
                    }
                    string hp = enemyhp.ToString();
                    enemyhp_text.text = hp;
                    GameObject.FindWithTag("enemy").GetComponent<Enemyhpbar>().enemyhp = enemyhp;
                    enemyhpbar.value = (float)enemyhp / (float)enemymaxhp;

                    }
                    else
                    {
                        //움직이는 상태이면
                        if (pvpenemy.Movestate == MoveState.Move)
                        {
                            //      Debug.Log("test4 move");
                            anim2.SetBool("run", true);
                            float roty = pvpenemy.Playerrot;
                            //    Debug.Log(roty);
                            remotetrans.eulerAngles = new Vector3(0, roty, 0);
                            //remotetrans.transform.Translate(Vector3.forward * Time.deltaTime * 10f);
                            //   Debug.Log("test4 remotetrans"+roty);

                            var destinationpos_flat = pvpenemy.Destinationpos.Value;
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
                        //멈춘 상태이면
                        else if (pvpenemy.Movestate == MoveState.Stop)
                        {
                            var enemypos_flat = pvpenemy.Playerpos.Value;
                            playerpos = new Vector3(enemypos_flat.X, enemypos_flat.Y, enemypos_flat.Z);
                            //멈추자 마자 공격을 아직 안했을때
                           
                            if (pvpenemy.Fire == 0)
                            {
                                // Debug.Log("test4 stop");
                                anim2.SetBool("run", false);

                                float roty = pvpenemy.Playerrot;
                                remotetrans.eulerAngles = new Vector3(0, roty, 0);


                                Debug.Log("playerposx : " + enemypos_flat.X + " , playerposy : " + enemypos_flat.Y + " , playerposz : " + enemypos_flat.Z);
                                // Debug.Log("stop_playerpos" + playerpos);
                                // Debug.Log("stop_playerpos_tr" + remotetrans.transform.position);
                                // remotetrans.transform.position = playerpos;
                                // remotetrans.transform.position = Vector3.MoveTowards(remotetrans.transform.position, playerpos, 10f * Time.deltaTime);
                                //     Debug.Log("playerposx : " + remotetrans.transform.position.x + " , playerposy : " + remotetrans.transform.position.y + " , playerposz : " + remotetrans.transform.position.z);
                                remotetrans.transform.position = Vector3.MoveTowards(remotetrans.transform.position, playerpos, 10f * Time.deltaTime);

                                if (pvpenemy.Skillfire == 1)
                                {

                                    Vector3 vec = target.position - remotetrans.transform.position;
                                    vec.Normalize();
                                    Debug.Log("target.localPosition:" + target.localPosition);

                                    //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                                    Quaternion q = Quaternion.LookRotation(vec);
                                    remotetrans.transform.rotation = q;

                                    //총알을 firepos의 위치에서 적이 있는 방향으로 복제한다


                                    string skillname = "skill" + enemyinfo.enemyskill1id.ToString();

                                    skill1 = Resources.Load<GameObject>("Skill/" + skillname);

                                    skillstate = 1;//스킬 사용중

                                    skill1real = Instantiate(skill1, enemygunpos.transform.position, enemygunpos.transform.rotation);

                                    skill1real.transform.SetParent(enemygunpos.transform);

                                    StartCoroutine(delay());
                                }


                            }//공격을 했을 때
                            else if (pvpenemy.Fire == 1)
                            {
                                if (skillstate == 0)
                                {
                                    // anim2.SetBool("run", false);
                                    //remotetrans.eulerAngles = new Vector3(0, pvpenemy.Playerrot, 0);
                                    remotetrans.transform.position = Vector3.MoveTowards(remotetrans.transform.position, playerpos, 10f * Time.deltaTime);

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
                                        Bullet = Resources.Load<GameObject>("Enemybullet");

                                        if (pvpenemy.Gunid ==0 || pvpenemy.Gunid == 1) {

                                            Quaternion bullettest = Quaternion.Euler(90, remotetrans.transform.eulerAngles.y, 0);
                                            Instantiate(Bullet, enemygunpos.transform.position, bullettest);
                                            // Instantiate(Bullet, enemygunpos.transform.position, enemygunpos.transform.rotation);

                                        }


                                    }
                                }
                            }


                           



                        }
                        // Debug.Log("test5 error");
                    }

                }
                else if(gametable == 1)
                    {
                    Control_flat.serverMessgae2 = null;
                    item = (Item)gameenemy.Item;
                    var itempos = item.Itempos.Value;
                    Vector3 hppos = new Vector3(itempos.X, itempos.Y, itempos.Z);
                    GameObject test = Instantiate(hprestore, hppos, Quaternion.identity);


                }
                else if (gametable == 2)
                {
                    Control_flat.serverMessgae2 = null;
                    effect =(Mapeffect) gameenemy.Mapeffect;
                    int[] selectmappart = new int[5];

                    selectmappart[0] = effect.Effectnum1;
                    selectmappart[1] = effect.Effectnum2;
                    selectmappart[2] = effect.Effectnum3;
                    selectmappart[3] = effect.Effectnum4;
                    selectmappart[4] = effect.Effectnum5;

                    StartCoroutine(mapfire(selectmappart));
                }






            }
        }
        catch (Exception e)
        {
           Debug.Log(e);
        }



    }

    IEnumerator mapfire(int[] selectmappart)
    {

        fire = Resources.Load<GameObject>("Plasma");
        List<GameObject> plasma = new List<GameObject>();

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                int number = selectmappart[i];
                GameObject selectpart = floorlist[number];
                selectpart.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < 5; i++)
            {
                int number = selectmappart[i];
                GameObject selectpart = floorlist[number];
                selectpart.GetComponent<MeshRenderer>().material.color = new Color(156 / 255f, 90 / 255f, 54 / 255f);
            }
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i < 5; i++)
        {
            int number = selectmappart[i];
            GameObject selectpart = floorlist[number];
            plasma.Add(Instantiate(fire, selectpart.GetComponent<Transform>().position, Quaternion.identity));

        }

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {
            Destroy(plasma[i]);

        }

    }


    IEnumerator delay() {

        yield return new WaitForSeconds(1f);
        Destroy(skill1real);
        skillstate = 0;
        Playerhpbar.skillatkedcount = 0;
    }



}
