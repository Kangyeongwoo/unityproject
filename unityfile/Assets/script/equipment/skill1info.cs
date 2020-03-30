using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class skill1info : MonoBehaviour
{
    public string skillname;

    public string equiped;

    public string skilltype;

    public string skilltype2;

    public string skillid;

    public string skillcount;

    public string skilldescription;

    public string atk;

    public string cooltime;

    public string price;

    public int useritemindex = 0;

    public int listindex;

    public Text skillviewname;

    public RawImage skillviewimage;

    public Text skillviewdescription;

    public Text skillviewspec;

    public GameObject skill1slot;

    public GameObject skill2slot;


    public int slot = 1;

    public void viewactive()
    {
        if (!string.IsNullOrEmpty(skillname))
        {
            //패널을 나타내고 내용과 이미지를 채운다
            GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().skillpanel.SetActive(true);

            skillviewname = GameObject.Find("skillname").GetComponent<Text>();
            skillviewimage = GameObject.Find("skillImage").GetComponent<RawImage>();
            skillviewdescription = GameObject.Find("skilldescription").GetComponent<Text>();
            skillviewspec = GameObject.Find("skillspec").GetComponent<Text>();
            skillviewname.text = skillname;
            skillviewdescription.text = skilldescription;

            //패널에도 지금 보고 있는 아이템의 인덱스를 보내주고 아이템 타입을 보내준다
            //그래야 패널이 장비 해제할때 안보이는 슬롯을 살릴수 있다

            skill_index.listindex = listindex;
            GameObject.Find("skillpanel").GetComponent<skill_index>().skilltype = skilltype;
            GameObject.Find("skillpanel").GetComponent<skill_index>().skilltype2 = skilltype2;
            GameObject.Find("skillpanel").GetComponent<skill_index>().equiped = equiped;
            GameObject.Find("skillpanel").GetComponent<skill_index>().slot = slot;

            //장비슬롯 버튼이 나타나도록 한다
            GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().bt1name.SetActive(false);
            GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().bt2name.SetActive(false);
            GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().bt3name.SetActive(true);

            Debug.Log("패널 누를때skill:" + equiped);

            //스킬 패널에 이미지와 내용 적
            string imagename = skilltype + skillid;
            Texture2D skilltexture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            skillviewimage.texture = skilltexture;



            //총이면 다음의 내용을 표시한다 패널에
            string spec = "공격력:" + atk + "\n";

            skillviewspec.text = spec;

        }


    }



}
