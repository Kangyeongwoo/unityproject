using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Bossdata;
using FlatBuffers;

public class bossmonsterhp : MonoBehaviour
{

    //몬스터 체력바에 나타나는 체력
    public Text monhp_text;

    //몬스터의 체력바 
    public Slider monhpbar;

    public int monsterhp;
    public int monstermaxhp;

    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;
    public static int skillatkedcount;


    // Start is called before the first frame update
    void Start()
    {
        monhp_text = GameObject.Find("enemyhptext").GetComponent<Text>();
        monhpbar = GameObject.Find("monsterhpbar").GetComponent<Slider>();
        var monsterdata = GameObject.FindWithTag("enemy").GetComponent<singlemonsterinfo>();
        monsterhp = monsterdata.monsterhp;
        monstermaxhp = monsterdata.monstermaxhp;
        monhp_text.text = monsterhp.ToString();
    }

    void OnTriggerEnter(Collider other)
    // void OnCollisionEnter(Collision other)
    {

        Debug.Log("colidertest1:"+other);
        //other는 부딪힌 오브젝트 벽 또는 적에 맞으면 총알이 사라진다
        if (other.gameObject.tag == "Playerbullet")
        {
            Debug.Log("colidertest2:" + other);
            var player = GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>();

            //몬스터의 체력은 플레이어 공격력 만큼 감소
            monsterhp = monsterhp - player.playerpower;

            if (monsterhp <= 0)
            {
                monsterhp = 0;
            }

            builder = new FlatBufferBuilder(1024);
            Monster.StartMonster(builder);
            Monster.AddMonsterstate(builder, 0);
            Monster.AddPower(builder, 0);
            Monster.AddHp(builder, 0);
            Monster.AddMonsterpos(builder, Vec3.CreateVec3(builder, 0, 0, 0));
            Monster.AddMonsterrot(builder, 0);
            Monster.AddAttacked(builder, 1);
            Monster.AddCurrenthp(builder, monsterhp);
            Offset<Monster> raidbossmonster = Monster.EndMonster(builder);

            var monsters = new Offset<Monster>[1];
            monsters[0] = raidbossmonster;
            var monstersOffset = Game.CreateMonsterVector(builder, monsters);


            Player.StartPlayer(builder);
            Player.AddStartstate(builder, PlayerStart.Play);
            Player.AddPower(builder, player.playerpower);
            Offset<Player> pypplayer = Player.EndPlayer(builder);



            Game.StartGame(builder);
            Game.AddPlayer(builder, pypplayer);
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

           


            string hp = monsterhp.ToString();
          //  monhp_text.text = hp;
          //  monhpbar.value = (float)monsterhp / (float)monstermaxhp;
            Debug.Log("player_hp: " + monsterhp);

        }

        if (other.gameObject.tag == "skill")
        {

            var player = GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>();

            //몬스터의 체력은 플레이어 공격력 만큼 감소
            monsterhp = monsterhp - player.playerskillatk;

            if (monsterhp <= 0)
            {
                monsterhp = 0;
            }


            builder = new FlatBufferBuilder(1024);
            Monster.StartMonster(builder);
            Monster.AddMonsterstate(builder, 0);
            Monster.AddPower(builder, 0);
            Monster.AddHp(builder, 0);
            Monster.AddMonsterpos(builder, Vec3.CreateVec3(builder, 0, 0, 0));
            Monster.AddMonsterrot(builder, 0);
            Monster.AddCurrenthp(builder, monsterhp);
            Offset<Monster> raidbossmonster = Monster.EndMonster(builder);

            var monsters = new Offset<Monster>[1];
            monsters[0] = raidbossmonster;
            var monstersOffset = Game.CreateMonsterVector(builder, monsters);

            Player.StartPlayer(builder);
            Player.AddStartstate(builder, PlayerStart.Play);
            Player.AddPower(builder, player.playerskillatk);
            Offset<Player> pypplayer = Player.EndPlayer(builder);

            Game.StartGame(builder);
            Game.AddPlayer(builder, pypplayer);
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

          
            string hp = monsterhp.ToString();
            //monhp_text.text = hp;
            //monhpbar.value = (float)monsterhp / (float)monstermaxhp;
            Debug.Log("player_hp: " + monsterhp);

        }
    }

    }
