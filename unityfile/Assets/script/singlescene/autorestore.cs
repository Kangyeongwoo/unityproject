using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autorestore : MonoBehaviour
{
    public GameObject hprestore;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(autoresotre());
    }

    IEnumerator autoresotre() {

        yield return new WaitForSeconds(1f);

        var player_hp = GameObject.FindWithTag("Player").GetComponent<playerhpbar_sg>();
        // var player = GameObject.FindWithTag("Player").
        double resotre = player_hp.playermaxhp * 0.1;

        player_hp.playerhp += (int)resotre;

        if (player_hp.playerhp > player_hp.playermaxhp)
        {
            player_hp.playerhp = player_hp.playermaxhp;
            player_hp.palyerhpbar.value = 1;
        }
        else
        {

            player_hp.palyerhpbar.value = (float)player_hp.playerhp / (float)player_hp.playermaxhp;
        }
        player_hp.playerhp_text.text = player_hp.playerhp.ToString();

        currentability.currenthpvalue = GameObject.FindWithTag("Player").GetComponent<playerhpbar_sg>().palyerhpbar.value;
        currentability.currenthp = GameObject.FindWithTag("Player").GetComponent<playerhpbar_sg>().playerhp;


        Destroy(hprestore);

    }
}
