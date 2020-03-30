using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System;

public class nicknamecheck : MonoBehaviour
{
    //닉네임 텍스트 상자
    public InputField nicknametext;

    //닉네임 에러 났을때 글자
    public Text nicknameerror;

    //닉네임 입력하는 패널
    public GameObject nicknamepanel;

    public void nickname_check() {


        StartCoroutine(nickname_ck());

    }

    IEnumerator nickname_ck()
    {
        //닉네임 입력받기
        string nickname = nicknametext.text;


        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("nickname", nickname);
   

        Debug.Log("3 , register.php에 form 전달");
        //닉네임이 있는지 체크
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/nicknamecheck.php", w);

        Debug.Log("4 , 통신 반환값 전달");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5, 네트워크 통신 error: " + www.error);

        }
        else //서버 연결 성공
        {

            string result = www.downloadHandler.text;
            Debug.Log("5, 네트워크 통신 성공 result: " + result);

            if (result == "ok")
            {
                Debug.Log("6, result ok / 로그인 페이지로 이동");
                StartCoroutine(nickname_up());

            }
            else
            {
                nicknameerror.text = "이미 존재하는 닉네임입니다";
                Debug.Log("6, result no");
            }


        }

    }


    IEnumerator nickname_up()
    {
        string nickname = nicknametext.text;
        string id = PlayerPrefs.GetString("id");
        string pw = PlayerPrefs.GetString("pw");

        WWWForm w = new WWWForm();

        w.AddField("id", id);
        w.AddField("pw", pw);
        w.AddField("select", "submit");
        w.AddField("nickname", nickname);


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/nicknameregister.php", w);

        Debug.Log("4 , 통신 반환값 전달");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5, 네트워크 통신 error: " + www.error);

        }
        else //서버 연결 성공
        {

            string result = www.downloadHandler.text;
            Debug.Log("5, 네트워크 통신 성공 result: " + result);

            if (result == "no_pw")
            {
               
            }
            else if(result == "no_id")
            {

                Debug.Log("6, result no");
            }
            else { 
            
            
                 var jsonPlayer = JSON.Parse(result);
                /*
                string nickname2 = jsonPlayer["nickname"];
                string level = jsonPlayer["level"];
                string gold = jsonPlayer["gold"];
                string wincount = jsonPlayer["wincount"];
                string losecount = jsonPlayer["losecount"];
                string power = jsonPlayer["power"];
                string hp = jsonPlayer["hp"];

                PlayerPrefs.SetString("nickname", nickname2);
                PlayerPrefs.SetString("level", level);
                PlayerPrefs.SetString("gold", gold);
                PlayerPrefs.SetString("wincount", wincount);
                PlayerPrefs.SetString("losecount", losecount);
                PlayerPrefs.SetString("power", power);
                PlayerPrefs.SetString("hp", hp);
                */
     
                string userindex_js = jsonPlayer["userindex"];
                int userindex = Convert.ToInt32(userindex_js);

                string nickname2 = jsonPlayer["nickname"];


                string level_js = jsonPlayer["level"];
                int level = Convert.ToInt32(level_js);

                string gold_js = jsonPlayer["gold"];
                int gold = Convert.ToInt32(gold_js);

                string wincount_js = jsonPlayer["wincount"];
                int wincount = Convert.ToInt32(wincount_js);

                string losecount_js = jsonPlayer["losecount"];
                int losecount = Convert.ToInt32(losecount_js);

                string power_js = jsonPlayer["power"];
                int power = Convert.ToInt32(power_js);

                string hp_js = jsonPlayer["hp"];
                int hp = Convert.ToInt32(hp_js);

                string gunid = jsonPlayer["gunid"];

                string aromrid = jsonPlayer["aromrid"];

                string skill1id = jsonPlayer["skill1id"];

                string skill2id = jsonPlayer["skill2id"];

                string drawcount = jsonPlayer["drawcount"];

                string totalpower = jsonPlayer["totalpower"];

                string totalhp = jsonPlayer["totalhp"];

                string maxchapter_js = jsonPlayer["maxchapter"];
                int maxchapter = Convert.ToInt32(maxchapter_js);

                string maxstage_js = jsonPlayer["maxstage"];
                int maxstage = Convert.ToInt32(maxstage_js);


                PlayerPrefs.SetInt("userindex", userindex);
                PlayerPrefs.SetString("nickname", nickname2);
                PlayerPrefs.SetInt("level", level);
                PlayerPrefs.SetInt("gold", gold);
                PlayerPrefs.SetInt("wincount", wincount);
                PlayerPrefs.SetInt("losecount", losecount);
                PlayerPrefs.SetInt("power", power);
                PlayerPrefs.SetInt("hp", hp);

                PlayerPrefs.SetString("gunid", gunid);
                PlayerPrefs.SetString("aromrid", aromrid);
                PlayerPrefs.SetString("skill1id", skill1id);
                PlayerPrefs.SetString("skill2id", skill2id);
                PlayerPrefs.SetString("drawcount", drawcount);
                PlayerPrefs.SetString("totalpower", totalpower);
                PlayerPrefs.SetString("totalhp", totalhp);

                PlayerPrefs.SetInt("maxchapter", maxchapter);
                PlayerPrefs.SetInt("maxstage", maxstage);

                nicknamepanel.SetActive(false);
                Debug.Log("6, result ok / 로그인 페이지로 이동");



            }

        }

    }



}
