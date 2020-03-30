using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class registercancel : MonoBehaviour
{
    //회원가입 취소 버튼 툴렀을때 함수
    public void register_cancel() {

        //퍼스의 이메일 체크값 가져오기
        var email = GameObject.Find("puase").GetComponent<puase>();
        string email_check = email.emailcheck;

        if (email_check=="1")
        {
            //취소버튼을 눌렀는데 인증을 했으면 임시 데이터 삭제
            Debug.Log("1 , 임시데이터 삭제 함수 실행");
            StartCoroutine(Upload());


        }
        else {
            //취소 버튼 누르고 인증 안한 상태 이면 바로 화면 전환
            Debug.Log("1 , 로그인 화면으로 이동");
            SceneManager.LoadScene("login_robotwar");

        }
    }

    IEnumerator Upload()
    {
        //퍼스의 변수와 스크립트 사용
        var email = GameObject.Find("puase").GetComponent<puase>();
        string email_check = email.emailcheck;
        string id = email.signup_id.text;

        //form 생성
        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", id);

        Debug.Log("2 , registercancel.php 에 이메일 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/registercancel.php", w);

        Debug.Log("3 , 통신 결과 반환");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("4 , 통신 에러 : "+ www.error);

        }
        else //서버 연결 성공
        {

            string result = www.downloadHandler.text;
            Debug.Log("4 , 통신 성공 resutl:" + result);

            //서버 반환 값이 ok일때
            if (result == "ok")
            {
                Debug.Log("5 , 통신 결과 ok / 로그인 화면으로 이동");
                SceneManager.LoadScene("login_robotwar");

            }
            else{
                //서버 반환 값이 ok가 아닐때
                Debug.Log("5 , 통신 결과 no / 이동 안함");

            }


        }






    }


}
