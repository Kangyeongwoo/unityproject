using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleJSON;
using System.Text;
using UnityEngine.SceneManagement;
using Bossdata;
using FlatBuffers;

public class findteam : MonoBehaviour
{

    public GameObject user1slot;
    public GameObject user2slot;
    public GameObject user3slot;
    public GameObject user4slot;


    public GameObject playerborn;

    //찾는중
    public Text finding;

    public GameObject tcpclosebt;


    public static TcpClient socketConnection;


    public Thread thread;

    public static ByteBuffer serverMessage;

    public static List<ByteBuffer> serverMessagelist;

    public Coroutine Coroutine_receive;

    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;
    public static NetworkStream stream;

    public GameObject findteam_bt;

    public void findteambt() {

        serverMessagelist = new List<ByteBuffer>();

        findteam_bt = GameObject.Find("findteam");
        findteam_bt.SetActive(false);

        var bossstart = GameObject.Find("bossreadystart_sc").GetComponent<bossreadystart>();
        user1slot = bossstart.user1slot;
        user2slot = bossstart.user2slot;
        user3slot = bossstart.user3slot;
        user4slot = bossstart.user4slot;
        tcpclosebt = bossstart.tcpclosebt;
        finding = bossstart.finding;
        playerborn = bossstart.playerborn;


        finding.enabled = true;

        tcpclosebt.SetActive(true);
       // playerborn.SetActive(false);

        //내 정보를 서버에 보낸다
        socketConnection = new TcpClient("49.247.131.90", 4345);
        //tcp 통신으로 내가 1번이면 
        //데이터 직렬화를 위한 객체생성 (id, nickname, userindex, level, power,hp )
        builder = new FlatBufferBuilder(1024);
        var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
        var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));
        Player.StartPlayer(builder);
        Player.AddStartstate(builder, PlayerStart.Match);
        Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
        Player.AddId(builder, idoffset);
        Player.AddNickname(builder, nicknameoffset);
        Player.AddLevel(builder, PlayerPrefs.GetInt("level"));
  

        string totalpower_pp = PlayerPrefs.GetString("totalpower");
        // int totalpower = int.Parse(totalpower_pp);       
        int totalpower;
        int.TryParse(totalpower_pp, out totalpower);

        string totalhp_pp = PlayerPrefs.GetString("totalhp");
        //  int totalhp = int.Parse(totalhp_pp);   
        int totalhp;
        int.TryParse(totalhp_pp, out totalhp);

        string gunid_pp = PlayerPrefs.GetString("gunid");
        // int gunid = int.Parse(gunid_pp);   
        int gunid;
        int.TryParse(gunid_pp, out gunid);

        string armorid_pp = PlayerPrefs.GetString("armorid");
        //  int armorid = int.Parse(armorid_pp);   
        int armorid;
        int.TryParse(armorid_pp, out armorid);

        string skill1id_pp = PlayerPrefs.GetString("skill1id");
        //  int skill1id = int.Parse(skill1id_pp);   
        int skill1id;
        int.TryParse(skill1id_pp, out skill1id);

        string skill2id_pp = PlayerPrefs.GetString("skill2id");
        // int skill2id = int.Parse(skill2id_pp);   
        int skill2id;
        int.TryParse(skill2id_pp, out skill2id);

        Player.AddPower(builder, totalpower);
        Player.AddHp(builder, totalhp);
        Player.AddGunid(builder, gunid);
        Player.AddArmorid(builder, armorid);
        Player.AddSkill1id(builder, skill1id);
        Player.AddSkill2id(builder, skill2id);

        Offset<Player> pypplayer = Player.EndPlayer(builder);


        Game.StartGame(builder);
        Game.AddPlayer(builder, pypplayer);
        Game.AddTablenum(builder, 0);
        Offset<Game> game = Game.EndGame(builder);

        builder.Finish(game.Value);
        sendBuffer = builder.SizedByteArray();
        sendbb = new ByteBuffer(sendBuffer);


        //소켓 연결
        stream = socketConnection.GetStream();
        if (stream.CanWrite)
        {
            stream.Write(sendBuffer, 0, sendBuffer.Length);
            stream.Flush();
        }


        Debug.Log("Client send 3");

        //데이터 받는 쓰레드 시작serverMessage2
        thread = new Thread(Receiving);
        thread.Start();

       // findbutton.gameObject.SetActive(false);
       // finding.enabled = true;
        tcpclosebt.gameObject.SetActive(true);

        //데이터 보내는 코루틴 시작
        Time.timeScale = 1f;

        Coroutine_receive = StartCoroutine(receive());

        Debug.Log("Client send 4");


    }


    static void Receiving()
    {
        //쓰레드를 실행
        // Receiving
        int count = 0;
        Byte[] bytes = new byte[1024];


        //반복해서 서버에서 데이터를 받고 스테틱 변수에 반영
        int length;

        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            //받은 데이터를 변수에 저장 bytebuffer
            var incomingData = new byte[length];
            Array.Copy(bytes, 0, incomingData, 0, length);
            serverMessage = new ByteBuffer(incomingData);
            Debug.Log("Client received2: "+ serverMessage);
            serverMessagelist.Add(serverMessage);
            count += 1;
            Debug.Log("Client count: " + count);
        }


    }


    IEnumerator receive()
    {
       

        while (true)
        {

           
            if (serverMessage == null)
            {
                Debug.Log("Client null ");

            }
            else
            {
                Debug.Log("serverMessagelistsize:  " + serverMessagelist.Count);
                if (serverMessagelist.Count == 1)
                {
                  
                }
                else if (serverMessagelist.Count == 2)
                {

                }
                else if (serverMessagelist.Count == 3)
                {
                    tcpclosebt.SetActive(false);
                    playerborn.SetActive(false);

                    for (int i = 0; i < 3; i++)
                    {
                        Game gameplayer = Game.GetRootAsGame(serverMessagelist[i]);
                        Player player = (Player)gameplayer.Player;
                        Debug.Log("player.Roomindex:" + player.Roomindex);
                        if (player.Roomindex == 0) {

                            user1slot.SetActive(true);
                            user1slot.transform.GetChild(2).GetComponent<Text>().text = player.Nickname;

                            if (player.Skill1id != 0)
                            {
                                string imagename = "skill" + player.Skill1id;
                                Texture2D skill1texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                                user1slot.transform.GetChild(0).GetComponent<RawImage>().texture = skill1texture;
                            }
                            else {

                                user1slot.transform.GetChild(0).GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);
                          
                            }


                            if(player.Skill2id != 0) {

                                string imagename = "ability" + player.Skill2id;
                                Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                                user1slot.transform.GetChild(1).GetComponent<RawImage>().texture = skill2texture;
                            }
                            else
                            {
                                user1slot.transform.GetChild(1).GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

                            }


                        }
                        else if (player.Roomindex == 1) {

                            user2slot.SetActive(true);
                            user2slot.transform.GetChild(2).GetComponent<Text>().text = player.Nickname;

                            if (player.Skill1id != 0)
                            {
                                string imagename = "skill" + player.Skill1id;
                                Texture2D skill1texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                                user2slot.transform.GetChild(0).GetComponent<RawImage>().texture = skill1texture;
                            }
                            else
                            {

                                user2slot.transform.GetChild(0).GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

                            }


                            if (player.Skill2id != 0)
                            {

                                string imagename = "ability" + player.Skill2id;
                                Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                                user2slot.transform.GetChild(1).GetComponent<RawImage>().texture = skill2texture;
                            }
                            else
                            {
                                user2slot.transform.GetChild(1).GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

                            }

                        }
                        else if (player.Roomindex == 2) {

                            user3slot.SetActive(true);
                            user3slot.transform.GetChild(2).GetComponent<Text>().text = player.Nickname;


                            if (player.Skill1id != 0)
                            {
                                string imagename = "skill" + player.Skill1id;
                                Texture2D skill1texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                                user3slot.transform.GetChild(0).GetComponent<RawImage>().texture = skill1texture;
                            }
                            else
                            {

                                user3slot.transform.GetChild(0).GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

                            }


                            if (player.Skill2id != 0)
                            {

                                string imagename = "ability" + player.Skill2id;
                                Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                                user3slot.transform.GetChild(1).GetComponent<RawImage>().texture = skill2texture;
                            }
                            else
                            {
                                user3slot.transform.GetChild(1).GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

                            }

                        }
                    }

                    Debug.Log("세명 다옴");
                    StopCoroutine(Coroutine_receive);
                    //받는 쓰레드 종료
                    thread.Abort();

                    StartCoroutine(Countdown());
                }





                }

           // Debug.Log("Client null2 ");
            yield return new WaitForSeconds(0.2f);
           // Debug.Log("Client null3 ");
        }






    }

    private IEnumerator Countdown()
    {
        //시간을 표시하는 함수
        float duration = 0; // 3 seconds you can change this to
                            //to whatever you want
        float totalTime = 4f;
        while (totalTime >= duration)
        {
            //매초 시간이 4에서 1씩 줄어든.
            totalTime -= Time.deltaTime;
            var integer = (int)totalTime; /* choose how to quantize this */
            //0이 되면 Go로 바뀜
            if (integer.ToString().Equals("0"))
            {
              //  readycount.text = "Go";
            }
            else
            {
              //  readycount.text = integer.ToString();
            }
            /* convert integer to string and assign to text */
            yield return null;
        }

        SceneManager.LoadScene("bossscenestart");
    }

    public void tcpclose()
    {
        Debug.Log("tcpclose");
        StopCoroutine(Coroutine_receive);

        string str = "close";
        byte[] closebyte = Encoding.UTF8.GetBytes(str);


        if (stream.CanWrite)
        {
            stream.Write(closebyte, 0, closebyte.Length);
            stream.Flush();
        }


        // findbutton.gameObject.SetActive(true);
        //  finding.enabled = false;
        thread.Abort();
        tcpclosebt.gameObject.SetActive(false);
        stream.Close();
        socketConnection.Close();
       

        //  serverMessage = null;
        serverMessagelist.Clear();
    }



}
