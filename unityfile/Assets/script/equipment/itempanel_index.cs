using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itempanel_index : MonoBehaviour
{
    //맨처음에 패널은 listindex = -1을 가지고 있다
    //이따 
    public static int listindex = -1 ;

    public string itemtype ;

    public string equiped ;

    public int useritemindex = 0;

    public void equip()
   {

        var equipstartcs = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();

        //아래쪽을 리스트를 눌러서 패널을 열었다
        if (equiped.Equals("0"))
        {
            //패널에서 장비 버튼을 클리하면
            if (itemtype.Equals("gun")|| itemtype.Equals("armor"))
        {
            //갑옷이나 무기의 change함수가 시작 -> change함수로 지금 패널에 어떤 아이템이 등록 되어 있는지에 따라 
            //아이템리스트에서 그 아이템을 확인하고 change하게 된다 -> 기존에 등록된 아이템이 있다면 교체
            //지금 선택한 무기가 총인데 기존에 등록된 있던 총이 없다 , 총을 골랐는데 총이 있다
            //갑옷을 선택했는데 갑옷이 있다 , 갑옷을 선택했는데 갑옷이 없다 

                GameObject olditem = equipstartcs.itemobjlist[listindex];

                olditem.GetComponent<iteminfo>().changeitem();

        }





        }

        //위쪽 슬롯을 눌러 패널을 열었다 장비해제 버튼을 눌렀다
        else if(equiped.Equals("1"))
        {
            //총을 장비 해제 할때
            if (itemtype.Equals("gun"))
            {
                //총 정보에 데이터를 삭제해 아무것도 안 뜨게 한다
                //총 공격력 변경
                GameObject weaponeslot = GameObject.Find("weaponeslot");
                weaponeslot.GetComponent<guninfo>().itemname = null;
                weaponeslot.GetComponent<guninfo>().atk = "0";
                //weaponeslot.GetComponent<guninfo>().atkspeed
              // weaponeslot.GetComponent<RawImage>().color = new Color(159, 154, 151);
                weaponeslot.GetComponent<RawImage>().texture = null;
                weaponeslot.GetComponent<RawImage>().color = new Color(159/255f, 154/255f, 151/255f);

                GameObject olditem = equipstartcs.itemobjlist[listindex];

                //장비 해제 시 원래 있던 아이템 장비상태 0으로 변경
                olditem.GetComponent<iteminfo>().equiped="0";

                olditem.SetActive(true);

                listindex = -1;

                //처음 저장한 데이터도 바꿔 준다
                PlayerPrefs.SetString("gunid", "0");

                PlayerPrefs.SetString("totalpower", PlayerPrefs.GetInt("power").ToString());

                equipstartcs.totalatk.GetComponent<Text>().text = PlayerPrefs.GetInt("power").ToString();

                //장비하고 있던 아이템 삭제
                GameObject oldgun = GameObject.FindWithTag("gun");
                Destroy(oldgun);

                equipstartcs.itempanel.SetActive(false);

               

            }
            else if (itemtype.Equals("armor")) {
                //갑옷을 장비 해제 할때
                //총 체력 변경
                GameObject armorslot = GameObject.Find("armorslot");
                armorslot.GetComponent<armorinfo>().itemname = null;
                armorslot.GetComponent<armorinfo>().def = "0";
                armorslot.GetComponent<RawImage>().texture = null;
                armorslot.GetComponent<RawImage>().color = new Color(159/255f, 154/255f, 151/255f);

                GameObject olditem = equipstartcs.itemobjlist[listindex];


                //원래 있던 아이템의 장비상태 0으로 수정
                olditem.GetComponent<iteminfo>().equiped = "0";

                olditem.SetActive(true);

                listindex = -1;

                //처음에 저장되었던 데이터도 삭제 
                PlayerPrefs.SetString("armorid", "0");

                PlayerPrefs.SetString("totalhp", PlayerPrefs.GetInt("hp").ToString());

                equipstartcs.totalhp.GetComponent<Text>().text = PlayerPrefs.GetInt("hp").ToString();


                equipstartcs.itempanel.SetActive(false);



            }



        }


    }

    //아이템 팔때 발동하는 함수 아이템 판매 버튼에 적용
    public void sellitem() {


        var equipstartcs = GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>();

        GameObject olditem = equipstartcs.itemobjlist[listindex];

        //아이템의 타입에 따라서 다르게 적용 된
        if (itemtype.Equals("gun"))
        {
            int sellgold;
            int.TryParse(olditem.GetComponent<iteminfo>().price, out sellgold);

            //서버 전송용
            equipstartcs.resultgold += sellgold;

            PlayerPrefs.SetInt("gold",equipstartcs.resultgold);

            GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + equipstartcs.resultgold.ToString();

            //goldtext

            Destroy(olditem);

            equipstartcs.sellitemlist.Add(useritemindex);

            equipstartcs.itempanel.SetActive(false);
        }
        else if (itemtype.Equals("armor"))
        {
            int sellgold;
            int.TryParse(olditem.GetComponent<iteminfo>().price, out sellgold);

            //서버 전송용
            equipstartcs.resultgold += sellgold;

            GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + equipstartcs.resultgold.ToString();

            //goldtext
            PlayerPrefs.SetInt("gold", equipstartcs.resultgold);

            Destroy(olditem);

            equipstartcs.sellitemlist.Add(useritemindex);

            equipstartcs.itempanel.SetActive(false);

        }





   
   
   
   }





}
