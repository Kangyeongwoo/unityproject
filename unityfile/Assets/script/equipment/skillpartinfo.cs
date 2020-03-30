using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillpartinfo : MonoBehaviour
{
    public string skillname;

    public string skilltype;

    public string skilltype2;

    public string skillid;

    public string skillcount;

    public string skilldescription;

    public string atk;

    public string cooltime;

    public string equiped;

    public string price;

    public int listindex;

    public int useritemindex = 0;



    public Text skillpartviewname;

    public RawImage skillpartviewimage;

    public Text skillpartviewdescription;

    public Text skillpartviewspec;

    public Text skillpartviewcount;

    public void viewactive()
    {

        //패널을 나타내고 내용과 이미지를 채운다

        var skillpartpanel  =   GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().skillpartpanel;
        skillpartpanel.SetActive(true);

        skillpartpanel.GetComponent<skillpart_index>().skilltype2 = skilltype2;
        skillpartpanel.GetComponent<skillpart_index>().skillid = skillid;
        skillpartpanel.GetComponent<skillpart_index>().skillcount = skillcount;
        skillpartpanel.GetComponent<skillpart_index>().listindex = listindex;
        skillpartpanel.GetComponent<skillpart_index>().skillname = skillname;
        skillpartpanel.GetComponent<skillpart_index>().atk = atk;
        skillpartpanel.GetComponent<skillpart_index>().cooltime = cooltime;
        skillpartpanel.GetComponent<skillpart_index>().equiped = equiped;
        skillpartpanel.GetComponent<skillpart_index>().price = price;
        skillpartpanel.GetComponent<skillpart_index>().useritemindex = useritemindex;



        skillpartviewname = GameObject.Find("skillpartname").GetComponent<Text>();
        skillpartviewimage = GameObject.Find("skillpartImage").GetComponent<RawImage>();
        skillpartviewdescription = GameObject.Find("skillpartdescription").GetComponent<Text>();
        skillpartviewspec = GameObject.Find("skillpartspec").GetComponent<Text>();
        skillpartviewcount = GameObject.Find("skillpartcount").GetComponent<Text>();

        skillpartviewname.text = skillname;
        skillpartviewdescription.text = skilldescription;

        //패널에도 지금 보고 있는 아이템의 인덱스를 보내주고 아이템 타입을 보내준다
        //그래야 패널이 장비 해제할때 안보이는 슬롯을 살릴수 있다

        string fullcount = skillcount + "/10";
        skillpartviewcount.text = fullcount;
        Debug.Log("fullcount:" + fullcount);
        Debug.Log("skillpartviewcount:" + skillpartviewcount.name);

        string imagename = "";

        if (skilltype2.Equals("active")) {

            imagename = "skill" + skillid;


        }
        else if (skilltype2.Equals("passive")) {

            imagename = "ability" + skillid;


        }

        Texture2D itemtexture = Resources.Load<Texture2D>("skillimage/" + imagename);
        skillpartviewimage.texture = itemtexture;

        skillpartviewspec.text = "10개의 조각으로 스킬을 합성할 수 있습니다";

      //  string count = skillcount.ToString();

    }



}
