using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;



public class eqitemdatajson
{
    public List<string> newskilljs;
    public List<string> sellitemjs;
    public List<string> sellskilljs;
    public List<string> sellskillpartjs;
}
public class newskilldatajson
{
    public int skillid;
}
public class sellitemdatajson
{
    public int useritemindex_it;
}
public class sellskilldatajson
{
    public int useritemindex_sk;
}
public class sellskillpartdatajson
{

    public int useritemindex_skp;

    public int useritemcount;
}







public class itemdatasend : MonoBehaviour
{
    public List<string> itemdatajs = new List<string>();

    public List<string> newskilldata = new List<string>();

    // public Hashtable buylist = new Hashtable();
    public List<string> sellitemdata = new List<string>();

    public List<string> sellskilldata = new List<string>();

    public List<string> sellskillpartdata = new List<string>();

    public List<string> totallist = new List<string>();

    public string itemToJason;
   
    public string result;

    public equipmentstart equipstartcs;

    void Start()
    {
        equipstartcs = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();

    }
    
    public void test() { 
    
   // StartCoroutine(selldatasend());

    }

    private void OnDisable()
    {
        //아이템 판매 정보 부터 보내기
        selldatasend();

        //아이템 착용정보 보내기
        string id = PlayerPrefs.GetString("id");
        Debug.Log("id" + id);

        string pw = PlayerPrefs.GetString("pw");
        Debug.Log("pw" + pw);

        int userindex = PlayerPrefs.GetInt("userindex");

        string gunid_pp = PlayerPrefs.GetString("gunid");
        Debug.Log("!!!gunid_pp:"+ gunid_pp);
        int gunid;
        int.TryParse(gunid_pp, out gunid);

        string armorid_pp = PlayerPrefs.GetString("armorid");
        Debug.Log("!!!armorid_pp:" + armorid_pp);
       // int armorid = int.Parse(armorid_pp);
        int armorid;
        int.TryParse(armorid_pp, out armorid);

        string skill1id_pp = PlayerPrefs.GetString("skill1id");
        int skill1id;
          int.TryParse(skill1id_pp, out skill1id);

        string skill2id_pp = PlayerPrefs.GetString("skill2id");
        int skill2id;
          int.TryParse(skill2id_pp, out skill2id);

        string totalpower_pp = PlayerPrefs.GetString("totalpower");
        int totalpower;
          int.TryParse(totalpower_pp, out totalpower);

        string totalhp_pp = PlayerPrefs.GetString("totalhp");
        int totalhp;
        int.TryParse(totalhp_pp, out totalhp);



        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", id);
        w.AddField("pw", pw);
        w.AddField("userindex", userindex);
        w.AddField("gunid", gunid);
        w.AddField("armorid", armorid);
        w.AddField("skill1id", skill1id);
        w.AddField("skill2id", skill2id);
        w.AddField("totalpower", totalpower);
        w.AddField("totalhp", totalhp);


        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/useritemchange.php", w);

        Debug.Log("4 , 통신 반환값 전달");
        // yield return ;
         www.SendWebRequest();

        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5, 네트워크 통신 error: " + www.error);

        }
        else //서버 연결 성공
        {

            result = www.downloadHandler.text;
            Debug.Log("5, 네트워크 통신 성공 result: " + result);
        }

    
    }



      public void selldatasend() 
   // IEnumerator selldatasend()
    {

        eqitemdatajson eqitemdatajson = new eqitemdatajson();

       

      
        //새로 만든 스킬을 json데이터로 만든다
        foreach (int newskillid in equipstartcs.newskilllist)
        {

            newskilldatajson newskilldatajson = new newskilldatajson();


            newskilldatajson.skillid = newskillid;

            //데이터로 직렬화 한다
            string newskilldataToJason = JsonUtility.ToJson(newskilldatajson);
            newskilldata.Add(newskilldataToJason);


        }


        //아이템 판매 정보를 json데이터로 만든다
        foreach (int sellitemid in equipstartcs.sellitemlist)
        {

            sellitemdatajson sellitemdatajson = new sellitemdatajson();

            //json 데이터를 직렬화 한다
            sellitemdatajson.useritemindex_it = sellitemid;

            string sellitemdataToJason = JsonUtility.ToJson(sellitemdatajson);
            sellitemdata.Add(sellitemdataToJason);


        }
        //스킬 판매 정보를 json데이터로 만든다
        foreach (int sellskillid in equipstartcs.sellskilllist)
        {

            sellskilldatajson sellskilldatajson = new sellskilldatajson();

            //json 데이터를 직렬화 한다
            sellskilldatajson.useritemindex_sk = sellskillid;

            string sellskilldataToJason = JsonUtility.ToJson(sellskilldatajson);
            sellskilldata.Add(sellskilldataToJason);


        }


        //스킬조각 정보를 보낸다 아이템 수량과 아이디
        foreach ( DictionaryEntry sellskillpartid in equipstartcs.sellskillpartlist)
        {

            sellskillpartdatajson sellskillpartdatajson = new sellskillpartdatajson();


            int key;
            int.TryParse(sellskillpartid.Key.ToString(), out key);

            int value;
            int.TryParse(sellskillpartid.Value.ToString(), out value);


            sellskillpartdatajson.useritemindex_skp = key;
            sellskillpartdatajson.useritemcount = value;

            string sellskillpartdataToJason = JsonUtility.ToJson(sellskillpartdatajson);
            sellskillpartdata.Add(sellskillpartdataToJason);


        }

        //각각 직렬화 해서 리스트로 만든 것들을 다시 큰 json으로 만든
        eqitemdatajson.newskilljs = newskilldata;
        eqitemdatajson.sellitemjs = sellitemdata;
        eqitemdatajson.sellskilljs = sellskilldata;
        eqitemdatajson.sellskillpartjs = sellskillpartdata;

        //이거 보내기
       itemToJason = JsonUtility.ToJson(eqitemdatajson);
        Debug.Log("test1" + itemToJason);



        string id = PlayerPrefs.GetString("id");
        Debug.Log("id" + id);

        string pw = PlayerPrefs.GetString("pw");
        Debug.Log("pw" + pw);

        int userindex = PlayerPrefs.GetInt("userindex");

        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("sellitemjson", itemToJason);
        w.AddField("id", id);
        w.AddField("pw", pw);
        w.AddField("userindex", userindex);


        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/usersellitem.php", w);

        Debug.Log("4 , 통신 반환값 전달");
     //    yield return 
        www.SendWebRequest();

        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5, 네트워크 통신 error: " + www.error);

        }
        else //서버 연결 성공
        {

            result = www.downloadHandler.text;
            Debug.Log("5, 네트워크 통신 성공 result: " + result);
        }





    }






}
