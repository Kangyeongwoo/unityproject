using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resultcheck : MonoBehaviour
{
    public GameObject joystick;

    public GameObject resultpanel;

    public Text resulttext;

    public static int check = 0 ;

    private void Awake()
    {
        //결과가 나오는 패널을 비활성화
        resultpanel.SetActive(false);
    }


    private void Update()
    {
        var player = GameObject.FindWithTag("Player").GetComponent<Playerhpbar>();
        var enemy = GameObject.FindWithTag("enemy").GetComponent<Enemyhpbar>();

        int enemyhp = enemy.enemyhp;

        int playerhp = player.playerhp;

        //상대체력과 내체력 확인해서 0이면 lose가 나타나도록
        //draw가 동시에 안나왔던 문제가 있다 
        if (playerhp != 0 && enemyhp == 0) {

            Time.timeScale = 0.0F;
            Control_flat.thread.Abort();
            resultpanel.SetActive(true);
            resulttext.text = "Win";
            check = 1;
        }
        else if (playerhp == 0 && enemyhp != 0) {

            Time.timeScale = 0.0F;
            Control_flat.thread.Abort();
            resultpanel.SetActive(true);

            resulttext.text = "Lose";
            check = 2;
        }
        else if (playerhp == 0 && enemyhp == 0)
        {
            Time.timeScale = 0.0F;
            Control_flat.thread.Abort();
            resultpanel.SetActive(true);
           // resulttext.text = "Draw";
            check = 3;
        }
        else { 
        
        
        }
    }
}
