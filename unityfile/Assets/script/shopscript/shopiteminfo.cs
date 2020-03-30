using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopiteminfo : MonoBehaviour
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

    public Text itemviewname;

    public RawImage itemviewimage;

    public Text itemviewdescription;

    public Text itemviewspec;

    public Text buybttext;



    // Start is called before the first frame update
    public void viewitempanel()
    {
        var shoppanel = GameObject.Find("shopscenestart_cs").GetComponent<Shopscenestart>().shopitempanel;
        shoppanel.SetActive(true);

        itemviewname = GameObject.Find("itemname").GetComponent<Text>();
        itemviewimage = GameObject.Find("itemImage").GetComponent<RawImage>();
        itemviewdescription = GameObject.Find("itemdescription").GetComponent<Text>();
        itemviewspec = GameObject.Find("itemspec").GetComponent<Text>();
        itemviewname.text = itemname;
        itemviewdescription.text = itemdescription;
        buybttext = GameObject.Find("buybttext").GetComponent<Text>();

        string buytext = price + " G\n\n" + "구매";
        buybttext.text = buytext;

        string imagename = itemtype + itemid;
        Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
        itemviewimage.texture = itemtexture;

        if (itemtype.Equals("gun"))
        {

            //총이면 다음의 내용을 표시한다 패널에
            // string spec = "공격력 +" + atk + "\n"+"공격속도:"+atkspeed;
            string spec = "공격력 +" + atk;
            itemviewspec.text = spec;

        }
        else if (itemtype.Equals("armor"))
        {

            //방어구면 다음의 내용을 표시한다 패널에
            string spec = "HP +" + def;
            itemviewspec.text = spec;

        }

        var shoppanelinfo = shoppanel.GetComponent<shoppanelinfo>();

        shoppanelinfo.itemname = itemname;
        shoppanelinfo.itemtype = itemtype;
        shoppanelinfo.itemid = itemid;
        shoppanelinfo.price = price;

    }


   
}
