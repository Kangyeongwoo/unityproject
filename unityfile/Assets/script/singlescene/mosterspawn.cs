using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;


public class mosterspawn : MonoBehaviour
{
    //monster 변수
    public GameObject monster;
    public List<GameObject> monsterlist;
    public  int monstercount = 0;

    public static string result;
    public int monster1id;
    public int monster2id;
    public int monster3id;

    private void Start()
    {
       
        StartCoroutine(spawn());
    }
    IEnumerator spawn()
    {

        yield return new WaitForSeconds(1f);


        int stage = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().stage;
        int chapter = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().chapter;

        //총쏘기 가능
        if (chapter == 1)
        {
            monster1id = 1;
            monster2id = 2;
            monster3id = 3;

            if (stage == 1)
            {

               StartCoroutine(monsterdata());
                
              




                yield return null;
            }


            if (stage == 2)
            {

                var jsonPlayer = JSON.Parse(result);

                //useritemData 로 아이템에 대한 정보를 받을 수 있다
                int mapmonsterDatalength = jsonPlayer["mapmonsterData"].Count;

                var mapmonster1 = jsonPlayer["mapmonsterData"][0];

                var mapmonster2 = jsonPlayer["mapmonsterData"][1];

                var mapmonster3 = jsonPlayer["mapmonsterData"][2];

                //최대 2번 반복 반복 횟수(몬스터생성횟)는 스테이지 별로 다르게 관리
                for (int i = 0; i < 3; i++)
                {

                    //랜덤한 X좌표
                    float randomX = Random.Range(-10f, 10f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
                                                             //랜덤한 Y좌표
                    float randomZ = Random.Range(-15f, 20f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.

                    Quaternion monrot = Quaternion.Euler(0, 180, 0);


                    monster = Resources.Load<GameObject>("Monster/stoneMonsterfac");

                    //monster 프래팹 생성
                    GameObject enemy_sub = Instantiate(monster, new Vector3(randomX, 0f, randomZ), monrot);
                    GameObject enemy_body = enemy_sub.transform.GetChild(0).gameObject;

                    enemy_body.GetComponent<singlemonsterinfo>().monsterid = 1;
                    enemy_body.GetComponent<singlemonsterinfo>().monsterattack = mapmonster1["monsterpower"];
                    enemy_body.GetComponent<singlemonsterinfo>().monsterhp = mapmonster1["monstermaxhp"];
                    enemy_body.GetComponent<singlemonsterinfo>().monstermaxhp = mapmonster1["monstermaxhp"];
                    enemy_body.GetComponent<singlemonsterinfo>().monstermoney = mapmonster1["monstergold"];

                    //   enemy_sub.transform.localScale = new Vector3(2, 2, 2);
                    monsterlist.Add(enemy_sub);
                    monstercount += 1;
                }
                for (int i = 0; i < 3; i++)
                {



                    //랜덤한 X좌표
                    float randomX = Random.Range(-10f, 10f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
                                                             //랜덤한 Y좌표
                    float randomZ = Random.Range(-15f, 20f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.

                    Quaternion monrot = Quaternion.Euler(0, 180, 0);
                    monster = Resources.Load<GameObject>("Monster/towerMonsterfac");

                    //monster 프래팹 생성
                    GameObject enemy_sub = Instantiate(monster, new Vector3(randomX, 0, randomZ), monrot);
                    GameObject enemy_body = enemy_sub.transform.GetChild(0).gameObject;

                    enemy_body.GetComponent<singlemonsterinfo>().monsterid = 2;
                    enemy_body.GetComponent<singlemonsterinfo>().monsterattack = mapmonster2["monsterpower"];
                    enemy_body.GetComponent<singlemonsterinfo>().monsterhp = mapmonster2["monstermaxhp"];
                    enemy_body.GetComponent<singlemonsterinfo>().monstermaxhp = mapmonster2["monstermaxhp"];
                    enemy_body.GetComponent<singlemonsterinfo>().monstermoney = mapmonster2["monstergold"];
                    monsterlist.Add(enemy_sub);

                    monstercount += 1;
                }
                //resultdata 가지고 넣기

                yield return null;
            }



            if (stage == 3)
            {
                //최대 2번 반복 반복 횟수(몬스터생성횟)는 스테이지 별로 다르게 관리
                for (int i = 0; i < 1; i++)
                {
                    var jsonPlayer = JSON.Parse(result);

                    //useritemData 로 아이템에 대한 정보를 받을 수 있다
                    int mapmonsterDatalength = jsonPlayer["mapmonsterData"].Count;

                    var mapmonster1 = jsonPlayer["mapmonsterData"][0];

                    var mapmonster2 = jsonPlayer["mapmonsterData"][1];

                    var mapmonster3 = jsonPlayer["mapmonsterData"][2];

                    //랜덤한 X좌표
                 //   float randomX = Random.Range(-10f, 10f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
                                                             //랜덤한 Y좌표
                 //   float randomZ = Random.Range(-15f, 20f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.

                    Quaternion monrot = Quaternion.Euler(0, 180, 0);


                    monster = Resources.Load<GameObject>("Monster/bossstoneMonsterfac");

                    //monster 프래팹 생성
                    GameObject enemy_sub = Instantiate(monster, new Vector3(0f, 0f, 0f), monrot);
                    GameObject enemy_body = enemy_sub.transform.GetChild(0).gameObject;

                    enemy_body.GetComponent<singlemonsterinfo>().monsterid = 3;
                    enemy_body.GetComponent<singlemonsterinfo>().monsterattack = mapmonster3["monsterpower"];
                    enemy_body.GetComponent<singlemonsterinfo>().monsterhp = mapmonster3["monstermaxhp"];
                    enemy_body.GetComponent<singlemonsterinfo>().monstermaxhp = mapmonster3["monstermaxhp"];
                    enemy_body.GetComponent<singlemonsterinfo>().monstermoney = mapmonster3["monstergold"];
                    //   enemy_sub.transform.localScale = new Vector3(2, 2, 2);
                    monsterlist.Add(enemy_sub);





                    monstercount += 1;

                }

                //resultdata 가지고 넣기

                yield return null;
            }
        }

        //몬스터가 다 생겼으니 총을 쏘게 만든다
        GameObject.FindWithTag("Player").GetComponent<singleplayerfire>().shootbool = 0;



    }


    IEnumerator monsterdata() {

        //서버에서 케릭터의 아이템 리스트를 받아온다

        string id = PlayerPrefs.GetString("id");
        Debug.Log("id" + id);

        string pw = PlayerPrefs.GetString("pw");
        Debug.Log("pw" + pw);

        int userindex = PlayerPrefs.GetInt("userindex");


        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("monster1id", monster1id);
        w.AddField("monster2id", monster2id);
        w.AddField("monster3id", monster3id);


        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/mapmonsterdata.php", w);

        Debug.Log("4 , 통신 반환값 전달");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5, 네트워크 통신 error: " + www.error);

        }
        else //서버 연결 성공
        {

            result = www.downloadHandler.text;
            Debug.Log("5, 네트워크 통신 성공 result: " + result);

            //json 데이터로 아이템 데이터, 스킬데이터를 받아온다
            var jsonPlayer = JSON.Parse(result);

            //useritemData 로 아이템에 대한 정보를 받을 수 있다
            int mapmonsterDatalength = jsonPlayer["mapmonsterData"].Count;

            var mapmonster1 = jsonPlayer["mapmonsterData"][0];

            var mapmonster2 = jsonPlayer["mapmonsterData"][1];

            var mapmonster3 = jsonPlayer["mapmonsterData"][2];



            //최대 2번 반복 반복 횟수(몬스터생성횟)는 스테이지 별로 다르게 관리
            for (int i = 0; i < 3; i++)
            {
                //근거리 몬스터
                //랜덤한 X좌표
                float randomX = Random.Range(-10f, 10f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
                                                         //랜덤한 Y좌표
                float randomZ = Random.Range(-15f, 20f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.

                Quaternion monrot = Quaternion.Euler(0, 180, 0);


                monster = Resources.Load<GameObject>("Monster/stoneMonsterfac");

                //monster 프래팹 생성
                GameObject enemy_sub = Instantiate(monster, new Vector3(randomX, 0f, randomZ), monrot);

                GameObject enemy_body = enemy_sub.transform.GetChild(0).gameObject;



                enemy_body.GetComponent<singlemonsterinfo>().monsterid = 1;
                enemy_body.GetComponent<singlemonsterinfo>().monsterattack = mapmonster1["monsterpower"];
                enemy_body.GetComponent<singlemonsterinfo>().monsterhp = mapmonster1["monstermaxhp"];
                enemy_body.GetComponent<singlemonsterinfo>().monstermaxhp = mapmonster1["monstermaxhp"];
                enemy_body.GetComponent<singlemonsterinfo>().monstermoney = mapmonster1["monstergold"];
              //  enemy_body.GetComponent<singlemonsterinfo>().monsteratkspeed = 1;


                //   enemy_sub.transform.localScale = new Vector3(2, 2, 2);
                monsterlist.Add(enemy_sub);
                monstercount += 1;
            }
            for (int i = 0; i < 2; i++)
            {
                //원거리 몬스터
                //랜덤한 X좌표
                float randomX = Random.Range(-10f, 10f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
                                                         //랜덤한 Y좌표
                float randomZ = Random.Range(-15f, 20f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.

                Quaternion monrot = Quaternion.Euler(0, 180, 0);
                monster = Resources.Load<GameObject>("Monster/towerMonsterfac");

                //monster 프래팹 생성
                GameObject enemy_sub = Instantiate(monster, new Vector3(randomX, 0, randomZ), monrot);

                GameObject enemy_body = enemy_sub.transform.GetChild(0).gameObject;

                enemy_body.GetComponent<singlemonsterinfo>().monsterid = 2;
                enemy_body.GetComponent<singlemonsterinfo>().monsterattack = mapmonster2["monsterpower"];
                enemy_body.GetComponent<singlemonsterinfo>().monsterhp = mapmonster2["monstermaxhp"];
                enemy_body.GetComponent<singlemonsterinfo>().monstermaxhp = mapmonster2["monstermaxhp"];
                enemy_body.GetComponent<singlemonsterinfo>().monstermoney = mapmonster2["monstergold"];


                monsterlist.Add(enemy_sub);

                monstercount += 1;
            }






        }




    }


    }
