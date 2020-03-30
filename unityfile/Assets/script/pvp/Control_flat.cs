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
using Pvpdata;
using FlatBuffers;


public class Control_flat : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject joy;
    public Transform Stickback;
    public Transform Stick;         // 조이스틱.
    public Transform player;
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

    //tcp 클라이언트 변수
    public static TcpClient socketConnection;

    //서버에서 받는 메세지
    public static string serverMessgae = "";

    //서버에서 받는 바이트 버퍼
    public static ByteBuffer serverMessgae2 ;

    //플레이어 데이터 제이슨으로 바꾼 뒤 스트링으로 바꾼 데이터
    public string jsondata;
    public static Thread thread;

    // public static NetworkStream stream = Findenemy_flat.stream;

    public static float totalTime;

    float yrot;

    public static Coroutine co_my_coroutine;

    public static Coroutine count_coroutine;

    public static Queue<ByteBuffer> receivequeue;

    public Animator anim;


    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;

    public static int clickcount = 0;

    public static Image fillImage;

    protected float maxValue = 2f, minValue = 0f;

    // Create a property to handle the slider's value
    public static float fillPercentage;
    public static float currentValue = 0f;
    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            // Ensure the passed value falls within min/max range
            currentValue = Mathf.Clamp(value, minValue, maxValue);

            // Calculate the current fill percentage and display it
            fillPercentage = currentValue / maxValue;
            fillImage.fillAmount = fillPercentage;

        }
    }


    public void Start()
    {
        fillImage = GameObject.Find("Fillimage").GetComponent<Image>();
        CurrentValue = 0f;

        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
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

        //플레이어 애니메이션
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();

        //소켓은 상대찾기 버튼을 누를때 연결된 소켓을 이용한다
        socketConnection = Findenemy_flat.socketConnection;

        //바이트버퍼를 담을 수 있는 큐생성
        receivequeue = new Queue<ByteBuffer>();
      //  StartCoroutine(receivall());
        
        //thread = new Thread(Receiving);
        //receiveing2 쓰레드 생성
        thread = new Thread(Receiving2);
        //쓰레드 시작
        thread.Start();

    }


    void Update()
        //이동에 대한 함수
    {

        //마우스를 누를때

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.y >= Stickback.transform.position.y - 100)
            {
                //pvp 상황에서 시작부터 총알을 안쏘게 하기 위한 카운트변수
                clickcount = 1;
                //** MoveFlag = true;
                MoveFlag = false;
                //뛰는 애니메이션 적용 파라미터는 animator에 있음
                //**  anim.SetBool("run", true);

                //조이스틱 배경을 마우스가 클릭한 위치로 이동
                //**  Stickback.transform.position = Input.mousePosition;
                Debug.Log("mouspoint" + Input.mousePosition);

                //조이스틱도 마우스가 클릭한 위치로 이동
                //**  Stick.transform.position = Input.mousePosition; // 타겟을 레이캐스트가 충돌된 곳으로 옮긴다.
                movefirst = Stick.transform.position;
                //이동한 위치(클릭한위치)

                //  if (!Mathf.Approximately(yrot, 0.0f))
                //   {
                //sending() 코루틴 시작
                co_my_coroutine = StartCoroutine(sending());
                //   }


            }

        }
        //마우스 누르고 있을때
        else if (Input.GetMouseButton(0))
        {
            if (count_coroutine != null) {
                StopCoroutine(count_coroutine);
            }


          

            //    if(Input.mousePosition.y >= Stickback.transform.position.y-50 && Input.mousePosition.y <= Stickback.transform.position.y + 50 && Input.mousePosition.x >= Stickback.transform.position.x - 50 && Input.mousePosition.x <= Stickback.transform.position.x + 50) {
                if (Input.mousePosition.y >= Stickback.transform.position.y - 100 )
                {

                if (player.GetComponent<Playerinfo>().playerskill1id != 0) {

                    // CurrentValue += 0.0086f;
                    CurrentValue += 0.0152f;
                }


                anim.SetBool("run", true);
                MoveFlag = true;
                Debug.Log("mousemove");
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

                //내가 2p 일때 함수
                if (Pvpscenestart_flat.enemyindex == 0)
                {
                    player.eulerAngles = new Vector3(0, Mathf.Atan2(-JoyVec.x, -JoyVec.y) * Mathf.Rad2Deg, 0);
                    yrot = Mathf.Atan2(-JoyVec.x, -JoyVec.y) * Mathf.Rad2Deg;
                }
                //내가 1p 일때 함수
                else if (Pvpscenestart_flat.enemyindex == 1)
                {
                    player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
                    yrot = Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg;
                }

                //오일러각 회전각   // x,y를 알고 있으니 아크탄젠트 버전2 각도를 구할 수 있다  //Mathf.Rad2Deg: 라디안을 각도로 변환해주는 함수

                //   player.transform.Translate(Vector3.forward * Time.deltaTime * 10f);

                Debug.Log("mouspoint" + Input.mousePosition);
                // 타겟을 레이캐스트가 충돌된 곳으로 옮긴다.


            }




        }

        //마우스 떼기
        else if (Input.GetMouseButtonUp(0))
        {
            if (Input.mousePosition.y >= Stickback.transform.position.y - 100)
            {
                //코루틴을 멈춘다
                StopCoroutine(co_my_coroutine);

                //**  count_coroutine = StartCoroutine(stopcount());

                Debug.Log("mouseoff");
                DragEnd();
                //애니메시션 끝
                anim.SetBool("run", false);
                //마우스에서 손가락을 떼면 조이스틱이 기능을 잃고 원래 위치로 돌아감
            }
        }
        else
        {

        }


        if (MoveFlag)
        {
            //10의 속도로 전방으로 이동
            player.transform.Translate(Vector3.forward * Time.deltaTime * 10f);


        }




    }

    public void OnDestroy()
    {
        //씬이 끝나게 되면 쓰레드가 멈춤
        thread.Abort();
    }

    public void DragEnd()
    {
        //드래그가 끝나는 함수

        Stickback.transform.position = Stickback1;//조이스틱의 배경을 원래위치로
        Stick.position = Stick1;// 스틱을 원래의 위치로.
        JoyVec = Vector3.zero;          // 방향을 0으로.
        MoveFlag = false; //움직임이 없다는 표시

        //목적지의 방향을 정해준다 -> 조이스틱에서 y가 캐릭터에게는 z 좌표가 됨
        Vector3 destination_rot = new Vector3(0, 0, yrot);

        //목적지를 정해준다 위치 + 방향 * 거리
        Vector3 destination = player.position + (destination_rot * + 2f * Time.deltaTime);

        float playerpos_x = player.position.x;
        float playerpos_y = player.position.y;
        float playerpos_z = player.position.z;

        //버퍼 만들기 (id, nickname, 포지션 , 방향 , 이동상태 , 목적, 공격상태 , 맞는상태 )
        builder = new FlatBufferBuilder(1024);
        var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
        var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));

        Player.StartPlayer(builder);
        Player.AddStartstate(builder, PlayerStart.Play);
        Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
        Player.AddId(builder, idoffset);
        Player.AddNickname(builder, nicknameoffset);
        Player.AddPlayerpos(builder, Vec3.CreateVec3(builder, playerpos_x, playerpos_y, playerpos_z));
        Player.AddPlayerrot(builder, yrot);
        Player.AddMovestate(builder, MoveState.Stop);
        Player.AddDestinationpos(builder, Vec3.CreateVec3(builder, playerpos_x, playerpos_y, playerpos_z));
        Player.AddAttacked(builder, 0);
        Player.AddFire(builder, 0);
        Player.AddSkillfire(builder, 0);
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


        if (Findenemy_flat.stream.CanWrite)
        {
            //데이터를 서버에 스트림으로 보낸다 bytearray
            Findenemy_flat.stream.Write(sendBuffer, 0, sendBuffer.Length);
            Findenemy_flat.stream.Flush();
        }

        Debug.Log("mose off Client send ");

    }

    /*
    static void Receiving()
    {
        //쓰레드를 실행
        // Receiving
       

           
            while (true)
            {
                 Byte[] bytes = new byte[1024];
                //반복해서 서버에서 데이터를 받고 스테틱 변수에 반영
               //  using (NetworkStream stream = socketConnection.GetStream())  
              //  {

                    int length;
                    
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                    var incomingData = new byte[length];
                        Array.Copy(bytes, 0, incomingData, 0, length);
                        serverMessgae2 = new ByteBuffer(incomingData);
                        Player pvpplayer = Player.GetRootAsPlayer(serverMessgae2);

                    //receivequeue.Enqueue(serverMessgae2);
                    Debug.Log("Client received2 rot: " + pvpplayer.Playerrot);
                    Debug.Log("Client received2 state: " + pvpplayer.Movestate);
                    }
               }

         //   }

        
    }
    */

    static void Receiving2()
    {
        //쓰레드를 실행
        // Receiving
        //시간이 얼마나 흐르는지 확인하는 스탑워치
      //  System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();


        while (true)
        {
         //   sw.Start();

            Byte[] bytes = new byte[1024];

            //반복해서 서버에서 데이터를 받고 스테틱 변수에 반영
           
             //   int length = stream.Read(bytes, 0, bytes.Length);
             int length = Findenemy_flat.stream.Read(bytes, 0, bytes.Length);
           // Debug.Log(bytes.Length);
                var incomingData = new byte[length];
                Array.Copy(bytes, 0, incomingData, 0, length);
            //마지막 변수를 만들고
                serverMessgae2 = new ByteBuffer(incomingData);
            //큐도 만든다 
                receivequeue.Enqueue(serverMessgae2);
            //  Debug.Log("Client received2 rot: " + pvpplayer.Playerrot);
            //  Debug.Log("Client received2 state: " + pvpplayer.Movestate);
         //   sw.Stop();

        //    Debug.Log(sw.ElapsedMilliseconds.ToString() + "ms");



              }
            

        }
    /*
    IEnumerator stopcount()
    {
       
        //시간을 표시하는 함수
        float duration = 0; // 3 seconds you can change this to
                            //to whatever you want
        totalTime = 4f;
        while (totalTime >= duration)
        {
            //매초 시간이 4에서 1씩 줄어든.
            totalTime -= Time.deltaTime;
            // choose how to quantize this 
            //0이 되면 Go로 바뀜
          //  Debug.Log("stopcount :" + totalTime);
            yield return null;
        }
        totalTime = 0;
       
    }
    */

    IEnumerator sending()
    {

        //이동할때 발생하는 코루틴
        while (true)
        {

            Debug.Log("courutin continue");
            // var idobj = GameObject.Find("connserver").GetComponent<connectserver>();

            Vector3 destination_rot = new Vector3(0, 0, yrot);

            Vector3 destination = player.position + (destination_rot * 0.1f * Time.deltaTime);

            float destinationpos_x = destination.x;
            float destinationpos_y = destination.y;
            float destinationpos_z = destination.z;


            float playerpos_x = player.position.x;
            float playerpos_y = player.position.y;
            float playerpos_z = player.position.z;

            builder = new FlatBufferBuilder(1024);
            var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
            var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));
            Player.StartPlayer(builder);
            Player.AddStartstate(builder, PlayerStart.Play);
            Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
            Player.AddId(builder, idoffset);
            Player.AddNickname(builder, nicknameoffset);
            Player.AddPlayerpos(builder, Vec3.CreateVec3(builder, playerpos_x, playerpos_y, playerpos_z));
            Player.AddPlayerrot(builder, yrot);
            Player.AddMovestate(builder, MoveState.Move);
            Player.AddDestinationpos(builder, Vec3.CreateVec3(builder, destinationpos_x, destinationpos_y, destinationpos_z));
            Player.AddAttacked(builder, 0);
            Player.AddFire(builder, 0);
            Player.AddSkillfire(builder, 0);
            Offset<Player> pypplayer = Player.EndPlayer(builder);
         //   builder.Finish(pypplayer.Value);

          //  sendBuffer = builder.SizedByteArray();
          //  sendbb = new ByteBuffer(sendBuffer);

            Game.StartGame(builder);
            Game.AddPlayer(builder, pypplayer);
            Game.AddTablenum(builder, 0);
            Offset<Game> game = Game.EndGame(builder);

            builder.Finish(game.Value);
            sendBuffer = builder.SizedByteArray();
            sendbb = new ByteBuffer(sendBuffer);


            Debug.Log("Client send ");
            //  NetworkStream stream = socketConnection.GetStream();
            /*
                  if (stream.CanWrite)
               {
                   stream.Write(sendBuffer, 0, sendBuffer.Length);
                   stream.Flush();
               }
               */
           
            if (Findenemy_flat.stream.CanWrite)
            {
                Findenemy_flat.stream.Write(sendBuffer, 0, sendBuffer.Length);
                Findenemy_flat.stream.Flush();
            }
            //1프레임마다 데이터 전송
            yield return new WaitForSeconds(0.1f);

        }

    }


}
