using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class facebookback : MonoBehaviour
{
    // Include Facebook namespace

    // Awake function from Unity's MonoBehavior

    //페이스북 로그인 버튼 클릭한적 있는지 확인하여 init을 못하게
    //페북로그인 버튼 클릭 카운트 
    public static string facebookcheck = "0";



    //페이스북 로그아웃 함수 fb.init이 되어 있어야함
    public void CallFBLogout()
    {
        //facebook sdk 에서 제공하는 함수
        //이전 로그인 상태를 확인해서 담는 변수
        string logincategory = PlayerPrefs.GetString("logincategory");
       
        //이전에 페북 로그인을 했으면
       if (logincategory == "facebook")
        {
            //자동 로그인이 될 때도 init을 연결 하므로 logout 함수 사용 가능
            FB.LogOut();
            Debug.Log("1 , CallFBLogout");
            PlayerPrefs.DeleteKey("id");
            PlayerPrefs.DeleteKey("pw");
            PlayerPrefs.DeleteKey("logincategory");
            Debug.Log("2 , 플레이어프랩스 정보 삭제");
            Debug.Log("3 , login씬으로 이동");
            SceneManager.LoadScene("login");


        }else if (logincategory == "robotwar") {
            //이전에 로봇워 계정으로 로그인 했으면

            PlayerPrefs.DeleteKey("id");
            PlayerPrefs.DeleteKey("pw");
            PlayerPrefs.DeleteKey("logincategory");
            Debug.Log("1 , 플레이어프랩스 정보 삭제");
            Debug.Log("2 , login씬으로 이동");
            SceneManager.LoadScene("login");
        }
        else {
            PlayerPrefs.DeleteKey("id");
            PlayerPrefs.DeleteKey("pw");
            PlayerPrefs.DeleteKey("logincategory");
            Debug.Log("1 , 플레이어프랩스 정보 삭제");
            Debug.Log("2 , login씬으로 이동");
            SceneManager.LoadScene("login");
        }
    }


    //페이스북 로그인이 진행되는 함수 init이 되어 있어야 한진행
    public void Initcheck()
    {   
        //init이 안되어있을때
        if (facebookcheck == "0")
        {
            //facebook에서 제공하는 함수 가장먼저 시작 
            Debug.Log("1 , init이 안되어있을 때 / Init()함수 시작");
            FB.Init(SetInit, OnHideUnity);


            facebookcheck = "1";

        }
        //init이 되어있을때
        else {
            //init과 로그인 상태 판별
            Debug.Log("1 , init이 되어있을 때 / setinit()함수 시작");
            SetInit();

            facebookcheck = "1";
        }
    }

        private void SetInit()
        {
            Debug.Log("2 , FB Init done.");

            //이전에 이 앱에 로그인 했었다면
            if (FB.IsLoggedIn)
            {   
                //페이스북 로그인이 되어 있다면 로그 생성 연결이 되어있음
                Debug.Log("3 , FB Logged In");

                Debug.Log("4 , 바로 로그인 하는 함수 시작");
                StartCoroutine(Upload_login());

            }
            else {
            //facebook으로 로그인 되어있지 않으면 아래 함수 시작 연결이 안되어있음
            Debug.Log("3 , FB not login");

            Debug.Log("4 , FB 에서 토큰 받아와 로그인");
            FBlogin();
            }

        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        //실질적인 로그인 함수
        void FBlogin() {
            //프로필과 이메일 정보를 가지고 온다
            var perms = new List<string>() { "public_profile", "email" };
            //페이스북에서 정보 전달 허용
            FB.LogInWithReadPermissions(perms, AuthCallback);
            
            void AuthCallback(ILoginResult result)
            {   
                //페이스북 로그인 성공
                if (FB.IsLoggedIn)
                {
                    // AccessToken class will have session details
                    // 엑세스 토큰 가져오기
                    var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                    // Print current access token's User ID
                    // 엑세스 토큰은 로그아웃하거나 해도 똑같다
                    Debug.Log("5 , FB.IsLoggedIn");
                    Debug.Log("6 , fb token" + aToken.UserId);

                // Print current access token's granted permissions
                // 디버그에 페이스북에서 어떤 항목을 가지고 왔는지 for문을 통해 보여줌
                /* foreach (string perm in aToken.Permissions)
                 {
                     Debug.Log(perm);
                 }
                 */
                Debug.Log("7 , 회원가입 함수 실행" );
                StartCoroutine(Upload());

              } 
                else
                {
                    //페이스북 로그아웃 상황에 나타나는 로그
                    Debug.Log(" 5 , User cancelled login");
                }
            }
        }

    IEnumerator Upload()
    {
        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        // 페이스북 토큰의 아이디값
        w.AddField("id", aToken.UserId);
        Debug.Log("8 , 가입 id :"+ aToken.UserId);

        // 페이스북 토큰의 아이디값
        w.AddField("pw", aToken.UserId);
        Debug.Log("9 , 가입 pw :"+ aToken.UserId);

        // 로그인 카테고리 페이스북 
        w.AddField("logincategory", "facebook");
        Debug.Log("10 , 가입 형식 :facebook");

        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/register.php", w);

        Debug.Log("11 , 회원가입 전달");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("12 , 네트워크 error :"+ www.error);
           
        }
        else //서버 연결 성공
        {
            //서버 에코값 가져오기
            string result = www.downloadHandler.text;
            Debug.Log("12 , 네트워크 성공 result :" + result);


            if (result == "ok")
            {
                Debug.Log("13 , register result :" + result);
                Debug.Log("14 , 페이스북 로그인 함수 실행 ");
                StartCoroutine(Upload_login());

            }
            else {
                Debug.Log("13 , register result error"+ result);
            }


        }

    }

    IEnumerator Upload_login()
    {

        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

        // form 형성
        WWWForm w = new WWWForm();

        w.AddField("select", "submit");
        // 페이스북 토큰의 아이디값
        w.AddField("id", aToken.UserId);
        // 페이스북 토큰의 아이디값
        w.AddField("pw", aToken.UserId);

        //http://49.247.131.90/login.php 애 post form 전달
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/login.php", w);
        Debug.Log("1 , 로그인 form 전달");

        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("2 , 네트워크 error"+ www.error);

        }
        else //서버 연결 성공
        {

            string result = www.downloadHandler.text;
            Debug.Log("2 , 네트워크 성공 result" + result);

            if (result == "ok")
            {
                //플레이어프랩스 임시로 id, pw, 로그인 방식 저장
                PlayerPrefs.SetString("id", aToken.UserId);
                PlayerPrefs.SetString("pw", aToken.UserId);
                PlayerPrefs.SetString("logincategory", "facebook");
                Debug.Log("3 , 네트워크 성공 result" + result);

                Debug.Log("4 , goto main");

                SceneManager.LoadScene("mainscene");

            }
            else
            {
                Debug.Log("3 , login result error" + result);

                //입력하신 정보를 다시 확인해주십시오

            }


        }


    }


}
