using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class rewardsend : MonoBehaviour
{
    public string result;
    public void sendreward() {

        StartCoroutine(senditem());
  
   }




    IEnumerator senditem()
    {
        int itemid = GameObject.Find("bossmonstercontrol_cs").GetComponent<bossmonstercontrol>().rewardid;

        string itemtype;
        if (itemid % 2 == 0)
        {
            itemtype = "armor";
        }
        else {
            itemtype = "gun";
        }

        //서버에서 케릭터의 아이템 리스트를 받아온다
        string id = PlayerPrefs.GetString("id");
        Debug.Log("id" + id);

        string pw = PlayerPrefs.GetString("pw");
        Debug.Log("pw" + pw);

        int userindex = PlayerPrefs.GetInt("userindex");

        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", id);
        w.AddField("pw", pw);
        w.AddField("userindex", userindex);
        w.AddField("itemid",itemid);
        w.AddField("itemtype", itemtype);
        w.AddField("itemcount", 1);

        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/rewardsend.php", w);

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
            Debug.Log("5, !!!!!!!!!!!!!!!!!네트워크 통신 성공 result: " + result);

            SceneManager.LoadScene("bosssceneready");

        }






    }



}
