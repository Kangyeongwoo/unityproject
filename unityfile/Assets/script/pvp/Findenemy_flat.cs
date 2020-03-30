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
using Pvpdata;
using FlatBuffers;

public class Findenemy_flat : MonoBehaviour
{
    public Socket clientSocket;

    public string id;

    public String jsondata;

    public Vector3 enemypos;

    public Quaternion enemyrot;

    public GameObject enemy = null;

    public Text enemynickname;

    public static TcpClient socketConnection;

    public static string serverMessgae = "";

    public static NetworkStream stream;

    public Text readycount;

    public Button findbutton;

    public Text finding;

    public Thread thread;

    public static ByteBuffer serverMessage2;

    public Coroutine Coroutine_receive;

    public Button tcpclosebt;

    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;

    public void findenemy()
    {
        //소켓 연결
        socketConnection = new TcpClient("49.247.131.90", 4343);


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
       // Player.AddPower(builder, PlayerPrefs.GetInt("power"));
       // Player.AddHp(builder, PlayerPrefs.GetInt("hp"));


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
        Player.AddSkill2id(builder,skill2id);

        Offset<Player> pypplayer = Player.EndPlayer(builder);
      

        Game.StartGame(builder);
        Game.AddPlayer(builder, pypplayer);
        Game.AddTablenum(builder,0);
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

        findbutton.gameObject.SetActive(false);
        finding.enabled = true;
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

        Byte[] bytes = new byte[1024];


        //반복해서 서버에서 데이터를 받고 스테틱 변수에 반영
        int length;

        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            //받은 데이터를 변수에 저장 bytebuffer
            var incomingData = new byte[length];
            Array.Copy(bytes, 0, incomingData, 0, length);
            serverMessage2 = new ByteBuffer(incomingData);
            Debug.Log("Client received2: " );
        }


    }


    IEnumerator receive()
    {
       
            while (true)
            {
          
                Debug.Log("Client  " + serverMessage2);
                if (serverMessage2 == null)
                {
                    Debug.Log("Client null ");

                }
                else
                {
                    //받은 데이터가 있다면
                    Debug.Log("Client notnull ");

                //버퍼객체 생성
                Game gameenemy = Game.GetRootAsGame(serverMessage2);
                Player pvpenemy = (Player)gameenemy.Player;
               // Player pvpenemy = Player.GetRootAsPlayer(serverMessage2);

                    string enemynickname_fl = pvpenemy.Nickname;

                    Debug.Log("player" + pvpenemy.Id);
                    Debug.Log("player" + pvpenemy.Nickname);

                    enemynickname.text = enemynickname_fl;

                    enemypos = new Vector3(-103.7f, 219.25f, -81.6f);

                    enemyrot = Quaternion.Euler(-90, 180, 0);

                    enemy = Resources.Load<GameObject>("pvpready_enemy");

                    finding.enabled = false;

                    tcpclosebt.gameObject.SetActive(false);

                    Instantiate(enemy, enemypos, enemyrot);


                  int enemygunid_fb =  pvpenemy.Gunid; 
                  string enemygunid_pp = enemygunid_fb.ToString();
                  GameObject enemyweaponhand = GameObject.FindWithTag("enemyweaponhand");

                        if (!enemygunid_pp.Equals("0"))
                        {

                            //캐릭터가 장비하고 있는 총의 이름을 만듦
                            string gunname = "gun" + enemygunid_pp;

                            //총 오브젝트를 만든다
                            GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
                            GameObject handgun = Instantiate(handgun_ob);


                            Vector3 gunpos = new Vector3(0, 0, 0);
                            Quaternion gunrot = Quaternion.Euler(0, 90, -90);
                            Vector3 gunscale = new Vector3(3, 1, 1);

                            //총을 손의 하위로 만든다
                            handgun.transform.SetParent(enemyweaponhand.transform);
                            //총의 스케일 , 위치 , 방향을 지정해줌
                            handgun.transform.localScale = gunscale;
                            handgun.transform.localPosition = gunpos;
                            handgun.transform.localRotation = gunrot;
                        }                   
                                     



                //받는 코루틴 종료
                StopCoroutine(Coroutine_receive);
                    //받는 쓰레드 종료
                    thread.Abort();
                    //카운트다운 코루틴 시작
                    StartCoroutine(Countdown());

                }

            Debug.Log("Client null2 ");
            yield return new WaitForSeconds(0.2f);
            Debug.Log("Client null3 ");
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
                readycount.text = "Go";
            }
            else
            {
                readycount.text = integer.ToString();
            }
            /* convert integer to string and assign to text */
            yield return null;
        }

        SceneManager.LoadScene("Pvpscene");
    }

    public void tcpclose()
    {
        Debug.Log("tcpclose");
        findbutton.gameObject.SetActive(true);
        finding.enabled = false;
        thread.Abort();
        tcpclosebt.gameObject.SetActive(false);
        StopCoroutine(Coroutine_receive);
        socketConnection.Close();
        serverMessage2 = null;

    }







}
