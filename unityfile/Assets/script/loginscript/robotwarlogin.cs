using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class robotwarlogin : MonoBehaviour
{
    //robotwar 로그인
    public InputField id;
    //robotwar 패스워드
    public InputField pw;
    //메일 발송 패널
    public GameObject authpanel;

    //로그인 함수
    public void login() {

        Debug.Log("1, 로그인 함수 시작");
        StartCoroutine(Upload());

    }

    IEnumerator Upload()
    {

        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", id.text);
        w.AddField("pw", pw.text);

        Debug.Log("2, login.php에 id pw 전달 ");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/login.php", w);

        Debug.Log("3, 통신 결과 리턴");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("4, 통신 에러 : "+ www.error);
           
        }
        else //서버 연결 성공
        {

            string result = www.downloadHandler.text;
            Debug.Log("5 , 통신 결과 :"+result);

            // echo ok 일때
            if (result == "ok")
            {
                Debug.Log("6 , 플레이어 프랩스에 로그인 정보 저장 / 메인 이동 ");
                PlayerPrefs.SetString("id", id.text);
                PlayerPrefs.SetString("pw", pw.text);
                PlayerPrefs.SetString("logincategory", "robotwar" );
                SceneManager.LoadScene("mainscene");

            }
            else {
                //로그인 실패
                //입력하신 정보를 다시 확인해주십시오
                //팝업창 활성화
                Debug.Log("6 , 팝업 활성화 ");
                authpanel.gameObject.SetActive(true);
            }


        }


    }


}
