using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Pvpdata;
using FlatBuffers;

public class monsterhpbar : MonoBehaviour
{
    //몬스터 체력바에 나타나는 체력
    public Text monhp_text;

    //몬스터의 체력바 
    public Slider monhpbar;

    public int monsterhp;
    public int monstermaxhp;
    public int monstermoney;
    // public static NetworkStream stream = Findenemy_flat.stream;
    public GameObject enemyfac;
    public GameObject enemy;
    //  public static int skillatkedcount;
    public int deathcount = 0;
    private void Start()
    {
      //  skillatkedcount = 0;
       

        int stage = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().stage;

        //보스몬스터 체력바는 캔버스에서 참조한다
         if (stage == 3) {
          GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().bossmonsterhpfac.SetActive(true);
             monhpbar = GameObject.Find("monsterhpbar").GetComponent<Slider>();

             monhp_text = GameObject.Find("enemyhptext").GetComponent<Text>();
         }
        var monsterdata = enemy.GetComponent<singlemonsterinfo>();
        monsterhp = monsterdata.monsterhp;
        monstermaxhp = monsterdata.monstermaxhp;
        monhp_text.text = monsterhp.ToString();
        monstermoney = monsterdata.monstermoney;

    }

    void OnTriggerEnter(Collider other)
    // void OnCollisionEnter(Collision other)
    {
        //other는 부딪힌 오브젝트 벽 또는 적에 맞으면 총알이 사라진다
        if (other.gameObject.tag == "Playerbullet" )
        {

            var player = GameObject.FindWithTag("Player").GetComponent<singleplayerinfo>();

            //몬스터의 체력은 플레이어 공격력 만큼 감소
            monsterhp = monsterhp - player.playerpower;

            string hp = monsterhp.ToString();
            monhp_text.text = hp;

            Debug.Log("player_hp: " + monsterhp);

        }

        if (other.gameObject.tag == "skill")
        {

            var player = GameObject.FindWithTag("Player").GetComponent<singleplayerinfo>();

            //몬스터의 체력은 플레이어 공격력 만큼 감소
            monsterhp = monsterhp - player.playerskillatk;

            string hp = monsterhp.ToString();
            monhp_text.text = hp;

            Debug.Log("player_hp: " + monsterhp);

        }

        Debug.Log("playerhpdown 2 :" + other.gameObject.tag);
        if (monsterhp <= 0)
        {
            if (deathcount == 0)
            {
                //슬라이더 색이 어디 까지 차있는지 정할 수 있음
                monsterhp = 0;
                string hp = monsterhp.ToString();
                monhp_text.text = hp;
                monhpbar.value = 0;


                itemdrop();

                GameObject.Find("Monsterspawn").GetComponent<mosterspawn>().monstercount -= 1;

                Debug.Log("monstercount:" + GameObject.Find("Monsterspawn").GetComponent<mosterspawn>().monstercount);

                if (GameObject.Find("Monsterspawn").GetComponent<mosterspawn>().monstercount == 0)
                {
                    Destroy(enemyfac);
                    GameObject.Find("clearcheck").GetComponent<clearcheck>().itemroot();

                }
                else
                {

                    Destroy(enemyfac);
                }
                deathcount += 1;
            }


        }
        else
        {
            //분수로 표현 가능
            monhpbar.value = (float)monsterhp / (float)monstermaxhp;
        }
    }



    public void itemdrop() {


        GameObject hprestore;
        GameObject passiveskill;
        GameObject activeskill;
        GameObject gold;
        List<GameObject> singleinven =GameObject.Find("singleinventory_cs").GetComponent<singleinventory>().itemtemp;

        GameObject goldobj = Resources.Load<GameObject>("itemobj/goldobj");
        gold = Instantiate(goldobj, enemy.transform.position, Quaternion.identity);
        gold.GetComponent<singleiteminfo>().itemtype = "gold";
        gold.GetComponent<singleiteminfo>().money = monstermoney;
        singleinven.Add(gold);

        int hpchance = Random.Range(1, 10);
        if (hpchance > 6)
        {
            GameObject hprestoreobj = Resources.Load<GameObject>("itemobj/hprestore");
            Vector3 hppos = new Vector3(enemy.transform.position.x+0.6f, enemy.transform.position.y, enemy.transform.position.z+0.6f);
            hprestore = Instantiate(hprestoreobj, hppos, Quaternion.identity);
            hprestore.GetComponent<singleiteminfo>().itemtype = "hprestore";
           // singleinven.Add(hprestore);
        }


        int passiveskillchance = Random.Range(1, 10);
        if (passiveskillchance > 5)
        {
            GameObject passiveskillobj = Resources.Load<GameObject>("itemobj/passiveskillobj");
            Vector3 itempos = new Vector3(enemy.transform.position.x - 0.6f, enemy.transform.position.y, enemy.transform.position.z + 0.6f);
            Quaternion itemrot = Quaternion.Euler(45,45,45);
            passiveskill = Instantiate(passiveskillobj, itempos, itemrot);
            passiveskill.GetComponent<singleiteminfo>().itemtype = "passiveskill";
            int passiveskillidchance = Random.Range(3, 8);
            passiveskill.GetComponent<singleiteminfo>().itemnumber = passiveskillidchance;
            singleinven.Add(passiveskill);

             
        }


        int activeskillchance = Random.Range(1, 10);
        if (activeskillchance > 7)
        {
            GameObject activeskillobj = Resources.Load<GameObject>("itemobj/activeskillobj");
            Vector3 itempos = new Vector3(enemy.transform.position.x - 0.6f, enemy.transform.position.y, enemy.transform.position.z - 0.6f);
            Quaternion itemrot = Quaternion.Euler(45, 45, 45);
            activeskill= Instantiate(activeskillobj, itempos, itemrot);
            activeskill.GetComponent<singleiteminfo>().itemtype = "activeskill";
            int activeskillidchance = Random.Range(1, 2);
            activeskill.GetComponent<singleiteminfo>().itemnumber = activeskillidchance;
            singleinven.Add(activeskill);

        }


    }



}
