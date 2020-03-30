using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemyhpbar : MonoBehaviour
{
    //몬스터 체력바에 나타나는 체력
    public Text enemyhp_text;

    //몬스터의 체력바 
    public Slider enemyhpbar;

    public int enemyhp;

    private void Start()
    {
        //enemy의 enemyinfo 안에서 hp를 가지고 와서 표시해준다
        var enemydata = GameObject.FindWithTag("enemy").GetComponent<Enemyinfo>();
        enemyhp = enemydata.enemyhp;
        enemyhp_text.text = enemyhp.ToString();
    }

    /*
    void OnCollisionEnter(Collision other)
    {

        int enemymaxhp = GameObject.FindWithTag("enemy").GetComponent<Enemyinfo>().enemymaxhp;
        int enemyhp = GameObject.FindWithTag("enemy").GetComponent<Enemyinfo>().enemyhp;

        //총알과 부딪힐때
        if (other.gameObject.tag == "palyerbullet")
        {
            //이름이 playerinfo인 파일의 componet 중 c#파일 playerinfo안에 있는 변수를 사용하기 위한 변수
            var player = GameObject.FindWithTag("Player").GetComponent<Playerinfo>();

            //몬스터의 체력은 플레이어 공격력 만큼 감소
            enemyhp = enemyhp - player.playerpower;

            string hp = enemyhp.ToString();
            enemyhp_text.text = hp;

            Debug.Log("enemy_hp: " + enemyhp);
        }


        if (enemyhp <= 0)
        {
            //슬라이더 색이 어디 까지 차있는지 정할 수 있음
            enemyhp = 0;
            enemyhpbar.value = 0;
        }
        else
        {
            //분수로 표현 가능
            enemyhpbar.value = (float)enemyhp / (float)enemymaxhp;
        }


    }
    */   
}
