using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class itemjson
{

    public List<string> passivearray;
    public List<string> activearray;

}


public class itemsubjson
{

    public string itemtype;
    public string itemid;
    public string itemcount;
}


public class itemsend : MonoBehaviour
{

    public List<string> passivestring = new List<string>();
    public List<string> activestring = new List<string>();
    public string result;
    public string itemToJason;
    public int gold;
    public int clickcount = 0;


    public void invensend() {

        //스태틱 초기화
        if (clickcount == 0)
        {


            itemjson itemjson = new itemjson();

            gold = singleinventory.goldinventory;

            foreach (DictionaryEntry activeskill in singleinventory.activeskillinventory)
            {
               
                itemsubjson itemsubjson = new itemsubjson();
                itemsubjson.itemtype = "activeskillpart";
                itemsubjson.itemid = activeskill.Key.ToString();
                itemsubjson.itemcount = activeskill.Value.ToString();

                string itemsubToJason = JsonUtility.ToJson(itemsubjson);
                activestring.Add(itemsubToJason);
            }
            //현재가지고 있는 패시브 스킬 조각을 가진다
            foreach (DictionaryEntry passiveskill in singleinventory.passiveskillinventory)
            {

               
                itemsubjson itemsubjson = new itemsubjson();
                itemsubjson.itemtype = "passiveskillpart";
                itemsubjson.itemid = passiveskill.Key.ToString();
                itemsubjson.itemcount = passiveskill.Value.ToString();

                string itemsubToJason = JsonUtility.ToJson(itemsubjson);
                passivestring.Add(itemsubToJason);

            }

            itemjson.passivearray = passivestring;

            itemjson.activearray = activestring;
            itemToJason = JsonUtility.ToJson(itemjson);


            Debug.Log(itemToJason);
            StartCoroutine(senditem());


            currentability.abilitylist = new List<int>();

            singleinventory.goldinventory = 0;
            singleinventory.passiveskillinventory = new Hashtable();
            singleinventory.activeskillinventory = new Hashtable();
            clickcount = 1;
        }

    }

    IEnumerator senditem() {


        //서버에서 케릭터의 아이템 리스트를 받아온다
        string id = PlayerPrefs.GetString("id");
        Debug.Log("id" + id);

        string pw = PlayerPrefs.GetString("pw");
        Debug.Log("pw" + pw);

        int userindex = PlayerPrefs.GetInt("userindex");


        //스테이지가 3인데 clear변수가 1이면

        int chapter = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().chapter;

        int chapterclear = GameObject.Find("clearcheck").GetComponent<clearcheck>().chapterclear;

        //현재 챕터 +1;
        int maxchapter = chapter;

        if (chapterclear == 1) {
            //보내도 됨
            maxchapter = chapter + 1;


        }


        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", id);
        w.AddField("pw", pw);
        w.AddField("userindex", userindex);
        w.AddField("gold", gold);
        w.AddField("maxchapter", maxchapter);
        w.AddField("itemjson", itemToJason);

        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/itemsend.php", w);

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
            clickcount = 0;
            SceneManager.LoadScene("mainscene");

        }






    }






}
