using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skill_index : MonoBehaviour
{
    //맨처음에 패널은 listindex = -1을 가지고 있다
    //이따 
    public static int listindex = -1;

    public string skilltype;

    public string skilltype2;

    public string equiped;

    public int slot = 0;

    public int useritemindex = 0;

    public GameObject skill1slot_slot;

    public GameObject skill2slot_slot;

    //아래쪽 skillinfo의 버튼은 두개가 되어 panel이 생성

    public void equipskill1()
    { //스킬 1 등록 버튼 클릭
        var equipstartcs = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();
        Debug.Log("test:"+ skilltype2);
        Debug.Log("equiped:" + equiped);
        if (equiped.Equals("0"))
        {
            Debug.Log("test2");

            //스킬 1번을 등록 (액티브 스킬이)
            if (skilltype2.Equals("active"))
            {
                GameObject newitem = equipstartcs.skillobjlist[listindex];
                if (listindex == -1)
                {

                    //0인데 왼쪽을 누름 -> skill1에 적용 -> equiped =1;
                   // GameObject newitem = equipstartcs.skillobjlist[listindex];
                    newitem.GetComponent<skillinfo>().equiped = "1";
                    newitem.SetActive(false);
                }
                else
                {

                    skill1slot_slot = GameObject.Find("skill1slot");
                    int skill1index_old = skill1slot_slot.GetComponent<skill1info>().listindex;
                    GameObject olditem = equipstartcs.skillobjlist[skill1index_old];
                    olditem.GetComponent<skillinfo>().equiped = "0";
                    olditem.SetActive(true);

                    //0인데 왼쪽을 누름 -> skill1에 적용 -> equiped =1;
                   // GameObject newitem = equipstartcs.skillobjlist[listindex];
                    newitem.GetComponent<skillinfo>().equiped = "1";
                    newitem.SetActive(false);
                }

                string imagename = skilltype + newitem.GetComponent<skillinfo>().skillid;
                GameObject skill1slot = GameObject.Find("skill1slot");
                RawImage skill1image = skill1slot.GetComponent<RawImage>();
                Texture2D skill1texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                skill1image.texture = skill1texture;
                skill1image.color = new Color(255, 255, 255);

                skill1slot.GetComponent<skill1info>().skillname = newitem.GetComponent<skillinfo>().skillname;
                skill1slot.GetComponent<skill1info>().skilltype = skilltype;
                skill1slot.GetComponent<skill1info>().skilltype2 = skilltype2;
                skill1slot.GetComponent<skill1info>().skillid = newitem.GetComponent<skillinfo>().skillid;
                skill1slot.GetComponent<skill1info>().skilldescription = newitem.GetComponent<skillinfo>().skilldescription;
                skill1slot.GetComponent<skill1info>().atk = newitem.GetComponent<skillinfo>().atk;
                skill1slot.GetComponent<skill1info>().skillcount = newitem.GetComponent<skillinfo>().skillcount;
                skill1slot.GetComponent<skill1info>().equiped = newitem.GetComponent<skillinfo>().equiped;
                skill1slot.GetComponent<skill1info>().listindex = listindex;

                PlayerPrefs.SetString("skill1id", newitem.GetComponent<skillinfo>().skillid);

                equipstartcs.skillpanel.SetActive(false);




                //스킬 2 패시브 스킬 적용
            }else if (skilltype2.Equals("passive")) {

                Debug.Log("test3");

                GameObject newitem = equipstartcs.skillobjlist[listindex];
                if (listindex == -1)
                {

                    //0인데 왼쪽을 누름 -> skill1에 적용 -> equiped =1;
                    // GameObject newitem = equipstartcs.skillobjlist[listindex];
                    newitem.GetComponent<skillinfo>().equiped = "1";
                    newitem.SetActive(false);
                }
                else
                {

                    skill2slot_slot = GameObject.Find("skill2slot");
                    int skill2index_old = skill2slot_slot.GetComponent<skill2info>().listindex;
                    GameObject olditem = equipstartcs.skillobjlist[skill2index_old];
                    olditem.GetComponent<skillinfo>().equiped = "0";
                    olditem.SetActive(true);

                    //0인데 왼쪽을 누름 -> skill1에 적용 -> equiped =1;
                    // GameObject newitem = equipstartcs.skillobjlist[listindex];
                    newitem.GetComponent<skillinfo>().equiped = "1";
                    newitem.SetActive(false);
                }


             //  GameObject olditem = equipstartcs.skillobjlist[listindex];
              //  olditem.GetComponent<skillinfo>().equiped = "1";

                string imagename = "ability" + newitem.GetComponent<skillinfo>().skillid;
                GameObject skill2slot = GameObject.Find("skill2slot");
                RawImage skill2image = skill2slot.GetComponent<RawImage>();
                Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                skill2image.texture = skill2texture;
                skill2image.color = new Color(255, 255, 255);

                skill2slot.GetComponent<skill2info>().skillname = newitem.GetComponent<skillinfo>().skillname;
                skill2slot.GetComponent<skill2info>().skilltype = skilltype;
                skill2slot.GetComponent<skill2info>().skilltype2 = skilltype2;
                skill2slot.GetComponent<skill2info>().skillid = newitem.GetComponent<skillinfo>().skillid;
                skill2slot.GetComponent<skill2info>().skilldescription = newitem.GetComponent<skillinfo>().skilldescription;
                skill2slot.GetComponent<skill2info>().atk = newitem.GetComponent<skillinfo>().atk;
                skill2slot.GetComponent<skill2info>().skillcount = newitem.GetComponent<skillinfo>().skillcount;
                skill2slot.GetComponent<skill2info>().equiped = newitem.GetComponent<skillinfo>().equiped;
                skill2slot.GetComponent<skill2info>().listindex = listindex;

                PlayerPrefs.SetString("skill2id", newitem.GetComponent<skillinfo>().skillid);


                newitem.SetActive(false);
                equipstartcs.skillpanel.SetActive(false);

            }
        }
        else {

            Debug.Log("testfail");
        }
        /*
        else if (equiped.Equals("1"))
        {
            //1인데 왼쪽을 누름 -> 이미등록됨;
            equipstartcs.skillpanel.SetActive(false);
        }
        */
        /*
        else if (equiped.Equals("2"))

        {
            //2인데 왼쪽을 누름 -> skill1에 적용 -> equiped =3;
            GameObject olditem = equipstartcs.skillobjlist[listindex];
            olditem.GetComponent<skillinfo>().equiped = "3";

            string imagename = skilltype + olditem.GetComponent<skillinfo>().skillid;
            GameObject skill1slot = GameObject.Find("skill1slot");
            RawImage skill1image = skill1slot.GetComponent<RawImage>();
            Texture2D skill1texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            skill1image.texture = skill1texture;
            skill1image.color = new Color(255, 255, 255);

            skill1slot.GetComponent<skill1info>().skillname = olditem.GetComponent<skillinfo>().skillname;
            skill1slot.GetComponent<skill1info>().skilltype = skilltype;
            skill1slot.GetComponent<skill1info>().skillid = olditem.GetComponent<skillinfo>().skillid;
            skill1slot.GetComponent<skill1info>().skilldescription = olditem.GetComponent<skillinfo>().skilldescription;
            skill1slot.GetComponent<skill1info>().atk = olditem.GetComponent<skillinfo>().atk;
            skill1slot.GetComponent<skill1info>().skillcount = olditem.GetComponent<skillinfo>().skillcount;
            skill1slot.GetComponent<skill1info>().equiped = olditem.GetComponent<skillinfo>().equiped;
            skill1slot.GetComponent<skill1info>().listindex = listindex;

            PlayerPrefs.SetString("skill1id", olditem.GetComponent<skillinfo>().skillid);

            equipstartcs.skillpanel.SetActive(false);
        }
        else if (equiped.Equals("3"))
        {
            //3인데 오른쪽을 누름 -> 이미등록됨;
            equipstartcs.skillpanel.SetActive(false);
        }
        */
    }
    /*
    public void equipskill2()
    {

        //스킬 2 등록 함수

        var equipstartcs = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();

        if (equiped.Equals("0"))
        {
            //0인데 오른쪽을 누름 -> skill2에 적용 -> equiped =2;
            GameObject olditem = equipstartcs.skillobjlist[listindex];
            olditem.GetComponent<skillinfo>().equiped = "2";

            string imagename = skilltype + olditem.GetComponent<skillinfo>().skillid;
            GameObject skill2slot = GameObject.Find("skill2slot");
            RawImage skill2image = skill2slot.GetComponent<RawImage>();
            Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            skill2image.texture = skill2texture;
            skill2image.color = new Color(255, 255, 255);

            skill2slot.GetComponent<skill2info>().skillname = olditem.GetComponent<skillinfo>().skillname;
            skill2slot.GetComponent<skill2info>().skilltype = skilltype;
            skill2slot.GetComponent<skill2info>().skillid = olditem.GetComponent<skillinfo>().skillid;
            skill2slot.GetComponent<skill2info>().skilldescription = olditem.GetComponent<skillinfo>().skilldescription;
            skill2slot.GetComponent<skill2info>().atk = olditem.GetComponent<skillinfo>().atk;
            skill2slot.GetComponent<skill2info>().skillcount = olditem.GetComponent<skillinfo>().skillcount;
            skill2slot.GetComponent<skill2info>().equiped = olditem.GetComponent<skillinfo>().equiped;
            skill2slot.GetComponent<skill2info>().listindex = listindex;

            PlayerPrefs.SetString("skill2id", olditem.GetComponent<skillinfo>().skillid);

            equipstartcs.skillpanel.SetActive(false);
        }
        else if (equiped.Equals("1"))
        {
            //1인데 오른쪽을 누름 -> skill2에 적용 -> equiped =3 ;
            GameObject olditem = equipstartcs.skillobjlist[listindex];
            olditem.GetComponent<skillinfo>().equiped = "3";

            string imagename = skilltype + olditem.GetComponent<skillinfo>().skillid;
            GameObject skill2slot = GameObject.Find("skill2slot");
            RawImage skill2image = skill2slot.GetComponent<RawImage>();
            Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            skill2image.texture = skill2texture;
            skill2image.color = new Color(255, 255, 255);

            skill2slot.GetComponent<skill2info>().skillname = olditem.GetComponent<skillinfo>().skillname;
            skill2slot.GetComponent<skill2info>().skilltype = skilltype;
            skill2slot.GetComponent<skill2info>().skillid = olditem.GetComponent<skillinfo>().skillid;
            skill2slot.GetComponent<skill2info>().skilldescription = olditem.GetComponent<skillinfo>().skilldescription;
            skill2slot.GetComponent<skill2info>().atk = olditem.GetComponent<skillinfo>().atk;
            skill2slot.GetComponent<skill2info>().skillcount = olditem.GetComponent<skillinfo>().skillcount;
            skill2slot.GetComponent<skill2info>().equiped = olditem.GetComponent<skillinfo>().equiped;
            skill2slot.GetComponent<skill2info>().listindex = listindex;

            PlayerPrefs.SetString("skill2id", olditem.GetComponent<skillinfo>().skillid);

            equipstartcs.skillpanel.SetActive(false);
        }
        else if (equiped.Equals("2"))
        {
            //2인데 오른쪽을 누름 -> 이미등록됨;
            equipstartcs.skillpanel.SetActive(false);
        }
        else if (equiped.Equals("3"))
        {
            //3인데 오른쪽을 누름 -> 이미등록됨;
            equipstartcs.skillpanel.SetActive(false);
        }


    }
    */

    //위쪽 skill1info,skill2info의 버튼은 한개가 되어 panel이 생성

        //
    public void equipedcancel() {
        //스킬해제 버튼 클릭
        var equipstartcs = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();

        Debug.Log("equiped.Equals :"+equiped);
        Debug.Log("skilltype2.Equals :" + skilltype2);

        if (equiped.Equals("1"))
        {
            if (skilltype2.Equals("active"))
            {
                //위쪽에 있는 걸 눌러서 왼쪽 버튼 누르는 것
                //skill1slot 초기화 ,   equiped =0;
                GameObject olditem = equipstartcs.skillobjlist[listindex];
                olditem.GetComponent<skillinfo>().equiped = "0";

                GameObject skill1slot = GameObject.Find("skill1slot");
                skill1slot.GetComponent<skill1info>().skillname = null;
                skill1slot.GetComponent<skill1info>().atk = "0";
                skill1slot.GetComponent<RawImage>().texture = null;
                skill1slot.GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

                listindex = -1;

                PlayerPrefs.SetString("skill1id", "0");
                olditem.SetActive(true);
                equipstartcs.skillpanel.SetActive(false);

           
             }else if (skilltype2.Equals("passive")) {

                GameObject olditem = equipstartcs.skillobjlist[listindex];
                olditem.GetComponent<skillinfo>().equiped = "0";


                GameObject skill2slot = GameObject.Find("skill2slot");
                skill2slot.GetComponent<skill2info>().skillname = null;
                skill2slot.GetComponent<skill2info>().atk = "0";
                skill2slot.GetComponent<RawImage>().texture = null;
                skill2slot.GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

                listindex = -1;

                PlayerPrefs.SetString("skill2id", "0");
                olditem.SetActive(true);
                equipstartcs.skillpanel.SetActive(false);


            }

        }
        /*
        else if (equiped.Equals("2"))
        {
            //위쪽에 있는 걸 눌러서 왼쪽 버튼 누르는 것
            //skill2slot 초기화 ,   equiped =0;
            GameObject olditem = equipstartcs.skillobjlist[listindex];
            olditem.GetComponent<skillinfo>().equiped = "0";


            GameObject skill2slot = GameObject.Find("skill2slot");
            skill2slot.GetComponent<skill2info>().skillname = null;
            skill2slot.GetComponent<skill2info>().atk = "0";
            skill2slot.GetComponent<RawImage>().texture = null;
            skill2slot.GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

            listindex = -1;

            PlayerPrefs.SetString("skill2id", "0");

            equipstartcs.skillpanel.SetActive(false);

        }
        */       
        /*
        else if (equiped.Equals("3"))
        {
            //slot1,slot2 판별
            //위쪽에 있는 걸 눌러서 왼쪽 버튼 누르는 것
            //skill1slot 초기화 or 2slot 초기화  , 1초기화 이면 equiped = 2/ 2초기화 이면 equiped = 1;
            if (slot == 1) {

                GameObject olditem = equipstartcs.skillobjlist[listindex];
                olditem.GetComponent<skillinfo>().equiped = "2";

                GameObject skill1slot = GameObject.Find("skill1slot");
                skill1slot.GetComponent<skill1info>().skillname = null;
                skill1slot.GetComponent<skill1info>().atk = "0";
                skill1slot.GetComponent<RawImage>().texture = null;
                skill1slot.GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);


                listindex = -1;

                PlayerPrefs.SetString("skill1id", "0");

                equipstartcs.skillpanel.SetActive(false);
            }
            else if(slot == 2) {

                GameObject olditem = equipstartcs.skillobjlist[listindex];
                olditem.GetComponent<skillinfo>().equiped = "1";
                GameObject skill2slot = GameObject.Find("skill2slot");
                skill2slot.GetComponent<skill2info>().skillname = null;
                skill2slot.GetComponent<skill2info>().atk = "0";
                skill2slot.GetComponent<RawImage>().texture = null;
                skill2slot.GetComponent<RawImage>().color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

                listindex = -1;

                PlayerPrefs.SetString("skill2id", "0");

                equipstartcs.skillpanel.SetActive(false);

            }

        }
        */

    }



    public void sellskill()
    {


        var equipstartcs = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();

        GameObject olditem = equipstartcs.skillobjlist[listindex];

        if (skilltype2.Equals("active"))
        {
            if (olditem.GetComponent<skillinfo>().useritemindex != useritemindex)
            {
                int sellgold;
                int.TryParse(olditem.GetComponent<skillinfo>().price, out sellgold);

                //서버 전송용
                equipstartcs.resultgold += sellgold;

                GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + equipstartcs.resultgold.ToString();

                //goldtext
                PlayerPrefs.SetInt("gold", equipstartcs.resultgold);
                equipstartcs.sellskilllist.Add(useritemindex);

                Destroy(olditem);

                equipstartcs.skillpanel.SetActive(false);
            }
            else
            {
                //새로 만든거임
                string skillid = olditem.GetComponent<skillinfo>().skillid;

                int skillid_int;
                int.TryParse(skillid, out skillid_int);

                for(int i=0; i < equipstartcs.newskilllist.Count; i++) {


                    int test = equipstartcs.newskilllist[i];

                    if(test == skillid_int) {
                        equipstartcs.newskilllist.RemoveAt(i);
                        break;
                    }

                }

                Destroy(olditem);
                equipstartcs.skillpanel.SetActive(false);
            }


        }
        else if (skilltype2.Equals("passive"))
        {
            if (olditem.GetComponent<skillinfo>().useritemindex != useritemindex)
            {

                int sellgold;
            int.TryParse(olditem.GetComponent<skillinfo>().price, out sellgold);

            //서버 전송용
            equipstartcs.resultgold += sellgold;

            GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + equipstartcs.resultgold.ToString();

            //goldtext

            PlayerPrefs.SetInt("gold", equipstartcs.resultgold);
            equipstartcs.sellskilllist.Add(useritemindex);

            Destroy(olditem);

            equipstartcs.skillpanel.SetActive(false);

            }
            else
            {
                //새로 만든거임
                //새로 만든거임
                string skillid = olditem.GetComponent<skillinfo>().skillid;

                int skillid_int;
                int.TryParse(skillid, out skillid_int);

                for (int i = 0; i < equipstartcs.newskilllist.Count; i++)
                {


                    int test = equipstartcs.newskilllist[i];

                    if (test == skillid_int)
                    {
                        equipstartcs.newskilllist.RemoveAt(i);
                        break;
                    }

                }
                Destroy(olditem);
                equipstartcs.skillpanel.SetActive(false);
            }
        }








    }





}
