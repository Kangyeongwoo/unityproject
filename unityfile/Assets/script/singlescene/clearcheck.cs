using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clearcheck : MonoBehaviour
{
    //클리어 표시 패널
    // public GameObject clearpanel;

    //클리어가 표시되는 텍스트
    //   public Text cleartext;

    //몬스터가 몇마리인지 카운트하기 위한 배열 -> 오픈필드에서는 arraylist가 나은듯
    //  public GameObject[] monsters;
    public GameObject abilitypanel;
    //조이스틱
    public GameObject stick;
    // Update is called once per frame
    public int monstercount;
    public GameObject resultpanel;
    public int chapterclear = 0;

    private void Start()
    {
       
        abilitypanel = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().abilitypanel;
        resultpanel = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().Resultpanel;

    }

    public void itemroot() {

        //몬스터를 다 죽이면 아이템 루팅 

        int stage = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().stage;

        List<GameObject> singleinven = GameObject.Find("singleinventory_cs").GetComponent<singleinventory>().itemtemp;
        var player_hp = GameObject.FindWithTag("Player").GetComponent<playerhpbar_sg>();
        foreach (GameObject item in singleinven) {

            //돈을 임시인벤토리에 담기
            var itemspec  = item.GetComponent<singleiteminfo>();
            Debug.Log(itemspec.itemtype+":"+itemspec.itemnumber);
            if (itemspec.itemtype.Equals("gold")) {

                singleinventory.goldinventory += itemspec.money;
                GameObject.Find("goldtext").GetComponent<Text>().text = "G : "+singleinventory.goldinventory.ToString();
                Destroy(item);
            }
       
            else if (itemspec.itemtype.Equals("passiveskill"))
            {
                //패시브스킬조각을 임시인벤토리에 담기
                int passiveid = itemspec.itemnumber;

                if (singleinventory.passiveskillinventory.ContainsKey(passiveid))
                {
                    int oricount = (int)singleinventory.passiveskillinventory[passiveid];
                    singleinventory.passiveskillinventory[passiveid] = oricount + 1;
                   // singleinventory.passiveskillinventory.Add(passiveid, oricount + 1);
                    Destroy(item);
                }
                else
                {
                    singleinventory.passiveskillinventory.Add(passiveid, 1);
                    Destroy(item);
                }

            }
            else if (itemspec.itemtype.Equals("activeskill"))
            {
                //액티브스킬조각을 임시인벤토리에 담기
                int activeid = itemspec.itemnumber;

                if (singleinventory.activeskillinventory.ContainsKey(activeid))
                {
                    int oricount = (int)singleinventory.activeskillinventory[activeid];
                    singleinventory.activeskillinventory[activeid] = oricount + 1;
                  // singleinventory.activeskillinventory.Add(activeid, oricount + 1);
                    Destroy(item);
                }
                else
                {
                    singleinventory.activeskillinventory.Add(activeid, 1);
                    Destroy(item);
                }

            }

        }
        //스테이지가 종료되면 체력을 유지할 수 있도록 한다
        currentability.currenthpvalue = GameObject.FindWithTag("Player").GetComponent<playerhpbar_sg>().palyerhpbar.value;
        currentability.currenthp = GameObject.FindWithTag("Player").GetComponent<playerhpbar_sg>().playerhp;

        if (stage == 1 || stage == 2)
        {
            //1,2스테이지에서는 2초를 기다리면 패널이 나온다
            Invoke("stageend", 2);
        }
        else if(stage==3){
            //3스테이지를 종료하면 바로 패널이 나온다
            //이건 모든 몬스터가 죽었을때 만이

            //스테이지 3인데 모든 몬스터를 죽인거다
            chapterclear = 1;
            stageend();
        }
    }



    public void stageend()
    {
     
           
            GameObject.FindWithTag("Player").GetComponent<singleplayerfire>().shootbool = 1;
            int stage = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().stage;
            int chapter = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().chapter;
            var passivenamelist = GameObject.Find("dropitem_cs").GetComponent<dropitem>().passiveskill;
            var passivenscript = GameObject.Find("dropitem_cs").GetComponent<dropitem>().passivescript;
        Time.timeScale = 0.0f;



        if (stage == 1 || stage == 2)
        {
       

           abilitypanel.SetActive(true);

            int ab_1;
            int ab_2;
            int ab_3;

            while (true)
            {
                ab_1 = Random.Range(3, 9);
                if (currentability.abilitylist.Contains(ab_1))
                {

                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                ab_2 = Random.Range(3, 9);
                if (currentability.abilitylist.Contains(ab_2) || ab_2 == ab_1)
                {

                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                ab_3 = Random.Range(3, 9);
                if (currentability.abilitylist.Contains(ab_3) || ab_3 == ab_1 || ab_3 == ab_2)
                {

                }
                else
                {
                    break;
                }
            }
            Text skill1text = abilitypanel.transform.GetChild(0).GetComponent<Text>();
            // skill1text.text = ab_1.ToString();
            skill1text.text = passivenamelist[ab_1].ToString();
            abilitypanel.transform.GetChild(1).GetComponent<randomskillid>().randomid = ab_1;
            string imagename1 = "ability" + ab_1;
            Texture2D itemtexture1 = Resources.Load<Texture2D>("Skillimage/" + imagename1);
            abilitypanel.transform.GetChild(1).GetComponent<RawImage>().texture = itemtexture1;
            abilitypanel.transform.GetChild(2).GetComponent<Text>().text= passivenscript[ab_1].ToString();


            Text skill2text = abilitypanel.transform.GetChild(3).GetComponent<Text>();
            //  skill2text.text = ab_2.ToString();
            skill2text.text = passivenamelist[ab_2].ToString();
            abilitypanel.transform.GetChild(4).GetComponent<randomskillid>().randomid = ab_2;
            string imagename2 = "ability" + ab_2;
            Texture2D itemtexture2 = Resources.Load<Texture2D>("Skillimage/" + imagename2);
            abilitypanel.transform.GetChild(4).GetComponent<RawImage>().texture = itemtexture2;
            abilitypanel.transform.GetChild(5).GetComponent<Text>().text = passivenscript[ab_2].ToString();




            Text skill3text = abilitypanel.transform.GetChild(6).GetComponent<Text>();
            //   skill3text.text = ab_3.ToString();
            skill3text.text = passivenamelist[ab_3].ToString();
            abilitypanel.transform.GetChild(7).GetComponent<randomskillid>().randomid = ab_3;
            string imagename3 = "ability" + ab_3;
            Texture2D itemtexture3 = Resources.Load<Texture2D>("Skillimage/" + imagename3);
            abilitypanel.transform.GetChild(7).GetComponent<RawImage>().texture = itemtexture3;
            abilitypanel.transform.GetChild(8).GetComponent<Text>().text = passivenscript[ab_3].ToString();




        }
        else if(stage ==3) {

            resultpanel.SetActive(true);
            GameObject resulttext = resultpanel.transform.GetChild(0).gameObject;
            resulttext.GetComponent<Text>().text = "Clear";

        }

    }








  



}
