using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossresultcheck : MonoBehaviour
{


    private void Start()
    {

        StartCoroutine(checkdeadcount());

    }


    IEnumerator checkdeadcount() {

        while (true) {


            if (bossstart.deadtable.Contains(0) && bossstart.deadtable.Contains(1) && bossstart.deadtable.Contains(2)) {

                GameObject deadpanel = GameObject.Find("bossstart_cs").GetComponent<bossstart>().deadpanel;
                deadpanel.SetActive(false);


                GameObject Resultpanel = GameObject.Find("bossstart_cs").GetComponent<bossstart>().Resultpanel;
                Resultpanel.SetActive(true);


            }



            yield return new WaitForSeconds(0.2f);
        }

       
    
    }

}
