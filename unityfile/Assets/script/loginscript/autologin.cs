using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class autologin : MonoBehaviour
{   
    //앱이 실행될 때 동작하는 함수
    public void Awake()
    {

        //플레이어프렙스에 id key가 있는지 확인
        if (PlayerPrefs.HasKey("id") == true) {


            Debug.Log("1 , PlayerPrefs.HasKey(id) ok");
            //이 시점에 upload 함수를 실행
            StartCoroutine(Upload());
            Debug.Log("5.5 , coroutine 종료 되고 ienumerator 나머지 동작");

        }
        else {
            //플레이어프렙스에 id key가 없음 로그인 한 기록이 없거나 로그아웃 했음
            Debug.Log("1 , PlayerPrefs.HasKey(id) no");
        }

        IEnumerator Upload()
        {
            //form 과 그 안에 들어가는 변수
            //저장된 아이디
            string id = PlayerPrefs.GetString("id");
            Debug.Log("2 id:" + id);
            //저장된 패스워드
            string pw = PlayerPrefs.GetString("pw");
            Debug.Log("3 pw:" + pw);
            //저장된 로그인 방식
            string logincategory = PlayerPrefs.GetString("logincategory");
            Debug.Log("4 logincategory:" + logincategory);

            //이전에 로그인한 방식이 robotwar계정 로그인 이라면
            if (logincategory=="robotwar") {

                //w에 id,pw를 합쳐서 post로 전달
                WWWForm w = new WWWForm();
                w.AddField("select", "submit");
                w.AddField("id", id);
                w.AddField("pw", pw);

                UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/login.php", w);
                Debug.Log("5 , http://49.247.131.90/login.php에 post");

                yield return www.SendWebRequest();


                //서버 연결 실패
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("6 , 네트워크 연결 실패 errcode: "+ www.error);

                }
                else //서버 연결 성공
                {

                    //php에서 echo로 전달 되는 데이터를 변수로 만듦
                    string result = www.downloadHandler.text;
                    Debug.Log("6 , 네트워크 연결 성공 result: " + result);

                   
                    if (result == "ok")
                    {
                        Debug.Log("7 , goto main");
                        //메인 신으로 넘어가는 함수
                        SceneManager.LoadScene("mainscene");

                    }
                    else
                    {
                        Debug.Log("7 , result not ok / dont go");
                    }

                }

            }
            //이전 로그인이 facebook로그인으로 이루어져 있을 때
            else if(logincategory=="facebook")
            {
                Debug.Log("5 , facebook login 함수 실행");
                //facebook 스크립트가 들어있는 오브젝트에서 스크립트를 참고하는 함수
                var facebook = GameObject.Find("facebook").GetComponent<facebookback>();
                //facebook 스크립트의 로그인 함수
                facebook.Initcheck();
            }

        }

    }
}
