using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System;

public class mainstart : MonoBehaviour
{

    public GameObject nicknamepanel;
    public GameObject mapselectpanel;

    void Awake() {

        mapselectpanel = GameObject.Find("mapselectpanel");

        mapselectpanel.SetActive(false);

        nicknamepanel.SetActive(false);

        StartCoroutine(checknickname());

    }

    IEnumerator checknickname()
    {
        string id = PlayerPrefs.GetString("id");
        Debug.Log("id"+id);

        string pw = PlayerPrefs.GetString("pw");
        Debug.Log("pw" + pw);



        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", id);
        w.AddField("pw", pw);
        w.AddField("logincategory", "robotwar");
        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/nickname.php", w);

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

            if (result == "no_nick")
            {
                nicknamepanel.SetActive(true);
                Debug.Log("6, result no");

            }
            else if(result == "no_pw")
            {
                Debug.Log("6, result no_pw");
            }
            else if (result == "no_id")
            {
                Debug.Log("6, result no_id");
            }
            else {

                Debug.Log("6, result"+result);
                var jsonPlayer = JSON.Parse(result);

                string userindex_js = jsonPlayer["userindex"];
                int userindex = Convert.ToInt32(userindex_js);

                string nickname = jsonPlayer["nickname"];
               

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
                PlayerPrefs.SetString("nickname", nickname);
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
               
                Debug.Log("7, result" + PlayerPrefs.GetString("maxchapter"));


                GameObject.Find("goldtext").GetComponent<Text>().text = "G : "+gold_js;

                GameObject.Find("nicknametext").GetComponent<Text>().text = nickname;

                //스테이지 선택값이 고정되도록 한
                int currentchapter = PlayerPrefs.GetInt("currentchapter");

                if (currentchapter != 0) {
                    maxchapter = currentchapter;
                }

                if (maxchapter == 1) {

                    GameObject mapobj = Resources.Load<GameObject>("singleMap/chapter1obj");

                    Instantiate(mapobj);

                    var mapname = GameObject.Find("chaptername").GetComponent<Text>();

                    mapname.text = "1. 사막";


                }else if (maxchapter == 2) {

                    GameObject mapobj = Resources.Load<GameObject>("singleMap/chapter2obj");

                    Instantiate(mapobj);

                    var mapname = GameObject.Find("chaptername").GetComponent<Text>();

                    mapname.text = "2. 초원";


                }






            }


        }

    }
}
