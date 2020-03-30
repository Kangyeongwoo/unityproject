using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillpart_index : MonoBehaviour
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


    public GameObject skillslot;

    public List<GameObject> skillobjlist;

    public List<GameObject> skillpartobjlist;

    public GameObject Skillinventory;


    public List<int> newskilllist;

    public Hashtable sellskillpartlist;


    public void combine()
    {


        var start = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();
        newskilllist = start.newskilllist;


        sellskillpartlist = start.sellskillpartlist;



        Debug.Log("combine");

        int skillcount_int;
        int.TryParse(skillcount, out skillcount_int);


        //스킬 조각이 10개 이상이면 스킬을 만들 수 있다
        if (skillcount_int >= 10)
        {

            int skillid_int;
            int.TryParse(skillid, out skillid_int);

            //새로 추가된 아이템을 list에 넣는다
            start.newskilllist.Add(skillid_int);




            skillobjlist = start.skillobjlist;

            skillpartobjlist = start.skillpartobjlist;

            Skillinventory = start.Skillinventory;

            //인벤토리에 프리팹을 만들고 스크립트에 내용입, 이미지 적용
            skillslot = Resources.Load<GameObject>("skillslot");

            //아이템 슬롯을 오브젝트로 만들기
            GameObject skill = Instantiate(skillslot);

            //아이템 리스트로 만들기
            skillobjlist.Add(skill);

            // GameObject.Find("itemslot(Clone)").name = useritemData["itemid"];
            // GameObject.Find(useritemData["itemid"]).transform.SetParent(Iteminventory.transform);
            // GameObject.Find(useritemData["itemid"]).transform.localScale = new Vector3(1, 1, 1);
            // GameObject.Find(useritemData["itemid"]).GetComponent<iteminfo>().atk = "10"; 

            //스킬슬롯을 스킬 인벤토리의 하위로 만든다
            skill.transform.SetParent(Skillinventory.transform);
            skill.transform.localScale = new Vector3(1, 1, 1);

            // test.transform.GetComponent<iteminfo>().atk = "10";


            string imagename = skillid;
            if (skilltype2.Equals("active"))
            {

                imagename = "skill" + skillid;


            }
            else if (skilltype2.Equals("passive"))
            {

                imagename = "ability" + skillid;

            }



            //스킬 인벤토리의 슬롯에 이미지를 적용한다
            RawImage skillimage = skill.GetComponent<RawImage>();
            Texture2D skilltexture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            skillimage.texture = skilltexture;
            // skillimage.color = new Color(255, 255, 255);

            //스킬 인벤토리에 개수를 표시 -> 이건 파편으로 바꾸고 재료 인벤토리에서 처리하기

            skill.GetComponent<skillinfo>().skillname = skillname;
            skill.GetComponent<skillinfo>().skilltype = skilltype;
            skill.GetComponent<skillinfo>().skilltype2 = skilltype2;
            skill.GetComponent<skillinfo>().skillid = skillid;
            skill.GetComponent<skillinfo>().skilldescription = skilldescription;
            skill.GetComponent<skillinfo>().atk = atk;
            skill.GetComponent<skillinfo>().skillcount = skillcount;
            skill.GetComponent<skillinfo>().equiped = equiped;
            skill.GetComponent<skillinfo>().listindex = skillobjlist.Count - 1;
            skill.GetComponent<skillinfo>().price = price;
            skill.GetComponent<skillinfo>().useritemindex = useritemindex;

            //새로 생긴 스킬은 얻은 아이템 리스트에 저장 skillid, 


            //개수 바꿔주기
            GameObject olditem = skillpartobjlist[listindex];





            //몇개 팔고 합성하는거
            if (start.sellskillpartlist.ContainsKey(useritemindex))
            {
                int originalcount;
                string oricount = start.sellskillpartlist[useritemindex].ToString();
                int.TryParse(oricount, out originalcount);
                int newcount = originalcount + 10;

                start.sellskillpartlist[useritemindex] = newcount;

                //  Debug.Log("sellpart1:"+start.sellskillpartlist[useritemindex]);
            }
            else
            {


                start.sellskillpartlist.Add(useritemindex, 10);
                //  Debug.Log("sellpart2:" + start.sellskillpartlist[useritemindex]);
            }





            int resultcount = skillcount_int - 10;


            //결과 값이 0일때는 값을 보내지 않고 리스트에서 삭제만 한다
            if (resultcount == 0)
            {
                Destroy(olditem);
                //사라진건 리스트로 저장 useritemindex를 저장하자
                //db에서 같은 useritemindex를 삭제

            }
            else
            {

                olditem.GetComponent<skillpartinfo>().skillcount = resultcount.ToString();
                olditem.transform.GetChild(1).gameObject.GetComponent<Text>().text = resultcount.ToString();




            }

            start.skillpartpanel.SetActive(false);

        }






    }


    //스킬 패널을 끄는 함수
    public void skillpartcancel()
    {


        var start = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();
        start.skillpartpanel.SetActive(false);

    }


    //스킬파는 패널로 넘어가 패널 생성
    public void skillpartsell()
    {

        var start = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();
        start.skillpartnumberpanel.SetActive(true);

       
        GameObject.Find("skillsellcount").GetComponent<Text>().text = "보유수량: " + skillcount;

    }

    //스킬 판매 수량 정하는 패널
    public void skillpartsell2() {

        var start = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();

        string sellcount = GameObject.Find("sellcount").GetComponent<InputField>().text;

        int sellcount_int;
        int.TryParse(sellcount, out sellcount_int);


        int skillcount_int;
        int.TryParse(skillcount, out skillcount_int);


        if (sellcount_int <= 0) {

            GameObject.Find("counterror").GetComponent<Text>().text = "수량을 입력하십시오";

        }
        else if (sellcount_int > skillcount_int) {

            GameObject.Find("counterror").GetComponent<Text>().text = "보유한 수량을 초과했습니다";

        }
        else {
            //판매가능한 수량이면 계산해서 서버로 보내기
            int resultcount = skillcount_int - sellcount_int;


                //개수 바꿔주기
                GameObject olditem = start.skillpartobjlist[listindex];


                //몇개 팔고 합성하는거
                if (start.sellskillpartlist.ContainsKey(useritemindex))
                {
                    int originalcount;
                    string oricount = start.sellskillpartlist[useritemindex].ToString();
                    int.TryParse(oricount, out originalcount);
                    int newcount = originalcount + sellcount_int;

                    start.sellskillpartlist[useritemindex] = newcount;

                    //  Debug.Log("sellpart1:"+start.sellskillpartlist[useritemindex]);
                }
                else
                {


                    start.sellskillpartlist.Add(useritemindex, sellcount_int);
                    //  Debug.Log("sellpart2:" + start.sellskillpartlist[useritemindex]);
                }





            if (resultcount == 0)
            {

                Destroy(olditem);


            }
            else
            {

                olditem.GetComponent<skillpartinfo>().skillcount = resultcount.ToString();
                olditem.transform.GetChild(1).gameObject.GetComponent<Text>().text = resultcount.ToString();

            }



            int sellgold;
            int.TryParse(olditem.GetComponent<skillpartinfo>().price, out sellgold);

            int sumsellgold = sellgold * sellcount_int;


            //서버 전송용
            start.resultgold += sumsellgold;

            PlayerPrefs.SetInt("gold", start.resultgold);

            GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + start.resultgold.ToString();


            sellcount = "";

            start.skillpartnumberpanel.SetActive(false);
            if(resultcount == 0) {
                start.skillpartpanel.SetActive(false);
            }
            else {

                start.skillpartpanel.SetActive(false);
            }

        }







    }

    //스킬파트를 파는 패널을 닫
    public void skillpartsellcancel() {

        var start = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();

        string sellcount = GameObject.Find("sellcount").GetComponent<InputField>().text;
        sellcount = "";
        start.skillpartnumberpanel.SetActive(false);


    }

}
