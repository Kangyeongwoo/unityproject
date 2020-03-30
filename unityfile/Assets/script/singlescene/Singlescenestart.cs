using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using FlatBuffers;
using Pvpdata;
using System;

public class Singlescenestart : MonoBehaviour
{

    public GameObject joystick;
  // public GameObject remotecontrol;
    public GameObject player = null;
  //  public GameObject enemy = null;
    public Transform maincamera;
    //   public static int enemyindex;
    public string result;
    public int chapter = 0;
    public int stage = 0 ;
    public GameObject abilitypanel;
    public GameObject pausepanel;
    public GameObject Resultpanel;
    public GameObject homepanel;
    public GameObject bossmonsterhpfac;

    private void Awake()
    {
        Time.timeScale = 1f;
        string scenename = SceneManager.GetActiveScene().name;

        if (scenename.Equals("singlescene1_1")) {

            chapter = 1;
            stage = 1;

        }
        else if (scenename.Equals("singlescene1_2")) {

            chapter = 1;
            stage = 2;

        }
        else if (scenename.Equals("singlescene1_3"))
        {
            bossmonsterhpfac = GameObject.Find("bossmonsterhpfac");
            bossmonsterhpfac.SetActive(false);
            chapter = 1;
            stage = 3;

        }
        else if (scenename.Equals("singlescene2_1"))
        {

            chapter = 2;
            stage = 1;

        }
        else if (scenename.Equals("singlescene2_2"))
        {

            chapter = 2;
            stage = 2;

        }
        else if (scenename.Equals("singlescene2_3"))
        {

            chapter = 2;
            stage = 3;

        }
        Text stagetext = GameObject.Find("stagetext").GetComponent<Text>();
        stagetext.text = "Stage " + stage;

        Text goldtext = GameObject.Find("goldtext").GetComponent<Text>();
        goldtext.text = "G : " + singleinventory.goldinventory;


        pausepanel = GameObject.Find("pausepanel");
        pausepanel.SetActive(false);

        abilitypanel = GameObject.Find("abilityselectpanel");
        abilitypanel.SetActive(false);

        Resultpanel = GameObject.Find("Resultpanel");
        Resultpanel.SetActive(false);

        homepanel = GameObject.Find("homepanel");
        homepanel.SetActive(false);

        //카메라위치방향
        Vector3 camerafirstpos = new Vector3(0, 156f, -30f);
        Quaternion camerafirstrot = Quaternion.Euler(85, 0, 0);
        maincamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        maincamera.position = camerafirstpos;
        maincamera.rotation = camerafirstrot;

        Quaternion playerfirstrot = Quaternion.Euler(0, 0, 0);
        Vector3 playerfirstpos = new Vector3(0, 0, -20f);
        player = Resources.Load<GameObject>("singleplayerfac");
        Instantiate(player, playerfirstpos, playerfirstrot);


        //플레이어 생성
        string totalpower_pp = PlayerPrefs.GetString("totalpower");
        int totalpower;
        int.TryParse(totalpower_pp, out totalpower);

        string totalhp_pp = PlayerPrefs.GetString("totalhp");
        int totalhp;
        int.TryParse(totalhp_pp, out totalhp);

        string gunid_pp = PlayerPrefs.GetString("gunid");
        Debug.Log("!!!gunid_pp:" + gunid_pp);
        int gunid;
        int.TryParse(gunid_pp, out gunid);

        string armorid_pp = PlayerPrefs.GetString("armorid");
        Debug.Log("!!!armorid_pp:" + armorid_pp);
        // int armorid = int.Parse(armorid_pp);
        int armorid;
        int.TryParse(armorid_pp, out armorid);

        string skill1id_pp = PlayerPrefs.GetString("skill1id");
        int skill1id;
        int.TryParse(skill1id_pp, out skill1id);

        string skill2id_pp = PlayerPrefs.GetString("skill2id");
        int skill2id;
        int.TryParse(skill2id_pp, out skill2id);

        GameObject playerbody = GameObject.FindWithTag("Player");
        // Debug.Log("pvpstart plna :" + playerbody.name);
        var playerbodycom = playerbody.GetComponent<singleplayerinfo>();

        playerbodycom.playermaxhp = totalhp;
        playerbodycom.playerhp = totalhp;
        playerbodycom.playerpower = totalpower;
        playerbodycom.playerindex = 0;
        playerbodycom.playergunid = gunid;
        playerbodycom.playerarmorid = armorid;
        playerbodycom.playerskill1id = skill1id;
       // playerbodycom.playerskill2id = skill2id;

        //장비를 장착시킴
        if (gunid != 0)
        {
            GameObject weaponhand = GameObject.FindWithTag("weaponhand");
            //캐릭터가 장비하고 있는 총의 이름을 만듦
            string gunname = "gun" + gunid_pp;

            //총 오브젝트를 만든다
            GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
            GameObject handgun = Instantiate(handgun_ob);


            Vector3 gunpos = new Vector3(0, 0, 0);
            Quaternion gunrot = Quaternion.Euler(0, 90, -90);
            Vector3 gunscale = new Vector3(3, 1, 0.5f);

            //총을 손의 하위로 만든다
            handgun.transform.SetParent(weaponhand.transform);
            //총의 스케일 , 위치 , 방향을 지정해줌
            handgun.transform.localScale = gunscale;
            handgun.transform.localPosition = gunpos;
            handgun.transform.localRotation = gunrot;

            //총마다 달린 능력을 적용 스테이지 1에서만 적용한다 나머지 스테이지는 스테틱이라 적용된 상태
            if (stage == 1)
            {
                if (gunid == 1)
                {

                    currentability.abilitylist.Add(3);

                }

            }
        }
        if (skill1id != 0)
        {

            string imagename = "skill" + skill1id_pp;
            GameObject skillbt = GameObject.Find("skillbt");
            Image skill1image = skillbt.GetComponent<Image>();
            Sprite skill1texture = Resources.Load<Sprite>("Skillimage/" + imagename);
            skill1image.sprite = skill1texture;

        }
        else
        {

            GameObject skillbt = GameObject.Find("skillbt");
            Image skill1image = skillbt.GetComponent<Image>();
            skill1image.color = new Color(159 / 255f, 154 / 255f, 151 / 255f);
            skillbt.GetComponent<Button>().interactable = false;
        }


        StartCoroutine(detailspec());

    }

    IEnumerator detailspec()
    {

        //서버에서 케릭터의 아이템 리스트를 받아온다

        string gunid_pp = PlayerPrefs.GetString("gunid");
        Debug.Log("!!!gunid_pp:" + gunid_pp);
        int playergunid;
        int.TryParse(gunid_pp, out playergunid);

        string skill1id_pp = PlayerPrefs.GetString("skill1id");
        int playerskill1id;
        int.TryParse(skill1id_pp, out playerskill1id);

        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("playergunid", playergunid);
        w.AddField("playerskill1id", playerskill1id);


        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/pvpitemspec.php", w);

        Debug.Log("4 , 통신 반환값 전달");

        yield return www.SendWebRequest();

        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5, 네트워크 통신 error: " + www.error);

        }
        else //서버 연결 성공
        {

            result = www.downloadHandler.text;
            Debug.Log("5, !!!!!!!!!!!!!!!!!네트워크 통신 성공 result: " + result);

            //json 데이터로 아이템 데이터, 스킬데이터를 받아온다
            var jsonPlayer = JSON.Parse(result);


            //useritemData 로 아이템에 대한 정보를 받을 수 있다
            string playerskillatk_js = jsonPlayer["playerskillatk"];
            int playerskillatk;
            int.TryParse(playerskillatk_js, out playerskillatk);


            string playeratkspeed_js = jsonPlayer["playeratkspeed"];
            float playeratkspeed;
            float.TryParse(playeratkspeed_js, out playeratkspeed);


            GameObject playerbody = GameObject.FindWithTag("Player");
            var playerbodycom = playerbody.GetComponent<singleplayerinfo>();

            //등록된 스킬과 아이템의 정보를 읽어서 저장한다
            playerbodycom.playerskillatk = playerskillatk;
            playerbodycom.playeratkspeed = playeratkspeed;
           
            Debug.Log("bulletspeed0!!!!!:" + playeratkspeed);
            GameObject.FindWithTag("Player").GetComponent<singleplayerfire>().fireRate = playeratkspeed;
           
        }

    }





}
