using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class guninfo : MonoBehaviour
{
    public string itemname = null;

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


    public GameObject weaponeslot;

    public Text itemviewname;

    public RawImage itemviewimage;

    public Text itemviewdescription;

    public Text itemviewspec;

    public GameObject armorslot;

    public void viewactive()
    {
        //무기를 눌렀을때 나오는 패널
        if (!string.IsNullOrEmpty(itemname)) {
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
                Debug.Log("bteq1:" + equiped);
                btname.text = "장비해제";
            }
            */
           

            Debug.Log("패널 누를때gun:" + equiped);


            //패널에 이미지 적용
            string imagename = itemtype + itemid;
            Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
            itemviewimage.texture = itemtexture;

            if (itemtype.Equals("gun"))
            {

                //총이면 다음의 내용을 표시한다 패널에
               // string spec = "공격력 +" + atk + "\n" + "공격속도:" + atkspeed;
                string spec = "공격력 +" + atk;

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
