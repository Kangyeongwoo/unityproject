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


public class Findenemy : MonoBehaviour
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

    public static String receivedata;

    public Coroutine Coroutine_receive;

    public Button tcpclosebt;



    public class Data
    {   //데이터 직렬화
        public string type;
        public string userindex;
        public string id;
        public string nickname;
        public string level;
        public string power;
        public string hp;

        //  public float[] playerpos = new float[3];

    }



        public void findenemy() {
        //소켓 연결
        socketConnection = new TcpClient("49.247.131.90", 4343);


        //데이터 직렬화를 위한 객체생성
        Data mydata = new Data();

        //직렬화에 이름 추가
        mydata.type = "0";
        mydata.userindex = PlayerPrefs.GetString("userindex");
        mydata.id = PlayerPrefs.GetString("id");
        mydata.nickname = PlayerPrefs.GetString("nickname");
        mydata.level = PlayerPrefs.GetString("level");
        mydata.power = PlayerPrefs.GetString("power");
        mydata.hp = PlayerPrefs.GetString("hp");



        Debug.Log("soccket 1 ");

        //litjson으로 직렬화한 데이터를 json 형식 스트링으로 변환
        jsondata = JsonUtility.ToJson(mydata);
        Debug.Log("soccket 2 ");


        //소켓 연결
        stream = socketConnection.GetStream();
        if (stream.CanWrite)
        {
            //데이터 보내기
            string clientMessage = jsondata;
            byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
            stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
        }


        Debug.Log("Client send 3");

        //데이터 받는 쓰레드 시작
        thread = new Thread(Receiving);
        thread.Start();

        findbutton.gameObject.SetActive(false);
        finding.enabled = true;
        tcpclosebt.gameObject.SetActive(true);

        //데이터 보내는 코루틴 시작
        Coroutine_receive = StartCoroutine(receive());
       


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
                        var incomingData = new byte[length];
                        Array.Copy(bytes, 0, incomingData, 0, length);
                        serverMessgae = Encoding.ASCII.GetString(incomingData);
                        Debug.Log("Client received: " + serverMessgae);

                        receivedata = serverMessgae;
                        Debug.Log("Client received2: " + serverMessgae);
                    }
                



        
    }



    IEnumerator receive() {

        while (true)
        {

            if (String.IsNullOrEmpty(receivedata))
            {
                Debug.Log("Client null " + receivedata);

            }
            else
            {
                //받은 데이터로 상대 캐릭터 생성
                Debug.Log("Client notnull " + receivedata);
                var jsonPlayer = JSON.Parse(serverMessgae);
                string enemynickname_js = jsonPlayer["nickname"];

                enemynickname.text = enemynickname_js;

                enemypos = new Vector3(-103.7f, 219.25f, -81.6f);

                enemyrot = Quaternion.Euler(-90, 180, 0);

                enemy = Resources.Load<GameObject>("pvpready_enemy");

                finding.enabled = false;

                tcpclosebt.gameObject.SetActive(false);

                Instantiate(enemy, enemypos, enemyrot);

                //캐릭터 생성되면 카운트다운 시작
                StartCoroutine(Countdown());
                StopCoroutine(Coroutine_receive);
                thread.Abort();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }


    private IEnumerator Countdown()
    {
        //시간을 표시하는 함
        float duration = 0; // 3 seconds you can change this to
                             //to whatever you want
        float totalTime = 4f;
        while (totalTime >= duration)
        { 
            totalTime -= Time.deltaTime;
            var integer = (int)totalTime; /* choose how to quantize this */
            if (integer.ToString().Equals("0"))
            {
                readycount.text = "Go";
            }
            else {
                readycount.text = integer.ToString();
            }
                                 /* convert integer to string and assign to text */
            yield return null;
        }

        SceneManager.LoadScene("Pvpscene");
    }

    public void tcpclose() {
        Debug.Log("tcpclose");
        findbutton.gameObject.SetActive(true);
        finding.enabled = false;
        tcpclosebt.gameObject.SetActive(false);
        StopCoroutine(Coroutine_receive);
        socketConnection.Close();

      }
}
