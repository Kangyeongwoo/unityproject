using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Pvpdata;
using FlatBuffers;

public class playerhpbar_sg : MonoBehaviour
{
    //몬스터 체력바에 나타나는 체력
    public Text playerhp_text;

    //몬스터의 체력바 
    public Slider palyerhpbar;

    public int playerhp;
    public int playermaxhp;
    // public static NetworkStream stream = Findenemy_flat.stream;
   
    public int skillatkedcount;
    public GameObject resultpanel;

    private void Start()
    {
        skillatkedcount = 0;
        var playerdata = GameObject.FindWithTag("Player").GetComponent<singleplayerinfo>();
        int stage = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().stage;
        playermaxhp = playerdata.playermaxhp;
        if (stage == 1)
        {
            playerhp = playerdata.playerhp;
            playerhp_text.text = playerhp.ToString();
        }
        else {

          palyerhpbar.value = currentability.currenthpvalue;
          playerhp = currentability.currenthp;
          playerhp_text.text = currentability.currenthp.ToString();

        }

        resultpanel = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().Resultpanel;
    }

    void OnTriggerEnter(Collider other)
    // void OnCollisionEnter(Collision other)
    {
        //other는 부딪힌 오브젝트 벽 또는 적에 맞으면 총알이 사라진다
        if (other.gameObject.tag == "enemybullet" || other.gameObject.tag == "enemy" || other.gameObject.tag == "enemybody")
        {
           
            var enemy = GameObject.FindWithTag("enemy").GetComponent<singlemonsterinfo>();

            //몬스터의 체력은 플레이어 공격력 만큼 감소
            playerhp = playerhp - enemy.monsterattack;

            string hp = playerhp.ToString();
            playerhp_text.text = hp;

            Debug.Log("player_hp: " + playerhp);

        }
        Debug.Log("playerhpdown 2 :" + other.gameObject.tag);
        if (playerhp <= 0)
        {

            //슬라이더 색이 어디 까지 차있는지 정할 수 있음
            playerhp = 0;
            string hp = playerhp.ToString();
            playerhp_text.text = hp;
            palyerhpbar.value = 0;

            Time.timeScale = 0f;
            resultpanel.SetActive(true);
            GameObject resulttext = resultpanel.transform.GetChild(0).gameObject;
            resulttext.GetComponent<Text>().text = "GameOver";
        }
        else
        {
            //분수로 표현 가능
            palyerhpbar.value = (float)playerhp / (float)playermaxhp;
        }
    }


}
