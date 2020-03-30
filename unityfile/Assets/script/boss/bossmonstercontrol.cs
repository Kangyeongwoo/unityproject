using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Net.Sockets;
using System.Threading;
using Bossdata;
using FlatBuffers;

public class bossmonstercontrol : MonoBehaviour
{

    public ByteBuffer receivemessage3;
    public ByteBuffer remotemonsterbyte;
    public Vector3 destinationpos;
    public Animator anim2;
    public Vector3 playerpos;

    public GameObject monster;
    public GameObject monsterborn;
    public int playerindex;

    public GameObject Bullet;
    public GameObject gunpos;
    public GameObject gunpos2;

    public int monsterhp;

    //몬스터 체력바에 나타나는 체력
    public Text monhp_text;

    //몬스터의 체력바 
    public Slider monhpbar;

    public int monstermaxhp;

    public int rewardid;

    // Start is called before the first frame update
    void Start()
    {
        playerindex = GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>().playerindex;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossplayercontrol.serverMessgae2 != null)
        {

            Game gameremoteplayer = new Game();
            Monster raidmonster;

            if (bossplayercontrol.receivequeue_mon.Count != 0)
            {
                remotemonsterbyte = bossplayercontrol.receivequeue_mon.Dequeue();
                gameremoteplayer = Game.GetRootAsGame(remotemonsterbyte);
                raidmonster = (Monster)gameremoteplayer.Monster(0);

                if (raidmonster.Monsterstate == 1) {

                    GameObject.Find("bossstart_cs").GetComponent<bossstart>().bossmonsterhpfac.SetActive(true);

                    monster = Resources.Load<GameObject>("Bossraid/raidbossfac");

                    monsterborn = Instantiate(monster, new Vector3(0f, 0f, 0f), Quaternion.identity);


                    if(playerindex == 1 || playerindex == 2) {
                        monsterborn.transform.Find("StoneMonster").gameObject.GetComponent<raidmonsterai>().enabled = false;

                    }

                }
                else
                {

                    if (raidmonster.Attacked == 1)
                    {
                        monsterhp = raidmonster.Currenthp;

                        Debug.Log("monsterhp:"+ monsterhp);
                        if (monsterhp <= 0)
                        {
                            monsterhp = 0;
                        }
                        string hp = monsterhp.ToString();
                        monhp_text.text = hp;
                        GameObject.FindWithTag("enemy").GetComponent<singlemonsterinfo>().monsterhp = monsterhp;
                        monstermaxhp = GameObject.FindWithTag("enemy").GetComponent<singlemonsterinfo>().monstermaxhp;
                        monhpbar.value = (float)monsterhp / (float)monstermaxhp;

                        if (monsterhp == 0) {

                            Destroy(monsterborn);

                            GameObject deadpanel = GameObject.Find("bossstart_cs").GetComponent<bossstart>().deadpanel;
                            deadpanel.SetActive(false);

                            GameObject winpanel = GameObject.Find("bossstart_cs").GetComponent<bossstart>().winpanel;
                            winpanel.SetActive(true);

                            int ab_1 = Random.Range(3, 6);

                            if (ab_1%2==0) {

                                string itemname = "armor" + ab_1;
                                rewardid = ab_1;
                                Texture2D itemtexture2 = Resources.Load<Texture2D>("Itemimage/" + itemname);
                                winpanel.transform.GetChild(1).GetComponent<RawImage>().texture = itemtexture2;
                                if (ab_1 == 4) {
                                    winpanel.transform.GetChild(0).GetComponent<Text>().text = "강철 장갑";
                                }else if (ab_1 == 6) {
                                    winpanel.transform.GetChild(0).GetComponent<Text>().text = "합금장갑";
                                }
                                Time.timeScale = 0;
                                bossplayercontrol.serverMessgae2 = null;
                            }
                            else {
                                string itemname = "gun" + ab_1;
                                rewardid = ab_1;
                                Texture2D itemtexture2 = Resources.Load<Texture2D>("Itemimage/" + itemname);
                                winpanel.transform.GetChild(1).GetComponent<RawImage>().texture = itemtexture2;
                                if (ab_1 == 3)
                                {
                                    winpanel.transform.GetChild(0).GetComponent<Text>().text = "헤비 머신건";
                                }
                                else if (ab_1 == 5)
                                {
                                    winpanel.transform.GetChild(0).GetComponent<Text>().text = "스나이핑건";
                                }
                                Time.timeScale = 0;
                                bossplayercontrol.serverMessgae2 = null;
                            }




                        }

                    }
                    else
                    {







                        //몬스터가 생성된 이후 행동
                        if (raidmonster.Movestate == MoveState.Move)
                        {
                            //이동 하게 되면 저 방향으로 이동
                            receivemessage3 = remotemonsterbyte;
                            var monster_destination = raidmonster.Monsterpos.Value;
                            float roty = raidmonster.Monsterrot;
                            monsterborn.transform.eulerAngles = new Vector3(0, roty, 0);
                            Vector3 destination = new Vector3(monster_destination.X, monster_destination.Y, monster_destination.Z);
                            monsterborn.transform.position = Vector3.MoveTowards(monsterborn.transform.position, destination, 20f * Time.deltaTime);

                        }
                        else
                        {
                            gunpos2 = monsterborn.transform.GetChild(0).gameObject;
                            gunpos = gunpos2.transform.GetChild(2).gameObject;
                            //이동이 아니면 공격
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
                          

                        }
                    }

                }



            }
            else
            {
                //데이터가 다 빠졌다 .. 이동에서 아직 공격이 안왔다 이땐 이동 데이터를 가지고 이동을 계속한다
                if (receivemessage3 != null) {

                    gameremoteplayer = Game.GetRootAsGame(receivemessage3);
                    raidmonster = (Monster)gameremoteplayer.Monster(0);
                    var monster_destination = raidmonster.Monsterpos.Value;
                    float roty = raidmonster.Monsterrot;
                    monsterborn.transform.eulerAngles = new Vector3(0, roty, 0);
                    Vector3 destination = new Vector3(monster_destination.X, monster_destination.Y, monster_destination.Z);
                    monsterborn.transform.position = Vector3.MoveTowards(monsterborn.transform.position, destination, 20f * Time.deltaTime);


                }


            }

        }
       
   }
}
