using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Bossdata;
using FlatBuffers;


public class bossrespawn : MonoBehaviour
{
    GameObject monster;
    GameObject monsterborn;
    public Transform player;

    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;

    // Start is called before the first frame update
    void Start()
    {
        var playerindex = GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>();
        if (playerindex.playerindex==0)
        {
            StartCoroutine(raidboss());
        }
    }

    IEnumerator raidboss() {


        yield return new WaitForSeconds(2f);
        builder = new FlatBufferBuilder(1024);

        Monster.StartMonster(builder);
        Monster.AddMonsterstate(builder, 1);
        Monster.AddPower(builder,0);
        Monster.AddHp(builder, 0);
        Monster.AddMonsterpos(builder, Vec3.CreateVec3(builder, 0, 0, 0));
        Monster.AddMonsterrot(builder, 0);
        Offset<Monster> raidbossmonster = Monster.EndMonster(builder);

        var monsters = new Offset<Monster>[1];
        monsters[0] = raidbossmonster;
        var monstersOffset = Game.CreateMonsterVector(builder, monsters);

        Game.StartGame(builder);
        Game.AddMonster(builder, monstersOffset);
        Game.AddTablenum(builder,1);
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

        GameObject.Find("bossstart_cs").GetComponent<bossstart>().bossmonsterhpfac.SetActive(true);

        monster = Resources.Load<GameObject>("Bossraid/raidbossfac");

        monsterborn = Instantiate(monster, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }
}
