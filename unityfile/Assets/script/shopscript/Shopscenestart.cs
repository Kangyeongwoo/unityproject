using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System;

public class Shopscenestart : MonoBehaviour
{
    //통신 후 받는 결과 값
    public string result;

    //무기 담아둘 빈객체
    public GameObject gunitemslot;

    //방어구 담아둘 빈객체
    public GameObject armoritemslot;

    //슬롯이 들어갈 아이템인벤토리자체
    public GameObject gunshopinventory;
    //스킬슬롯이 들어갈 스킬인벤토리자체
    public GameObject armorshopinventory;

    //아이템 눌렀을때 나오는 패널
    public GameObject shopitempanel;

    private void Awake()
    {
        shopitempanel = GameObject.Find("shopitempanel");
        shopitempanel.SetActive(false);
        gunshopinventory = GameObject.Find("gunshopinventory");
        armorshopinventory = GameObject.Find("armorshopinventory");

        int gold = PlayerPrefs.GetInt("gold");
        string nickname = PlayerPrefs.GetString("nickname");

        GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + gold.ToString();

        GameObject.Find("nicknametext").GetComponent<Text>().text = nickname;

        StartCoroutine(shopitemlist());
    }


    IEnumerator shopitemlist() {

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
        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/shopitem.php", w);

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
            Debug.Log("5, 네트워크 통신 성공 result: " + result);

            //리스트에서 가져온걸 inventory에 넣기

            //json 데이터로 아이템 데이터, 스킬데이터를 받아온다
            var jsonPlayer = JSON.Parse(result);

            //shopitem 로 아이템에 대한 정보를 받을 수 있다
            //판매하는 무기가 몇개인지
            int gunshopitemlength = jsonPlayer["gunshopitem"].Count;
            //판매하는 방어구가 몇개인지
            int armorshopitemlength = jsonPlayer["armorshopitem"].Count;

            //판매하는 총 슬롯을 만들고 이미지랑 내용을 적용
            for (int i = 0; i < gunshopitemlength; i++)
            {
                var gunshopdata = jsonPlayer["gunshopitem"][i];

                Debug.Log("useritemData : " + gunshopdata["itemname"]);

                //인벤토리에 프리팹을 만들고 스크립트에 내용입, 이미지 적용
                gunitemslot = Resources.Load<GameObject>("shopitemslot");

                //아이템 슬롯을 오브젝트로 만들기
                GameObject gunitem = Instantiate(gunitemslot);

                //아이템 리스트로 만들기
               // itemobjlist.Add(gunitem);
                
                //아이템 슬롯을 인벤토리의 하위로 만든다
                gunitem.transform.SetParent(gunshopinventory.transform);
                gunitem.transform.localScale = new Vector3(1, 1, 1);
               
                string itemtype = gunshopdata["itemtype"];
                string itemid = gunshopdata["itemid"];

                //인벤토리의 아이템 슬롯에 이미지를 적용
                string imagename = itemtype + itemid;
                // RawImage itemimage = gunitem.GetComponent<RawImage>();
                //  Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
                //  itemimage.texture = itemtexture;
                gunitem.transform.GetChild(0).GetComponent<Text>().text = gunshopdata["itemname"];
                Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
                gunitem.transform.GetChild(1).GetComponent<RawImage>().texture = itemtexture;
                string pricemodify = gunshopdata["price"] + " G";
                gunitem.transform.GetChild(2).GetComponent<Text>().text = pricemodify;


                //아이템창 슬롯에 있는 iteminfo에 데이터 저장
                gunitem.GetComponent<shopiteminfo>().itemname = gunshopdata["itemname"];
                gunitem.GetComponent<shopiteminfo>().itemtype = gunshopdata["itemtype"];
                gunitem.GetComponent<shopiteminfo>().itemid = gunshopdata["itemid"];
                gunitem.GetComponent<shopiteminfo>().itemdescription = gunshopdata["itemdescription"];
                gunitem.GetComponent<shopiteminfo>().atk = gunshopdata["atk"];
                gunitem.GetComponent<shopiteminfo>().def = gunshopdata["def"];
                gunitem.GetComponent<shopiteminfo>().atkspeed = gunshopdata["atkspeed"];
                gunitem.GetComponent<shopiteminfo>().equiped = gunshopdata["equiped"];
                gunitem.GetComponent<shopiteminfo>().rare = gunshopdata["rare"];
                gunitem.GetComponent<shopiteminfo>().price = gunshopdata["price"];
                gunitem.GetComponent<shopiteminfo>().listindex = i;

            }

            for (int i = 0; i < armorshopitemlength; i++)
            {
                var armorshopdata = jsonPlayer["armorshopitem"][i];

                Debug.Log("useritemData : " + armorshopdata["itemname"]);

                //인벤토리에 프리팹을 만들고 스크립트에 내용입, 이미지 적용
                armoritemslot = Resources.Load<GameObject>("shopitemslot");

                //아이템 슬롯을 오브젝트로 만들기
                GameObject armoritem = Instantiate(armoritemslot);

                //아이템 리스트로 만들기
                // itemobjlist.Add(gunitem);

                //아이템 슬롯을 인벤토리의 하위로 만든다
                armoritem.transform.SetParent(armorshopinventory.transform);
                armoritem.transform.localScale = new Vector3(1, 1, 1);

                string itemtype = armorshopdata["itemtype"];
                string itemid = armorshopdata["itemid"];

                //인벤토리의 아이템 슬롯에 이미지를 적용
                string imagename = itemtype + itemid;
                // RawImage itemimage = armoritem.GetComponent<RawImage>();
                //  Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
                //  itemimage.texture = itemtexture;

                armoritem.transform.GetChild(0).GetComponent<Text>().text = armorshopdata["itemname"];
                Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
                armoritem.transform.GetChild(1).GetComponent<RawImage>().texture = itemtexture;
                string pricemodify = armorshopdata["price"] + " G";
                armoritem.transform.GetChild(2).GetComponent<Text>().text = pricemodify;



                //아이템창 슬롯에 있는 iteminfo에 데이터 저장
                armoritem.GetComponent<shopiteminfo>().itemname = armorshopdata["itemname"];
                armoritem.GetComponent<shopiteminfo>().itemtype = armorshopdata["itemtype"];
                armoritem.GetComponent<shopiteminfo>().itemid = armorshopdata["itemid"];
                armoritem.GetComponent<shopiteminfo>().itemdescription = armorshopdata["itemdescription"];
                armoritem.GetComponent<shopiteminfo>().atk = armorshopdata["atk"];
                armoritem.GetComponent<shopiteminfo>().def = armorshopdata["def"];
                armoritem.GetComponent<shopiteminfo>().atkspeed = armorshopdata["atkspeed"];
                armoritem.GetComponent<shopiteminfo>().equiped = armorshopdata["equiped"];
                armoritem.GetComponent<shopiteminfo>().rare = armorshopdata["rare"];
                armoritem.GetComponent<shopiteminfo>().price = armorshopdata["price"];
                armoritem.GetComponent<shopiteminfo>().listindex = i;

            }





        }




    }
    }
