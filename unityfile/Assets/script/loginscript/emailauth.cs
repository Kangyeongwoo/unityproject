using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class emailauth : MonoBehaviour
{
    //패널 변수
    public GameObject authpanel;

    //아이디입력필드 변수
    public InputField signup_id;

    //입력 error 표출 텍스트창 변수 (아이디 존재 , 형식 오류)
    public Text iderrortext;



    //id체크 함수 업로드 함수를 실행
    public void idcheck() {
        Debug.Log("1 , idcheck함수의 코루틴 시작");
        StartCoroutine(Upload());
        Debug.Log("4 , 코루틴 sendrequest 리턴 완료");
    }

    //서버와 통신하여 아이디를 체크하는 함수
    IEnumerator Upload()
    {
        //form 과 그 안에 들어가는 변수
        Debug.Log("2 , form에 id 넣기");
        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", signup_id.text);

        //포스트 방식으로 form 의 데이터 전송
        Debug.Log("3 , http://49.247.131.90/sendmail.php 에 post로 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/sendmail.php", w);


        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5 , 네트워크 연결 에러 errcode :"+ www.error);

        }
        else //서버 연결 성공
        {
           
            //php에서 echo로 전달 되는 데이터를 변수로 만듦
            string result = www.downloadHandler.text;
            Debug.Log("5 , 네트워크 연결 성공 result :"+ result);

            //이메일 전달하고 서버에서 보낸 반환값을 <br> 로 쪼개서 담을 배열
            string[] stringSeparators = new string[] { "<br>" };
            Debug.Log("6 , 서버에서 보낸 이메일 전달 상황 배열에 담기");

            // 스플릿 한 배열을 빈값없애고 다시 배열에 담기
            string[] split_text = result.Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);
            Debug.Log("7 , 스플릿된 배열을 빈 값 없도록 배열에 담기");
            //StringSplitOptions.None

            //배열의 마지막을 담는 변수
            string result2 = split_text[split_text.Length - 1];
            Debug.Log("8 , 배열의 마지막 값 :"+ result2.Trim());

            //php 에서 전달값 1 기존 아이디가 없으며 이메일 형식 정확
            if (result2.Trim() == "1") {
                // 확인 팝업창 열기
                authpanel_active();
                Debug.Log("9 , 이메일 전달 성공");
            }
            else {
                //기존 아이디가 있거나 이메일 형식이 이상하면 text 출력
                iderrortext.text = result;
                Debug.Log("9 , 이메일 전달 실패 이유:"+ result);

               
            }
        }


    }


    //인증 번호 입력 패널 활성화하는 함수
    public void authpanel_active() {

        //인증 번호 입력 패널 활성화
        authpanel.gameObject.SetActive(true);


    }



}
