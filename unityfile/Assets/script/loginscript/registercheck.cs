using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class registercheck : MonoBehaviour
{
    //팝업창의 제목 창
    public Text titletext;

    //팝업창의 내용 창
    public Text contenttext;

    //팝업창 패널 전체의 변수
    public GameObject panel;


    //회원 가입 함수
    public void register_check()
    {
        //passwordcheck 스크립트에서 패스워드가 일치하는지 확인하는 변수 가져오기
        var pw = GameObject.Find("passwordcheck").GetComponent<passwordcheck>();
        string pw_check = pw.pwcheck;

        //pause 스크립트에서 이메일 인증이 완료되었는지 확인하는 변수 가져오기
        var email = GameObject.Find("puase").GetComponent<puase>();
        string email_check = email.emailcheck;

        //이메일 변수
        string id = email.signup_id.text;
        //패스워드 변수
        string password = pw.password2.text;



        //이메일 인증 하고 비밀번호가 일치 일때
        if (pw_check == "1" && email_check == "1")
        {
            Debug.Log("1, 회원가입 함수 작동");
            StartCoroutine(Upload());

        }

        //이메일 인증 하고 비밀번호가 불일치 일때
        else if (pw_check == "1" && email_check == "0")
        {
            //이메일을 확인해주세요
            Debug.Log("1, 팝업창 활성화");
            titletext.text = "회원가입 실패";
            contenttext.text = "이메일을 확인해주세요";
            //팝업창 활성화
            panel.gameObject.SetActive(true);
        }

        //이메일 인증 안하고 비밀번호가 일치 일때
        else if (pw_check == "0" && email_check == "1")
        {
            //비밀번호를 확인해주세요
            Debug.Log("1, 팝업창 활성화");
            titletext.text = "회원가입 실패";
            contenttext.text = "비밀번호를 확인해주세요";
            //팝업창 활성화
            panel.gameObject.SetActive(true);
        }

        //이메일 비밀번호 둘다 안했을 때
        else
        {
            Debug.Log("1, 팝업창 활성화");
            //이메일을 인증과 비밀번호를 확인해주세요
            titletext.text = "회원가입 실패";
            contenttext.text = "이메일을 인증과 비밀번호를 확인해주세요";
            //팝업창 활성화
            panel.gameObject.SetActive(true);
        }






    }

    IEnumerator Upload()
    {
        //password 스크립트에서 비밀번호 일치인지 확인하는 변수 가져오기
        var pw = GameObject.Find("passwordcheck").GetComponent<passwordcheck>();
        string pw_check = pw.pwcheck;

        //pause 스크립트에서 이메일을 인증했는지 확인하는 변수 가져오기
        var email = GameObject.Find("puase").GetComponent<puase>();
        string email_check = email.emailcheck;

        string id = email.signup_id.text;
        string password = pw.password2.text;


        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", id);
        w.AddField("pw", password);
        w.AddField("logincategory", "robotwar");
        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/register.php", w);

        Debug.Log("4 , 통신 반환값 전달");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5, 네트워크 통신 error: "+ www.error);
           
        }
        else //서버 연결 성공
        {

            string result = www.downloadHandler.text;
            Debug.Log("5, 네트워크 통신 성공 result: " + result);

            if (result == "ok") {
                Debug.Log("6, result ok / 로그인 페이지로 이동" );
                SceneManager.LoadScene("login_robotwar");

            }
            else {

                Debug.Log("6, result no");
            }


        }

    }


}
