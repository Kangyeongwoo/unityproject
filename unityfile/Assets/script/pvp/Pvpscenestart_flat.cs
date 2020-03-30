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

public class Pvpscenestart_flat : MonoBehaviour
{
    public GameObject joystick;
    public GameObject remotecontrol;
    public GameObject player = null;
    public GameObject enemy = null;
    public Transform maincamera;
    public static int enemyindex;
    public string result;
    // public Player pvpenemy;
    //  public GameObject playerbody;
    //  public GameObject enemybody;
    //  public UnityWebRequest www;
    private void Awake()
    {

        ByteBuffer startenemy = Findenemy_flat.serverMessage2;

        Game gameenemy = Game.GetRootAsGame(startenemy);
        Player pvpenemy = (Player)gameenemy.Player;

        //  Player pvpenemy = Player.GetRootAsPlayer(startenemy);

        enemyindex = pvpenemy.Roomindex;
        Debug.Log("enemynick: " + pvpenemy.Nickname);
        Debug.Log("enemyindex: "+enemyindex);

       

        if (enemyindex == 0)
        {
            //상대가 1p 내가 2P 일때 정해진 위치와 방향에 캐릭터와 카메라 생성
           
            //enemy위치방향
            Quaternion enemyfirstrot = Quaternion.Euler(0, 0, 0);
            Vector3 enemyfirstpos = new Vector3(0, 0, -14f);

            //플레이어위치방향
            Quaternion playerfirstrot = Quaternion.Euler(0, 180, 0);
            Vector3 playerfirstpos = new Vector3(0, 0, 13f);

            //카메라위치방향
            Vector3 camerafirstpos = new Vector3(-0.2f, 26.8f, 8f);
            Quaternion camerafirstrot = Quaternion.Euler(85, 180, 0);

            //플레이어 생성
            player = Resources.Load<GameObject>("pvpplayerfac");
            Instantiate(player, playerfirstpos, playerfirstrot);

            //enemy생성
            enemy = Resources.Load<GameObject>("pvpenemyfac");
            Instantiate(enemy, enemyfirstpos, enemyfirstrot);

            //카메라 지정
            maincamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
            maincamera.position = camerafirstpos;
            maincamera.rotation = camerafirstrot;

            //플레이어 생성
            //생성하면서 info에 데이터 저장
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
            var playerbodycom = playerbody.GetComponent<Playerinfo>();


            GameObject enemybody = GameObject.FindWithTag("enemy");
            Debug.Log("pvpstart enna :" + enemybody.name);
            //생성하면서 info에 데이터 저장
            var enemybodycom = enemybody.GetComponent<Enemyinfo>();

            //  playerbodycom.playermaxhp = PlayerPrefs.GetInt("hp");
            //  playerbodycom.playerhp = PlayerPrefs.GetInt("hp");
            //  playerbodycom.playerpower = PlayerPrefs.GetInt("power");
            playerbodycom.playermaxhp = totalhp;
            playerbodycom.playerhp = totalhp;
            playerbodycom.playerpower = totalpower;
            playerbodycom.playerindex = 1;
            playerbodycom.playergunid = gunid;
            playerbodycom.playerarmorid = armorid;
            playerbodycom.playerskill1id = skill1id;
            playerbodycom.playerskill2id = skill2id;


            Debug.Log("pvpstart plna :" + playerbody.name);

            //enemy 생성
           
            enemybodycom.enemymaxhp = pvpenemy.Hp;
            enemybodycom.enemyhp = pvpenemy.Hp;
            enemybodycom.enemypower = pvpenemy.Power;
            enemybodycom.enemyindex = pvpenemy.Roomindex;
            enemybodycom.enemygunid = pvpenemy.Gunid;
            enemybodycom.enemyarmorid = pvpenemy.Armorid;
            enemybodycom.enemyskill1id = pvpenemy.Skill1id;
            enemybodycom.enemyskill2id = pvpenemy.Skill2id;

            if (gunid != 0) {
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

            }
            if (pvpenemy.Gunid!=0) {
                //캐릭터가 장비하고 있는 총의 이름을 만듦
                GameObject enemyweaponhand = GameObject.FindWithTag("enemyweaponhand");
                string enemygunid_pp = pvpenemy.Gunid.ToString();
                string gunname = "gun" + enemygunid_pp;

                //총 오브젝트를 만든다
                GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
                GameObject handgun = Instantiate(handgun_ob);


                Vector3 gunpos = new Vector3(0, 0, 0);
                Quaternion gunrot = Quaternion.Euler(0, 90, -90);
                Vector3 gunscale = new Vector3(3, 1, 0.5f);

                //총을 손의 하위로 만든다
                handgun.transform.SetParent(enemyweaponhand.transform);
                //총의 스케일 , 위치 , 방향을 지정해줌
                handgun.transform.localScale = gunscale;
                handgun.transform.localPosition = gunpos;
                handgun.transform.localRotation = gunrot;

            }

            if (skill1id !=0) {

                string imagename = "skill" + skill1id_pp;
                GameObject skillbt = GameObject.Find("skillbt");
                Image skill1image = skillbt.GetComponent<Image>();
                Sprite skill1texture = Resources.Load<Sprite>("Skillimage/" + imagename);
                skill1image.sprite = skill1texture;

            }
            else {

                GameObject skillbt = GameObject.Find("skillbt");
                Image skill1image = skillbt.GetComponent<Image>();
                skill1image.color = new Color(159 / 255f, 154 / 255f, 151 / 255f);
                skillbt.GetComponent<Button>().interactable = false;
            }



        }
        else if (enemyindex == 1)
        {
            //상대가 2p 내가 1p

            //플레이어 위치방향
            Quaternion playerfirstrot = Quaternion.Euler(0, 0, 0);
            Vector3 playerfirstpos = new Vector3(0, 0, -14f);

            //enemy위치방향
            Quaternion enemyfirstrot = Quaternion.Euler(0, 180, 0);
            Vector3 enemyfirstpos = new Vector3(0, 0, 13f);

            //카메라위치방향
            Vector3 camerafirstpos = new Vector3(-0.2f, 26.8f, -6f);
            Quaternion camerafirstrot = Quaternion.Euler(85, 0, 0);

            //플레이어 생성
            player = Resources.Load<GameObject>("pvpplayerfac");
            Instantiate(player, playerfirstpos, playerfirstrot);

            //enemy생성
            enemy = Resources.Load<GameObject>("pvpenemyfac");
            Instantiate(enemy, enemyfirstpos, enemyfirstrot);

            //카메라 지정
            maincamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
            maincamera.position = camerafirstpos;
            maincamera.rotation = camerafirstrot;

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
            var playerbodycom = playerbody.GetComponent<Playerinfo>();

            // playerbodycom.playermaxhp = PlayerPrefs.GetInt("hp");
            // playerbodycom.playerhp = PlayerPrefs.GetInt("hp");
            // playerbodycom.playerpower = PlayerPrefs.GetInt("power");
            playerbodycom.playermaxhp = totalhp;
            playerbodycom.playerhp = totalhp;
            playerbodycom.playerpower = totalpower;
            playerbodycom.playerindex = 0;
            playerbodycom.playergunid = gunid;
            playerbodycom.playerarmorid = armorid;
            playerbodycom.playerskill1id = skill1id;
            playerbodycom.playerskill2id = skill2id;

            //enemy생성
            GameObject enemybody = GameObject.FindWithTag("enemy");
          //  Debug.Log("pvpstart enna :" + enemybody.name);
            var enemybodycom = enemybody.GetComponent<Enemyinfo>();
          
            enemybodycom.enemymaxhp = pvpenemy.Hp;
            enemybodycom.enemyhp = pvpenemy.Hp;
            enemybodycom.enemypower = pvpenemy.Power;
            enemybodycom.enemyindex = pvpenemy.Roomindex;
            enemybodycom.enemygunid = pvpenemy.Gunid;
            enemybodycom.enemyarmorid = pvpenemy.Armorid;
            enemybodycom.enemyskill1id = pvpenemy.Skill1id;
            enemybodycom.enemyskill2id = pvpenemy.Skill2id;


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

            }
            if (pvpenemy.Gunid != 0)
            {
                //캐릭터가 장비하고 있는 총의 이름을 만듦
                GameObject enemyweaponhand = GameObject.FindWithTag("enemyweaponhand");
                string enemygunid_pp = pvpenemy.Gunid.ToString();
                string gunname = "gun" + enemygunid_pp;

                //총 오브젝트를 만든다
                GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
                GameObject handgun = Instantiate(handgun_ob);


                Vector3 gunpos = new Vector3(0, 0, 0);
                Quaternion gunrot = Quaternion.Euler(0, 90, -90);
                Vector3 gunscale = new Vector3(3, 1, 0.5f);

                //총을 손의 하위로 만든다
                handgun.transform.SetParent(enemyweaponhand.transform);
                //총의 스케일 , 위치 , 방향을 지정해줌
                handgun.transform.localScale = gunscale;
                handgun.transform.localPosition = gunpos;
                handgun.transform.localRotation = gunrot;

            }

            if (skill1id != 0)
            {

                string imagename = "skill" + skill1id_pp;
                GameObject skillbt = GameObject.Find("skillbt");
                Image skill1image = skillbt.GetComponent<Image>();
                Sprite skill1texture = Resources.Load<Sprite>("Skillimage/" + imagename);
                skill1image.sprite = skill1texture;

            }
            else {
                GameObject skillbt = GameObject.Find("skillbt");
                Image skill1image = skillbt.GetComponent<Image>();
                skill1image.color = new Color(159/255f , 154/255f , 151/255f);
                skillbt.GetComponent<Button>().interactable = false;
            }

        }

       
      StartCoroutine(detailspec());

        //  StartCoroutine(postToServer( (www) => {
        //      result =www.downloadHandler.text;
        //      Debug.Log("콜백: " + result);
        //  }
        //));
      //  InvokeRepeating("test", 5 ,1);



    }
    /*
IEnumerator postToServer( Action<UnityWebRequest> callback)
    {
        string gunid_pp = PlayerPrefs.GetString("gunid");
        Debug.Log("!!!gunid_pp:" + gunid_pp);
        int playergunid;
        int.TryParse(gunid_pp, out playergunid);


        string skill1id_pp = PlayerPrefs.GetString("skill1id");
        int playerskill1id;
        int.TryParse(skill1id_pp, out playerskill1id);

        ByteBuffer startenemy = Findenemy_flat.serverMessage2;

        Game gameenemy = Game.GetRootAsGame(startenemy);
        Player pvpenemy = (Player)gameenemy.Player;
        int enemygunid = pvpenemy.Gunid;
        int enemyskill1id = pvpenemy.Skill1id;

        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("playergunid", playergunid);
        w.AddField("playerskill1id", playerskill1id);
        w.AddField("enemygunid", enemygunid);
        w.AddField("enemyskill1id", enemyskill1id);

        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/pvpitemspec.php", w);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("[Error]:" + www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }

        callback(www);
    }
    */


    IEnumerator detailspec() {

        //서버에서 케릭터의 아이템 리스트를 받아온다

        string gunid_pp = PlayerPrefs.GetString("gunid");
        Debug.Log("!!!gunid_pp:" + gunid_pp);
        int playergunid;
        int.TryParse(gunid_pp, out playergunid);


        string skill1id_pp = PlayerPrefs.GetString("skill1id");
        int playerskill1id;
        int.TryParse(skill1id_pp, out playerskill1id);

        ByteBuffer startenemy = Findenemy_flat.serverMessage2;

        Game gameenemy = Game.GetRootAsGame(startenemy);
        Player pvpenemy = (Player)gameenemy.Player;
        int enemygunid = pvpenemy.Gunid;
        int enemyskill1id = pvpenemy.Skill1id;

        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("playergunid", playergunid);
        w.AddField("playerskill1id", playerskill1id);
        w.AddField("enemygunid", enemygunid);
        w.AddField("enemyskill1id", enemyskill1id);



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
            //  int playeratkspeed;
            //  int.TryParse(playeratkspeed_js, out playeratkspeed);
            float playeratkspeed;
            float.TryParse(playeratkspeed_js, out playeratkspeed);

            string enemyskillatk_js = jsonPlayer["enemyskillatk"];
            int enemyskillatk;
            int.TryParse(enemyskillatk_js, out enemyskillatk);


            string enemyatkspeed_js = jsonPlayer["enemyatkspeed"];
            //  int enemyatkspeed;
            //  int.TryParse(enemyatkspeed_js, out enemyatkspeed);
            float enemyatkspeed;
            float.TryParse(enemyatkspeed_js, out enemyatkspeed);

            GameObject playerbody = GameObject.FindWithTag("Player");
            var playerbodycom = playerbody.GetComponent<Playerinfo>();


            GameObject enemybody = GameObject.FindWithTag("enemy");
            Debug.Log("pvpstart enna :" + enemybody.name);
            //생성하면서 info에 데이터 저장
            var enemybodycom = enemybody.GetComponent<Enemyinfo>();

            //등록된 스킬과 아이템의 정보를 읽어서 저장한다
            playerbodycom.playerskillatk = playerskillatk;
            playerbodycom.playeratkspeed = playeratkspeed;
            enemybodycom.enemyskillatk = enemyskillatk;
            enemybodycom.enemyatkspeed = enemyatkspeed;
            Debug.Log("bulletspeed0!!!!!:" + playeratkspeed);
            Playerfire.fireRate = playeratkspeed;
            Remotecontrol_flat.fireRate = enemyatkspeed;
        }

    }

   

}
