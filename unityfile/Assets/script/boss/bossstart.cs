using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleJSON;
using System.Text;
using UnityEngine.SceneManagement;
using Bossdata;
using FlatBuffers;

public class bossstart : MonoBehaviour
{
    public GameObject joystick;
    public GameObject remotecontrol;
    public GameObject bossplayer = null;
    public GameObject bossplayerborn = null;
    public GameObject bossremoteplayer = null;
    public GameObject bossremoteplayerborn = null;
    public GameObject enemy = null;
    public Transform maincamera;
    public static int enemyindex;
    public string result;
    public static Hashtable remoteplayerlist;
    public GameObject bossmonsterhpfac;
    public GameObject Resultpanel;
    public GameObject deadpanel;
    public GameObject winpanel;

    public static Hashtable deadtable;
    public static Hashtable abilitytable;


    private void Awake()
    {
        Resultpanel = GameObject.Find("Resultpanel");
        deadpanel= GameObject.Find("deadpanel");
        winpanel= GameObject.Find("winpanel");
        bossmonsterhpfac = GameObject.Find("bossmonsterhpfac");
        Resultpanel.SetActive(false);
        deadpanel.SetActive(false);
        winpanel.SetActive(false);
        bossmonsterhpfac.SetActive(false);

        List<ByteBuffer> serverMessagelist = findteam.serverMessagelist;
        remoteplayerlist = new Hashtable();
        deadtable = new Hashtable();
        abilitytable = new Hashtable();
        //플레이어1위치방향
        //      Quaternion player1rot = Quaternion.Euler(0, 180, 0);
        //      Vector3 player1pos = new Vector3(0, 1, 25);
        Quaternion player1rot = Quaternion.Euler(0, 0, 0);
        Vector3 player1pos = new Vector3(0, 1, -30);


        Quaternion player1camerarot = Quaternion.Euler(85, 0, 0);
        Vector3 player1camerapos = new Vector3(0, 130, -30);

        //플레이어2위치방향
        Quaternion player2rot = Quaternion.Euler(0, 0, 0);
        Vector3 player2pos = new Vector3(12, 1, -30);

        Quaternion player2camerarot = Quaternion.Euler(85, 0, 0);
        Vector3 player2camerapos = new Vector3(12, 130, -30);

        //플레이어3위치방향
        Quaternion player3rot = Quaternion.Euler(0, 0, 0);
        Vector3 player3pos = new Vector3(-12, 1, -30);

        Quaternion player3camerarot = Quaternion.Euler(85, 0, 0);
        Vector3 player3camerapos = new Vector3(-12, 130, -30);

        //카메라 지정
        maincamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
       
        for (int i = 0; i < 3; i++)
        {
            Game gameplayer = Game.GetRootAsGame(serverMessagelist[i]);
            Player player = (Player)gameplayer.Player;
            Debug.Log("1-player.Roomindex:" + player.Roomindex);
            List<int> abilitylist = new List<int>();


            if (player.Nickname.Equals(PlayerPrefs.GetString("nickname")))
            {
                //자기플레이어 생성
                bossplayer = Resources.Load<GameObject>("Bossraid/bossplayer");

                if (player.Roomindex == 0)
                {
                    bossplayerborn = Instantiate(bossplayer, player1pos, player1rot);
                    maincamera.position = player1camerapos;
                    maincamera.rotation = player1camerarot;

                }
                else if (player.Roomindex == 1)
                {
                    bossplayerborn = Instantiate(bossplayer, player2pos, player2rot);
                    maincamera.position = player2camerapos;
                    maincamera.rotation = player2camerarot;

                }
                else if (player.Roomindex == 2)
                {
                    bossplayerborn = Instantiate(bossplayer, player3pos, player3rot);
                    maincamera.position = player3camerapos;
                    maincamera.rotation = player3camerarot;
                }

                var playerbodycom = bossplayerborn.transform.Find("bossplayer").GetComponent<bossplayerinfo>();
                playerbodycom.playermaxhp = player.Hp;
                playerbodycom.playerhp = player.Hp;
                playerbodycom.playerpower = player.Power;
                playerbodycom.playerindex = player.Roomindex;
                playerbodycom.playergunid = player.Gunid;
                playerbodycom.playerarmorid = player.Armorid;
                playerbodycom.playerskill1id = player.Skill1id;
                playerbodycom.playerskill2id = player.Skill2id;
                playerbodycom.playerskillatk = player.Power*4;

                remoteplayerlist.Add(player.Roomindex, bossplayerborn);

                //장비를 장착시킴
                if (player.Gunid != 0)
                {

                    GameObject weaponhand = GameObject.FindWithTag("weaponhand");
                    //캐릭터가 장비하고 있는 총의 이름을 만듦
                    string gunname = "gun" + player.Gunid;

                    //총 오브젝트를 만든다
                    GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
                    GameObject handgun = Instantiate(handgun_ob);


                    Vector3 gunpos = new Vector3(0, 0, 0);
                    Quaternion gunrot = Quaternion.Euler(0, 90, -90);
                    Vector3 gunscale = new Vector3(3, 1, 0.5f);

                    //총을 손의 하위로 만든다
                    handgun.transform.SetParent(weaponhand.transform);
                    //총의 스케일 , 위치 , 방향을 지정해줌
                    handgun.transform.localScale = gunscale;
                    handgun.transform.localPosition = gunpos;
                    handgun.transform.localRotation = gunrot;

                    //총마다 달린 능력을 적용 스테이지 1에서만 적용한다 나머지 스테이지는 스테틱이라 적용된 상태
                   
                    if (player.Gunid == 1)
                        {
                        // 내 커런트 능력에 ADD 해야된다
                        //  currentability.abilitylist.Add(3);
                        abilitylist.Add(3);
                        }
                       
                }
                if (player.Skill1id != 0)
                {

                    string imagename = "skill" + player.Skill1id;
                    GameObject skillbt = GameObject.Find("skillbt");
                    Image skill1image = skillbt.GetComponent<Image>();
                    Sprite skill1texture = Resources.Load<Sprite>("Skillimage/" + imagename);
                    skill1image.sprite = skill1texture;

                }
                else
                {

                    GameObject skillbt = GameObject.Find("skillbt");
                    Image skill1image = skillbt.GetComponent<Image>();
                    skill1image.color = new Color(159 / 255f, 154 / 255f, 151 / 255f);
                    skillbt.GetComponent<Button>().interactable = false;
                }

                if (player.Skill2id != 0)
                {
                    // 패시브 스킬 있으면 커런트 능력에 add
                    abilitylist.Add(player.Skill2id);
                }
                else
                {
                      // 패시브 스킬이 없으면 그냥 둔다
                }

                abilitytable.Add(player.Roomindex, abilitylist);

            }
            else
            {
                //플레이어 생성
                bossremoteplayer = Resources.Load<GameObject>("Bossraid/remoteplayer");
              

                if (player.Roomindex == 0)
                {
                    bossremoteplayerborn = Instantiate(bossremoteplayer, player1pos, player1rot);

                }
                else if (player.Roomindex == 1)
                {
                    bossremoteplayerborn = Instantiate(bossremoteplayer, player2pos, player2rot);
                }
                else if (player.Roomindex == 2)
                {
                    bossremoteplayerborn = Instantiate(bossremoteplayer, player3pos, player3rot);
                }

                var remoteplayerbodycom = bossremoteplayerborn.transform.Find("remoteplayer").GetComponent<bossremoteplayerinfo>();
                remoteplayerbodycom.remoteplayermaxhp = player.Hp;
                remoteplayerbodycom.remoteplayerhp = player.Hp;
                remoteplayerbodycom.remoteplayerpower = player.Power;
                remoteplayerbodycom.remoteplayerindex = player.Roomindex;
                remoteplayerbodycom.remoteplayergunid = player.Gunid;
                remoteplayerbodycom.remoteplayerarmorid = player.Armorid;
                remoteplayerbodycom.remoteplayerskill1id = player.Skill1id;
                remoteplayerbodycom.remoteplayerskill2id = player.Skill2id;


                remoteplayerlist.Add(player.Roomindex, bossremoteplayerborn);

                Debug.Log("2-playerroomindex:"+ player.Roomindex);


                //장비를 장착시킴
                if (player.Gunid != 0)
                {
                    GameObject test0 = bossremoteplayerborn.transform.GetChild(0).gameObject; //remoteplayer
                    GameObject test1 = test0.transform.GetChild(0).gameObject; //ironman
                    GameObject test2 = test1.transform.GetChild(0).gameObject; //root
                    GameObject test3 = test2.transform.GetChild(0).gameObject; //hips
                    GameObject test4 = test3.transform.GetChild(2).gameObject; //spine
                    GameObject test5 = test4.transform.GetChild(0).gameObject;//spine1
                    GameObject test6 = test5.transform.GetChild(2).gameObject;//rightshol
                    GameObject test7 = test6.transform.GetChild(0).gameObject;//rightarm
                    GameObject test8 = test7.transform.GetChild(0).gameObject;//armroll
                    GameObject test9 = test8.transform.GetChild(0).gameObject;//forearm
                    GameObject test10 = test9.transform.GetChild(0).gameObject;//forearmroll
                    GameObject test11 = test10.transform.GetChild(0).gameObject;//righthand
                    GameObject weaponhand = test11.transform.GetChild(5).gameObject;//weaphn
                  
                    string gunname = "gun" + player.Gunid;

                    //총 오브젝트를 만든다
                    GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
                    GameObject handgun = Instantiate(handgun_ob);


                    Vector3 gunpos = new Vector3(0, 0, 0);
                    Quaternion gunrot = Quaternion.Euler(0, 90, -90);
                    Vector3 gunscale = new Vector3(3, 1, 0.5f);

                    //총을 손의 하위로 만든다
                    handgun.transform.SetParent(weaponhand.transform);
                    //총의 스케일 , 위치 , 방향을 지정해줌
                    handgun.transform.localScale = gunscale;
                    handgun.transform.localPosition = gunpos;
                    handgun.transform.localRotation = gunrot;

                    /*
                    //  GameObject weaponhand = GameObject.FindWithTag("weaponhand");
                    GameObject weaponhand = bossremoteplayerborn.transform.Find("b_RightWeapon").gameObject;
                    //캐릭터가 장비하고 있는 총의 이름을 만듦
                    string gunname = "gun" + player.Gunid;

                    //총 오브젝트를 만든다
                    GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
                    GameObject handgun = Instantiate(handgun_ob);


                    Vector3 gunpos = new Vector3(0, 0, 0);
                    Quaternion gunrot = Quaternion.Euler(0, 90, -90);
                    Vector3 gunscale = new Vector3(3, 1, 0.5f);

                    //총을 손의 하위로 만든다
                    handgun.transform.SetParent(weaponhand.transform);
                    //총의 스케일 , 위치 , 방향을 지정해줌
                    handgun.transform.localScale = gunscale;
                    handgun.transform.localPosition = gunpos;
                    handgun.transform.localRotation = gunrot;

                    //총마다 달린 능력을 적용 스테이지 1에서만 적용한다 나머지 스테이지는 스테틱이라 적용된 상태
                     */
                    if (player.Gunid == 1)
                    {
                        // 내 커런트 능력에 ADD 해야된다
                        //  currentability.abilitylist.Add(3);
                        abilitylist.Add(3);

                    }
                   
                }
                if (player.Skill1id != 0)
                {

                }
                else
                {


                }

                if (player.Skill2id != 0)
                {
                    // 패시브 스킬 있으면 커런트 능력에 add
                    abilitylist.Add(player.Skill2id);
                }
                else
                {
                    // 패시브 스킬이 없으면 그냥 둔다
                }


                abilitytable.Add(player.Roomindex, abilitylist);


            }



        }

    }










}



