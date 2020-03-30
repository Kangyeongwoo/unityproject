using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class shopitemjson
{

    public List<string> buylist_js;

}


public class shopitemsubjson
{

    public string itemtype;
    public string itemid;
  //  public string itemcount;
}

public class buybutton : MonoBehaviour
{

    List<string> buylist_js = new List<string>();

    // public Hashtable buylist = new Hashtable();
    public List<string> buylist = new List<string>();

    public string itemToJason;
    public string result;
    public int shoptoeqcount =0;

    //패널 취소 버튼 함수
    public void panelcancel() {

        GameObject.Find("shopscenestart_cs").GetComponent<Shopscenestart>().shopitempanel.SetActive(false);

    }

    //아인템 사는 버튼
    public void buyitem() {


        int curruentgold = PlayerPrefs.GetInt("gold");

        var shoppanel = GameObject.Find("shopscenestart_cs").GetComponent<Shopscenestart>().shopitempanel;
        var shoppanelinfo = shoppanel.GetComponent<shoppanelinfo>();

        int itemprice;
        int.TryParse(shoppanelinfo.price, out itemprice);


        if (curruentgold >= itemprice)
        {
            int minusgold = curruentgold - itemprice;
            PlayerPrefs.SetInt("gold", minusgold);
            GameObject.Find("goldtext").GetComponent<Text>().text = minusgold.ToString()+" G";
            // buylist.Add(shoppanelinfo.itemid,shoppanelinfo.itemtype);
            buylist.Add(shoppanelinfo.itemid);
            //구매 완료 -> 확인 원래 패널 까지 꺼짐
            shoppanel.SetActive(false);
        }
        else { 
        
            //잔액 부족
        
        }


    }


    //씬이 꺼질때 발동하는 함
    private void OnDisable()
    {

        if (shoptoeqcount == 0)
        {

            shopitemjson shopitem_js = new shopitemjson();


            foreach (string buyone in buylist)
            {

                shopitemsubjson shopitemsubjson = new shopitemsubjson();

                int itemid_or;
                int.TryParse(buyone, out itemid_or);

                if (itemid_or%2== 0)
                {
                    shopitemsubjson.itemtype = "armor";
                }
                else
                {
                    shopitemsubjson.itemtype = "gun";
                }

                shopitemsubjson.itemid = buyone;



                string shopitemsubToJason = JsonUtility.ToJson(shopitemsubjson);
                buylist_js.Add(shopitemsubToJason);


            }

            shopitem_js.buylist_js = buylist_js;
            itemToJason = JsonUtility.ToJson(shopitem_js);

            Debug.Log(itemToJason);


            string id = PlayerPrefs.GetString("id");
            Debug.Log("id" + id);

            string pw = PlayerPrefs.GetString("pw");
            Debug.Log("pw" + pw);

            int userindex = PlayerPrefs.GetInt("userindex");

            int gold = PlayerPrefs.GetInt("gold");

            WWWForm w = new WWWForm();
            w.AddField("select", "submit");
            w.AddField("id", id);
            w.AddField("pw", pw);
            w.AddField("userindex", userindex);
            w.AddField("gold", gold);
            w.AddField("buyitemjson", itemToJason);


            Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


            Debug.Log("3 , register.php에 form 전달");
            UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/buyitem.php", w);

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
        else { 
        
        }

    }




}
