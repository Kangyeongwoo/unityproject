using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Pvpdata;
using FlatBuffers;

public class Playerhpbar : MonoBehaviour
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

    private void Start()
    {
        skillatkedcount = 0;
        var playerdata = GameObject.FindWithTag("Player").GetComponent<Playerinfo>();
        playerhp = playerdata.playerhp;
        playermaxhp = playerdata.playermaxhp;
        playerhp_text.text = playerhp.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnCollisionEnter 1 :" + other.gameObject.tag);
       // int playermaxhp = GameObject.FindWithTag("Player").GetComponent<Playerinfo>().playermaxhp; 
       

        //총알과 부딪힐때
        if (other.gameObject.tag == "enemybullet")
        {
            Debug.Log("playerhpdown 1 :"+other.gameObject.tag);

            var enemy = GameObject.FindWithTag("enemy").GetComponent<Enemyinfo>();

            //몬스터의 체력은 플레이어 공격력 만큼 감소
            playerhp = playerhp - enemy.enemypower;


            //맞은 데이터 전달 (id,nickname, 플레이 상태 ,유저인덱스 ,공격당한 상태, 현재 hp) 
            builder = new FlatBufferBuilder(1024);
            var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
            var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));

            Player.StartPlayer(builder);
            Player.AddStartstate(builder, PlayerStart.Play);
            Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
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

            if (Findenemy_flat.stream.CanWrite)
            {
                Findenemy_flat.stream.Write(sendBuffer, 0, sendBuffer.Length);
                Findenemy_flat.stream.Flush();
            }


            string hp = playerhp.ToString();
            playerhp_text.text = hp;

            Debug.Log("player_hp: " + playerhp);

        }else if (other.gameObject.tag == "mapfire") {

            double mapatk = playermaxhp * 0.1;

            playerhp = playerhp - (int)mapatk;


            //맞은 데이터 전달 (id,nickname, 플레이 상태 ,유저인덱스 ,공격당한 상태, 현재 hp) 
            builder = new FlatBufferBuilder(1024);
            var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
            var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));

            Player.StartPlayer(builder);
            Player.AddStartstate(builder, PlayerStart.Play);
            Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
            Player.AddId(builder, idoffset);
            Player.AddAttacked(builder, 1);
            Player.AddCurrenthp(builder, playerhp);
            Offset<Player> pypplayer = Player.EndPlayer(builder);
           // builder.Finish(pypplayer.Value);
           // sendBuffer = builder.SizedByteArray();
           // sendbb = new ByteBuffer(sendBuffer);

            Game.StartGame(builder);
            Game.AddPlayer(builder, pypplayer);
            Game.AddTablenum(builder, 0);
            Offset<Game> game = Game.EndGame(builder);

            builder.Finish(game.Value);
            sendBuffer = builder.SizedByteArray();
            sendbb = new ByteBuffer(sendBuffer);

            //NetworkStream stream = socketConnection.GetStream();

            if (Findenemy_flat.stream.CanWrite)
            {
                Findenemy_flat.stream.Write(sendBuffer, 0, sendBuffer.Length);
                Findenemy_flat.stream.Flush();
            }


            string hp = playerhp.ToString();
            playerhp_text.text = hp;

        }else if (other.gameObject.tag == "hprestore") {

            double resotre = playermaxhp * 0.1;

            playerhp = playerhp + (int)resotre;
            Debug.Log("hprestore: "+ playerhp);


            if (playerhp >= playermaxhp) {

                playerhp = playermaxhp;


            }

            //맞은 데이터 전달 (id,nickname, 플레이 상태 ,유저인덱스 ,공격당한 상태, 현재 hp) 
            builder = new FlatBufferBuilder(1024);
            var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
            var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));

            Player.StartPlayer(builder);
            Player.AddStartstate(builder, PlayerStart.Play);
            Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
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

            if (Findenemy_flat.stream.CanWrite)
            {
                Findenemy_flat.stream.Write(sendBuffer, 0, sendBuffer.Length);
                Findenemy_flat.stream.Flush();
            }


            string hp = playerhp.ToString();
            playerhp_text.text = hp;



        }


        Debug.Log("playerhpdown 2 :" + other.gameObject.tag);
        if (playerhp <= 0)
        {

            //슬라이더 색이 어디 까지 차있는지 정할 수 있음
            playerhp = 0;
            string hp = playerhp.ToString();
            playerhp_text.text = hp;
            palyerhpbar.value = 0;
        }
        else
        {
            //분수로 표현 가능
            palyerhpbar.value = (float)playerhp / (float)playermaxhp;
        }

       
    }

    private void OnParticleCollision(GameObject other)
    {
        skillatkedcount += 1;

        Debug.Log("OnParticleCollision 1 :" + other.gameObject.tag);

        if (other.gameObject.tag == "skill" && skillatkedcount ==1)
        {
            Debug.Log("OnParticleCollision 2 :" + other.gameObject.tag);
            Debug.Log("playerhpdown 1 :" + other.gameObject.tag);

            var enemy = GameObject.FindWithTag("enemy").GetComponent<Enemyinfo>();

            //몬스터의 체력은 플레이어 공격력 만큼 감소
            playerhp = playerhp - enemy.enemyskillatk;


            //맞은 데이터 전달 (id,nickname, 플레이 상태 ,유저인덱스 ,공격당한 상태, 현재 hp) 
            builder = new FlatBufferBuilder(1024);
            var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
            var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));

            Player.StartPlayer(builder);
            Player.AddStartstate(builder, PlayerStart.Play);
            Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
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

            if (Findenemy_flat.stream.CanWrite)
            {
                Findenemy_flat.stream.Write(sendBuffer, 0, sendBuffer.Length);
                Findenemy_flat.stream.Flush();
            }


            string hp = playerhp.ToString();
            playerhp_text.text = hp;

            Debug.Log("player_hp: " + playerhp);


        }

        Debug.Log("playerhpdown 2 :" + other.gameObject.tag);
        if (playerhp <= 0)
        {

            //슬라이더 색이 어디 까지 차있는지 정할 수 있음
            playerhp = 0;
            string hp = playerhp.ToString();
            playerhp_text.text = hp;
            palyerhpbar.value = 0;
        }
        else
        {
            //분수로 표현 가능
            palyerhpbar.value = (float)playerhp / (float)playermaxhp;
        }
    }


}
