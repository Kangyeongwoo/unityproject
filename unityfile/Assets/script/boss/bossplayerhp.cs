using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Bossdata;
using FlatBuffers;

public class bossplayerhp : MonoBehaviour
{
    //몬스터 체력바에 나타나는 체력
    public Text playerhp_text;

    //몬스터의 체력바 
    public Slider palyerhpbar;

    public int playerhp;
    public int playermaxhp;
    // public static NetworkStream stream = Findenemy_flat.stream;
    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;
    public static int skillatkedcount;


    void Start()
    {
        skillatkedcount = 0;
        var playerdata = GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>();
        playerhp = playerdata.playerhp;
        playermaxhp = playerdata.playermaxhp;
        playerhp_text.text = playerhp.ToString();
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnCollisionEnter 1 :" + other.gameObject.tag);
        // int playermaxhp = GameObject.FindWithTag("Player").GetComponent<Playerinfo>().playermaxhp; 


        //총알과 부딪힐때
        if (other.gameObject.tag == "enemybullet" || other.gameObject.tag == "enemybody")
        {
            Debug.Log("playerhpdown 1 :" + other.gameObject.tag);

            var enemy = GameObject.FindWithTag("enemy").GetComponent<singlemonsterinfo>();


            Debug.Log("enemy.monsterattack  :" + enemy.monsterattack);
            //몬스터의 체력은 플레이어 공격력 만큼 감소
            playerhp = playerhp - enemy.monsterattack;


            //맞은 데이터 전달 (id,nickname, 플레이 상태 ,유저인덱스 ,공격당한 상태, 현재 hp) 
            builder = new FlatBufferBuilder(1024);
            var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
            var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));

            Player.StartPlayer(builder);
            Player.AddStartstate(builder, PlayerStart.Play);
            Player.AddUserindex(builder, GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>().playerindex);
            Player.AddRoomindex(builder, GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>().playerindex);
            Player.AddId(builder, idoffset);
            Player.AddAttacked(builder, 1);
            Player.AddCurrenthp(builder, playerhp);
            Offset<Player> pypplayer = Player.EndPlayer(builder);
            //  builder.Finish(pypplayer.Value);
            //  sendBuffer = builder.SizedByteArray();
            //  sendbb = new ByteBuffer(sendBuffer);

            Game.StartGame(builder);
            Game.AddPlayer(builder, pypplayer);
            Game.AddTablenum(builder, 0);

            Offset<Game> game = Game.EndGame(builder);

            builder.Finish(game.Value);
            sendBuffer = builder.SizedByteArray();
            sendbb = new ByteBuffer(sendBuffer);

            //NetworkStream stream = socketConnection.GetStream();

            if (findteam.stream.CanWrite)
            {
                findteam.stream.Write(sendBuffer, 0, sendBuffer.Length);
                findteam.stream.Flush();
            }

            if (playerhp <= 0) {

                playerhp = 0;
            }

            string hp = playerhp.ToString();
            playerhp_text.text = hp;
            palyerhpbar.value = (float)playerhp / (float)playermaxhp;
            Debug.Log("player_hp: " + playerhp);

            if (playerhp == 0) {

                bossstart.remoteplayerlist.Remove(GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>().playerindex);
                int key = GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>().playerindex;
                Destroy(GameObject.Find("bossplayer(Clone)"));
                //Destroy((GameObject)bossstart.remoteplayerlist[key]);
                GameObject deadpanel = GameObject.Find("bossstart_cs").GetComponent<bossstart>().deadpanel;
                deadpanel.SetActive(true);
                if (!bossstart.deadtable.Contains(key))
                {
                    bossstart.deadtable.Add(key, "dead");
                }
            }

        }
    }

    }
