using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossreadystart : MonoBehaviour
{


    Vector3 playerpos;

    Quaternion playerrot;

    public GameObject player = null;
    public GameObject playerborn;

    //찾는중
    public Text finding;

    public Text usernickname;

    public Text usergoldtext;

    public GameObject user1slot;
    public GameObject user2slot;
    public GameObject user3slot;
    public GameObject user4slot;

    public GameObject tcpclosebt;

    public GameObject activeskillimage;
    public GameObject passiveskillimage;



    private void Awake()
    {
        user1slot= GameObject.Find("user1slot");
        user2slot = GameObject.Find("user2slot");
        user3slot = GameObject.Find("user3slot");
        user4slot = GameObject.Find("user4slot");
        tcpclosebt = GameObject.Find("tcpclosebutton");
        finding = GameObject.Find("finding").GetComponent<Text>();
        usernickname = GameObject.Find("nicknametext").GetComponent<Text>();
        usergoldtext = GameObject.Find("goldtext").GetComponent<Text>();
        activeskillimage = GameObject.Find("activeskillimage");
        passiveskillimage = GameObject.Find("passiveskillimage");


        tcpclosebt.SetActive(false);
        finding.enabled = false;
        user1slot.SetActive(false);
        user2slot.SetActive(false);
        user3slot.SetActive(false);
        user4slot.SetActive(false);
        activeskillimage.SetActive(false);
        passiveskillimage.SetActive(false);

        roadbossscene();

    }


    public void roadbossscene()
    {
        //내 닉네임을 통해서 캐릭터 생성
        usernickname.text = PlayerPrefs.GetString("nickname");

        playerpos = new Vector3(-106.3f, 219.25f, -80f);

        playerrot = Quaternion.Euler(-90, 180, 0);

        player = Resources.Load<GameObject>("pvpready_player");

        //플레이어 생성
        playerborn = Instantiate(player, playerpos, playerrot);


        string gunid_pp = PlayerPrefs.GetString("gunid");

        string armorid_pp = PlayerPrefs.GetString("armorid");

        string totalpower_pp = PlayerPrefs.GetString("totalpower");

        string totalhp_pp = PlayerPrefs.GetString("totalhp");

        GameObject weaponhand = GameObject.FindWithTag("weaponhand");

        string skill1id = PlayerPrefs.GetString("skill1id");

        string skill2id = PlayerPrefs.GetString("skill2id");

        if (!gunid_pp.Equals("0"))
        {

            //캐릭터가 장비하고 있는 총의 이름을 만듦
            string gunname = "gun" + gunid_pp;

            //총 오브젝트를 만든다
            GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
            GameObject handgun = Instantiate(handgun_ob);


            Vector3 gunpos = new Vector3(0, 0, 0);
            Quaternion gunrot = Quaternion.Euler(0, 90, -90);
            Vector3 gunscale = new Vector3(3, 1, 1);

            //총을 손의 하위로 만든다
            handgun.transform.SetParent(weaponhand.transform);
            //총의 스케일 , 위치 , 방향을 지정해줌
            handgun.transform.localScale = gunscale;
            handgun.transform.localPosition = gunpos;
            handgun.transform.localRotation = gunrot;
        }
        int gold = PlayerPrefs.GetInt("gold");
        string nickname = PlayerPrefs.GetString("nickname");

        GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + gold.ToString();

        GameObject.Find("nicknametext").GetComponent<Text>().text = nickname;

        int skill1id_int;
        int.TryParse(skill1id, out skill1id_int);

        int skill2id_int;
        int.TryParse(skill2id, out skill2id_int);

        /*
        if (skill1id_int != 0) { 
       
            string imagename = "skill" + skill1id_int;
            RawImage skillimage = activeskillimage.GetComponent<RawImage>();
            Texture2D skilltexture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            skillimage.texture = skilltexture;
        }else {

            RawImage skillimage = activeskillimage.GetComponent<RawImage>();
            skillimage.color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

        }

        if(skill2id_int != 0) {

            string imagename2 = "ability" + skill2id_int;
            RawImage skillimage2 = passiveskillimage.GetComponent<RawImage>();
            Texture2D skilltexture2 = Resources.Load<Texture2D>("Skillimage/" + imagename2);
            skillimage2.texture = skilltexture2;

        }
        else {
            RawImage skillimage2 = passiveskillimage.GetComponent<RawImage>();
            skillimage2.color = new Color(159 / 255f, 154 / 255f, 151 / 255f);

        }
        */

        //  yield return null;
    }







}
