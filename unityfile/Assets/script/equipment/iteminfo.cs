using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iteminfo : MonoBehaviour
{
    public string itemname;

    public string equiped;

    public string itemtype;

    public string itemid;

    public int listindex;

    public string itemdescription;

    public string atk;

    public string atkspeed;

    public string def;

    public string rare;

    public string price;

    public int useritemindex = 0;

    public Text itemviewname;

    public RawImage itemviewimage;

    public Text itemviewdescription;

    public Text itemviewspec;

    public GameObject weaponeslot;

    public GameObject armorslot;


    //슬롯 안에 들어있는 함수 슬롯 안에 있어서 아이템인벤토리에서 슬롯을 클릭하면 반응하는 함수

    public void viewactive() {

        //패널을 나타내고 내용과 이미지를 채운다
        GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itempanel.SetActive(true);

        itemviewname = GameObject.Find("itemname").GetComponent<Text>();
        itemviewimage = GameObject.Find("itemImage").GetComponent<RawImage>();
        itemviewdescription = GameObject.Find("itemdescription").GetComponent<Text>();
        itemviewspec = GameObject.Find("itemspec").GetComponent<Text>();
        itemviewname.text = itemname;
        itemviewdescription.text = itemdescription;

        //패널에도 지금 보고 있는 아이템의 인덱스를 보내주고 아이템 타입을 보내준다
        //그래야 패널이 장비 해제할때 안보이는 슬롯을 살릴수 있다

        itempanel_index.listindex = listindex;
        GameObject.Find("itempanel").GetComponent<itempanel_index>().itemtype = itemtype;
        GameObject.Find("itempanel").GetComponent<itempanel_index>().equiped = equiped;
        GameObject.Find("itempanel").GetComponent<itempanel_index>().useritemindex = useritemindex;

        GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itembt1name.SetActive(true);
        GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itembt2name.SetActive(true);
        GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itembt3name.SetActive(false);


        Text btname = GameObject.Find("equipbttext").GetComponent<Text>();
       /*
         if (equiped.Equals("0"))
        {
            Debug.Log("bteq0:" + equiped);
            btname.text = "장비";
        }
        else
        {
            Debug.Log("bteq1:" + equiped);
            btname.text = "장비해제";
        }
        */

        Debug.Log("패널 누를때item:"+ equiped);


        string imagename = itemtype + itemid;
        Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
        itemviewimage.texture = itemtexture;

        if (itemtype.Equals("gun")) {

            //총이면 다음의 내용을 표시한다 패널에
            // string spec = "공격력 +" + atk + "\n"+"공격속도:"+atkspeed;
            string spec = "공격력 +" + atk;
            itemviewspec.text = spec;

        }
        else if (itemtype.Equals("armor")) {

            //방어구면 다음의 내용을 표시한다 패널에
            string spec = "HP +" + def ;
            itemviewspec.text = spec;

        }


    }


    public void changeitem()
    {
        //패널에서 장비 버튼을 클릭시 발동하는 함수 , 무기와 방어구만 적용됨

        var equipstartcs = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();
        string imagename = itemtype + itemid;
       
        if (itemtype.Equals("gun")) {

            //총을 바꾸는 거면 총 데이터가 있는 거에 데이터 덮어쓰기
            weaponeslot = GameObject.Find("weaponeslot");
            //공격력 변경

            if (itempanel_index.listindex == -1)
            {
                //들어있는게 없을때 새로 입력하는 것의 장비 상태를 1로 수정
                GameObject newitem = equipstartcs.itemobjlist[listindex];
                newitem.GetComponent<iteminfo>().equiped = "1";
                newitem.SetActive(false);

            }
            else {
                //들어있는게 있을때 원래 있던 장비 상태는 0으로 새로 착용하는 것은 1로 수정
                int gunindex = weaponeslot.GetComponent<guninfo>().listindex;
                GameObject olditem = equipstartcs.itemobjlist[gunindex];
                olditem.GetComponent<iteminfo>().equiped = "0";
                olditem.SetActive(true);

                GameObject newitem = equipstartcs.itemobjlist[listindex];
                newitem.GetComponent<iteminfo>().equiped = "1";
                newitem.SetActive(false);
            }

            //위쪽 슬롯의 이미지와 데이터 변경
            RawImage weaponeimage = weaponeslot.GetComponent<RawImage>();
            Texture2D weaponetexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
            weaponeimage.texture = weaponetexture;
            weaponeimage.color = new Color(255,255,255);

            weaponeslot.GetComponent<guninfo>().itemname = itemname;
            weaponeslot.GetComponent<guninfo>().itemtype = itemtype;
            weaponeslot.GetComponent<guninfo>().itemid = itemid;
            weaponeslot.GetComponent<guninfo>().itemdescription = itemdescription;
            weaponeslot.GetComponent<guninfo>().atk = atk;
            weaponeslot.GetComponent<guninfo>().def = def;
            weaponeslot.GetComponent<guninfo>().atkspeed = atkspeed;
            weaponeslot.GetComponent<guninfo>().equiped = equiped;
            weaponeslot.GetComponent<guninfo>().rare = rare;
            weaponeslot.GetComponent<guninfo>().listindex = listindex;

            //총을 바꿨으면 pp의 총id , total 공격력 저장
            PlayerPrefs.SetString("gunid", itemid);

            int totalatk = PlayerPrefs.GetInt("power") + int.Parse(atk);

            PlayerPrefs.SetString("totalpower", totalatk.ToString());

            equipstartcs.totalatk.GetComponent<Text>().text = totalatk.ToString();

            //원래 있던 총 오브젝트 삭제
            GameObject oldgun = GameObject.FindWithTag("gun");
            Destroy(oldgun);

            //새로운 총 오브젝트 만들어서 위치 방향 스케일 적용
            string gunname = "gun" + itemid;
            GameObject weapontest = GameObject.FindWithTag("weaponhand");
            GameObject handgun_test = Resources.Load<GameObject>("Weapone/" + gunname);
            GameObject handgun = Instantiate(handgun_test);
            Vector3 gunpos = new Vector3(0, 0, 0);
            Quaternion gunrot = Quaternion.Euler(0, 90, -90);
            Vector3 gunscale = new Vector3(3, 1, 1);
            handgun.transform.SetParent(weapontest.transform);
            handgun.transform.localScale = gunscale;
            handgun.transform.localPosition = gunpos;
            handgun.transform.localRotation = gunrot;


            equipstartcs.itempanel.SetActive(false);
        }
        else if (itemtype.Equals("armor")) {

            armorslot = GameObject.Find("armorslot");
            //체력 변경
            //itempanel_index.listindex == -1
            if (armorslot.GetComponent<RawImage>().texture == null)
            {
                //원래 아무것도 없으면 장비상태 1로 바꾸고 인벤토리 창에서 사라지게 함
                GameObject newitem = equipstartcs.itemobjlist[listindex];
                newitem.GetComponent<iteminfo>().equiped = "1";
                newitem.SetActive(false);
            }
            else
            {
                //이미 장착한게 있었으면 원래 있던건 장비상태 0으로 새로 장비한건 장비상태 1로 바꾸기
                int armorindex = armorslot.GetComponent<armorinfo>().listindex;
                GameObject olditem = equipstartcs.itemobjlist[armorindex];
                olditem.GetComponent<iteminfo>().equiped = "0";
                olditem.SetActive(true);
                Debug.Log("itempanel :"+itempanel_index.listindex);
                GameObject newitem = equipstartcs.itemobjlist[listindex];
                newitem.GetComponent<iteminfo>().equiped = "1";
                newitem.SetActive(false);
            }

            //위쪽 방어구 슬롯의 이미지와 내용 변경
            RawImage armorimage = armorslot.GetComponent<RawImage>();
            Texture2D armortexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
            armorimage.texture = armortexture;
            armorimage.color = new Color(255, 255, 255);

            armorslot.GetComponent<armorinfo>().itemname = itemname;
            armorslot.GetComponent<armorinfo>().itemtype = itemtype;
            armorslot.GetComponent<armorinfo>().itemid = itemid;
            armorslot.GetComponent<armorinfo>().itemdescription = itemdescription;
            armorslot.GetComponent<armorinfo>().atk = atk;
            armorslot.GetComponent<armorinfo>().def = def;
            armorslot.GetComponent<armorinfo>().atkspeed = atkspeed;
            armorslot.GetComponent<armorinfo>().equiped = equiped;
            armorslot.GetComponent<armorinfo>().rare = rare;
            armorslot.GetComponent<armorinfo>().listindex = listindex;


            //맨 처음 저장된 데이터도 바꾼 장비로 덮어 씌워준다
            PlayerPrefs.SetString("armorid", itemid);

            int totalhp = PlayerPrefs.GetInt("hp") + int.Parse(def);

            PlayerPrefs.SetString("totalhp", totalhp.ToString());

            equipstartcs.totalhp.GetComponent<Text>().text = totalhp.ToString();




            equipstartcs.itempanel.SetActive(false);

        }


    }




}
