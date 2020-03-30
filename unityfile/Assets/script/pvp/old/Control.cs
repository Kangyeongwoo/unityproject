using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;



public class Data3
{   //데이터 직렬화
    public string type;
    public string userindex;
    public string id;
    public string nickname;
    public List<string> playerpos = new List<string>();
    public string playerrot;
    public string movestate;
    public List<string> destinationpos = new List<string>();
}


public class Control : MonoBehaviour
{

    // 공개
    //
    public GameObject joy;
    public Transform Stickback;
    public Transform Stick;         // 조이스틱.
    public Transform Player;
    public Camera camera2;

    // 비공개
    //조이스틱의 맨 처음 위치 
    private Vector3 Stick1;
    //조이스틱배경의 맨 처음 위치 
    private Vector3 Stickback1;
    //처음으로 이동한 위치 (처음 클릭한 위치)
    private Vector3 movefirst;


    private Vector3 StickFirstPos;  // 조이스틱의 처음 위치.
    private Vector3 JoyVec;         // 조이스틱의 벡터(방향)
    private float Radius;           // 조이스틱 배경의 반 지름.
    private bool MoveFlag;          // 움직이는지 아닌지 확인
    public PointerEventData Data;


    //플레이어 위치
    public Transform playerpos;
    //닉네임 인풋필드
    public InputField texts;
    //클라이언트 소켓
    static public Socket clientSocket;

    public static TcpClient socketConnection;

    public static string serverMessgae = "";

    //플레이어 데이터 제이슨으로 바꾼 뒤 스트링으로 바꾼 데이터
    public string jsondata;
    public Thread thread;

    public static string receivedata;

    float yrot;

    Coroutine co_my_coroutine;

    public static Queue receivequeue;

    public Animator anim;

    public void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //플레이어의 위치를 조이스틱의 방향으로 이동

        //조이스틱배경의 맨 처음 위치 
        Stickback1 = Stickback.transform.position;
        //조이스틱의 맨 처음 위치 
        Stick1 = Stick.transform.position;

        //반지름은 배경의 y값의 절반
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        Debug.Log("0 , start");
        Debug.Log("1 , Radius:" + Radius);

        //조이스틱의 원래 위치
        StickFirstPos = Stick.transform.position;
        StickFirstPos.z = 0;
        Debug.Log("2 , StickFirstPos:" + StickFirstPos);

        // 캔버스 크기에대한 반지름 조절.
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Debug.Log("3 , Can:" + Can);
        Radius *= Can;
        Debug.Log("4 , Radius:" + Radius);
        MoveFlag = false;

        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();

        socketConnection = Findenemy.socketConnection;

        receivequeue = new Queue();

        thread = new Thread(Receiving);
        thread.Start();

    }


    void Update()

    {

        //마우스를 누를때

        if (Input.GetMouseButtonDown(0))
        {
            MoveFlag = true;
            //뛰는 애니메이션 적용 파라미터는 animator에 있음
            anim.SetBool("run", true);

            //조이스틱 배경을 마우스가 클릭한 위치로 이동
            Stickback.transform.position = Input.mousePosition;
            Debug.Log("mouspoint" + Input.mousePosition);

            //조이스틱도 마우스가 클릭한 위치로 이동
            Stick.transform.position = Input.mousePosition; // 타겟을 레이캐스트가 충돌된 곳으로 옮긴다.
            movefirst = Stick.transform.position;
            //이동한 위치(클릭한위치)
           // if (!Mathf.Approximately(yrot, 0.0f))
          //  {
                co_my_coroutine = StartCoroutine(sending());
           // }




        }
        //마우스 누르고 있을때
        else if (Input.GetMouseButton(0))
        {
            MoveFlag = true;

            //스틱이 이동 후 초기 위치
            StickFirstPos = movefirst;
            Vector3 Pos = Input.mousePosition;
            Debug.Log("2 , Pos:" + Pos);
            Pos.z = 0;

            // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
            JoyVec = (Pos - StickFirstPos).normalized;
            Debug.Log("3 , JoyVec:" + JoyVec);

            // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
            float Dis = Vector3.Distance(Pos, StickFirstPos);
            Debug.Log("4 , Dis:" + Dis);

            // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는곳으로 이동. 
            if (Dis < Radius)
            {
                //조이스틱의 위치 = 처음위치에 정규화한 벡터 곱하기 거리 한걸 더한것
                Stick.position = StickFirstPos + JoyVec * Dis;
                Debug.Log("5 , Stick.position:" + Stick.position);
                // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
            }
            else
            {
                //거리가 반지름 보다 크면 반지름 까지만 올수 있게 한다
                Stick.position = StickFirstPos + JoyVec * Radius;
                Debug.Log("5 , Stick.position:" + Stick.position);
            }

            if (Pvpscenestart.enemyindex == "0") {
                Player.eulerAngles = new Vector3(0, Mathf.Atan2(-JoyVec.x, -JoyVec.y) * Mathf.Rad2Deg, 0);
                yrot = Mathf.Atan2(-JoyVec.x, -JoyVec.y) * Mathf.Rad2Deg;
            }
            else if (Pvpscenestart.enemyindex == "1") {
                Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
                yrot = Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg;
            }

            //오일러각 회전각   // x,y를 알고 있으니 아크탄젠트 버전2 각도를 구할 수 있다  //Mathf.Rad2Deg: 라디안을 각도로 변환해주는 함수


           
            //   Player.transform.Translate(Vector3.forward * Time.deltaTime * 10f);

            Debug.Log("mouspoint" + Input.mousePosition);
            // 타겟을 레이캐스트가 충돌된 곳으로 옮긴다.



        }


        else if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine(co_my_coroutine);

            anim.SetBool("run", false);
         
               DragEnd();
                
            //마우스에서 손가락을 떼면 조이스틱이 기능을 잃고 원래 위치로 돌아감
        }
        else
        {

        }


        if (MoveFlag)
        {
            Player.transform.Translate(Vector3.forward * Time.deltaTime * 5f);


        }




    }

    public void OnDestroy()
    {
        thread.Abort();
    }

    public void DragEnd()
    {
        Stickback.transform.position = Stickback1;//조이스틱의 배경을 원래위치로
        Stick.position = Stick1;// 스틱을 원래의 위치로.
        JoyVec = Vector3.zero;          // 방향을 0으로.
        MoveFlag = false; //움직임이 없다는 표시

        playerpos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Data3 mydata = new Data3();

        mydata.type = "1";
        mydata.userindex = PlayerPrefs.GetString("userindex");
        mydata.id = PlayerPrefs.GetString("id");
        mydata.nickname = PlayerPrefs.GetString("nickname");

        Vector3 destination_rot = new Vector3(0, yrot, 0);

        Vector3 destination = playerpos.position + (destination_rot * 10f * Time.deltaTime);

        string playerpos_x = playerpos.position.x.ToString("N4");
        string playerpos_y = playerpos.position.y.ToString("N4");
        string playerpos_z = playerpos.position.z.ToString("N4");
        mydata.playerpos.Add(playerpos_x);
        mydata.playerpos.Add(playerpos_y);
        mydata.playerpos.Add(playerpos_z);
        mydata.playerrot = yrot.ToString();


        mydata.destinationpos.Add(playerpos_x);
        mydata.destinationpos.Add(playerpos_y);
        mydata.destinationpos.Add(playerpos_z);


        mydata.movestate = "1";

        jsondata = JsonUtility.ToJson(mydata);
        Debug.Log("6." + jsondata);


        string toSend = jsondata;

            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string clientMessage = toSend;
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }


            Debug.Log("Client send ");

    }


    static void Receiving()
    {
        //쓰레드를 실행
        // Receiving
        while (true)
        {
           
            Byte[] bytes = new byte[1024];
            while (true)
            {

                //반복해서 서버에서 데이터를 받고 스테틱 변수에 반영

                using (NetworkStream stream = socketConnection.GetStream())
                {

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

            }

        }
    }
    IEnumerator sending()
    {

        while (true)
        {

            playerpos = GameObject.FindWithTag("Player").GetComponent<Transform>();
            Data3 mydata = new Data3();
            // var idobj = GameObject.Find("connserver").GetComponent<connectserver>();

            Vector3 destination_rot = new Vector3(0, 0, yrot);

            Vector3 destination = playerpos.position + (destination_rot * 0.1f * Time.deltaTime);

            mydata.type = "1";
            mydata.userindex = PlayerPrefs.GetString("userindex");
            mydata.id = PlayerPrefs.GetString("id");
            mydata.nickname = PlayerPrefs.GetString("nickname");

            string destinationpos_x = destination.x.ToString("N4");
            string destinationpos_y = destination.y.ToString("N4");
            string destinationpos_z = destination.z.ToString("N4");
            mydata.destinationpos.Add(destinationpos_x);
            mydata.destinationpos.Add(destinationpos_y);
            mydata.destinationpos.Add(destinationpos_z);

            string playerpos_x = playerpos.position.x.ToString("N4");
            string playerpos_y = playerpos.position.y.ToString("N4");
            string playerpos_z = playerpos.position.z.ToString("N4");
            mydata.playerpos.Add(playerpos_x);
            mydata.playerpos.Add(playerpos_y);
            mydata.playerpos.Add(playerpos_z);
            mydata.playerrot = yrot.ToString();
           
            mydata.movestate = "0";

            jsondata = JsonUtility.ToJson(mydata);
            Debug.Log("6." + jsondata);


            string toSend = jsondata;

          
            Debug.Log("Client send ");
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string clientMessage = toSend;
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }


            yield return new WaitForSeconds(0.2f);

        }

    }


}