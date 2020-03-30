using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class armorinfo : MonoBehaviour
{
    //방어구 이름
    public string itemname;
    //방어구 장비상태
    public string equiped;
    //아이템이 방어구인지
    public string itemtype;
    //방어구 아이디
    public string itemid;
    //아이템 리스트의 몇번째인지
    public int listindex;
    //아이템설명
    public string itemdescription;
    //아이템공격력
    public string atk;
    //아이템공격속도
    public string atkspeed;
    //아이템방어력
    public string def;
    //아이템레어도
    public string rare;
    public string price;

    //아이템indexid
    public int useritemindex = 0;


    //public string price;
    //무기 슬롯
    public GameObject weaponeslot;
    //패널의 장비이름
    public Text itemviewname;
    //패널의 장비 이미지
    public RawImage itemviewimage;
    //패널의 장비 설명
    public Text itemviewdescription;
    //패널의 장비 상세능력
    public Text itemviewspec;
    //방어구 슬롯
    public GameObject armorslot;



    public void viewactive()
    {
        //방어구 슬롯을 클릭하여 패널을 띄움
        Debug.Log("itemname:"+itemname);
        if (!string.IsNullOrEmpty(itemname))
        {
            Debug.Log("itemnametest" );
            //패널을 나타내고 내용과 이미지를 채운다
            GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itempanel.SetActive(true);

            //패널안에 방어구 이미지 내용 적용
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


            GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itembt1name.SetActive(false);
            GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itembt2name.SetActive(false);
            GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itembt3name.SetActive(true);

            /*
            Text btname = GameObject.Find("equipbttext").GetComponent<Text>();
            if (equiped.Equals("0"))
            {
                Debug.Log("bteq0:" + equiped);
                btname.text = "장비";
            }
            else
            {   
                //위쪽에서 슬롯을 누를때는 장비해제라는 글자가 나타나게 한다
                Debug.Log("bteq1:" + equiped);
                btname.text = "장비해제";
            }
            */


            Debug.Log("패널 누를때gun:" + equiped);

            //이미지를 패널에 적용시킨다
            string imagename = itemtype + itemid;
            Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
            itemviewimage.texture = itemtexture;

            if (itemtype.Equals("gun"))
            {

                //총이면 다음의 내용을 표시한다 패널에
                string spec = "공격력:" + atk + "\n" + "공격속도:" + atkspeed;

                itemviewspec.text = spec;

            }
            else if (itemtype.Equals("armor"))
            {

                //방어구면 다음의 내용을 표시한다 패널에
                string spec = "HP +" + def;
                itemviewspec.text = spec;

            }


        }
    }


}
