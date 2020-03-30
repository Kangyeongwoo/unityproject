using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class puase : MonoBehaviour
{
    //퍼즈 체크 변수
    bool bPaused;

    //아이디 인풋 필드
    public InputField signup_id;

    //텍스트 이메일 인증 했는지 표출하는 텍스트
    public Text myText;

    //이메일 체크에 대한 확인 변수
    public string emailcheck = "0";

    //일시정지를 감지하는 함수
    private void OnApplicationPause(bool pause)
    {
        //퍼즈가 되어 있을 때
        if (pause)
        {
            //퍼즈를 확인하기 위한 변수 true
            bPaused = true;
            Debug.Log(" 1, 일시정지 ");
        }
        else
        {//일시정지가 풀릴 때

            if (bPaused) {
                //퍼즈를 확인하기 위한 변수 false
                bPaused = false;
                Debug.Log(" 1, 일시정지 해제 ");


                Debug.Log(" 2, 이메일이 인증 됬는지 확인하는 함수 실행 ");
                StartCoroutine(Upload());

            }

        }

    }


    //이메일 다 입력하고 손가락 띄었을 때 작동하는 함수
    public void idcheckbox_check() {
        //서버와 통신하여 이메일 인증 체크하는 함수 실행행
        Debug.Log(" 1, 이메일이 인증 됬는지 확인하는 함수 실행 ");
        StartCoroutine(Upload());
    }


    //서버와 통신하여 아이디를 체크하는 함수
    IEnumerator Upload()
    {
        //form 과 그 안에 들어가는 변수
        Debug.Log(" 1, IEnumerator Upload() 함수 실행 ");
        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", signup_id.text);
        Debug.Log(" 2, form으로 아이디 전달 ");


        //포스트 방식으로 form 의 데이터 전송
        Debug.Log("3 , emailcheck.php 에 post로 아이디 전달");

        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/emailcheck.php", w);

        Debug.Log("4 , 네트워크 통신 결과 전달");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5 , 네트워크 통신 에러 :"+ www.error);

        }
        else //서버 연결 성공
        {

            //php에서 echo로 전달 되는 데이터를 변수로 만듦
            string result = www.downloadHandler.text;
            Debug.Log("5 , 네트워크 통신 result :"+ result);

            //결과가 yes라면
            if (result == "yes")
            {
                //인증완료 표출 emailcheck=1
                myText.text = "인증완료";
                emailcheck = "1";
                Debug.Log("6 , 네트워크 통신 result ok / emailcheck:" + emailcheck);

            }
            else
            {//결과가 yes가 아니면

                emailcheck = "0";
                //공백 표출 emailcheck=0
                Debug.Log("email" + emailcheck);
                myText.text = "";
                Debug.Log("6 , 네트워크 통신 result no / emailcheck:" + emailcheck);
            }



        }


    }

}
