using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class pvpstart : MonoBehaviour
{
    Vector3 playerpos;

    Quaternion playerrot;

    public GameObject player = null;

    public Text finding;

    public Text usernickname;

    public Button tcpclosebt;

    void Awake() {

        tcpclosebt.gameObject.SetActive(false);
        finding.enabled = false;
        //  StartCoroutine(roadpvp());
        roadpvp();

    }

  //  IEnumerator roadpvp()
    public void roadpvp()
    {
        //내 닉네임을 통해서 캐릭터 생성
        usernickname.text = PlayerPrefs.GetString("nickname");

        playerpos = new Vector3(-108.2f,219.25f,-81.6f);

        playerrot = Quaternion.Euler(-90, 180, 0);

        player = Resources.Load<GameObject>("pvpready_player");

        //플레이어 생성
        Instantiate(player, playerpos, playerrot);


        string gunid_pp = PlayerPrefs.GetString("gunid");

        string armorid_pp = PlayerPrefs.GetString("armorid");

        string totalpower_pp = PlayerPrefs.GetString("totalpower");

        string totalhp_pp = PlayerPrefs.GetString("totalhp");

        GameObject weaponhand = GameObject.FindWithTag("weaponhand");

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
        int gold= PlayerPrefs.GetInt("gold");
        string nickname = PlayerPrefs.GetString("nickname");

        GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + gold.ToString();

        GameObject.Find("nicknametext").GetComponent<Text>().text = nickname;



        //  yield return null;
    }


}
