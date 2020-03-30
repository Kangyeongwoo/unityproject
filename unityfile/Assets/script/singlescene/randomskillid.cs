using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class randomskillid : MonoBehaviour
{
    public int randomid ;


    public void skillid() {

        int stage = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().stage;
        int chapter = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().chapter;
        currentability.abilitylist.Add(randomid);

        Time.timeScale = 1f;

        if (chapter==1 && stage == 1) {
            SceneManager.LoadScene("singlescene1_2");
        }
        if(chapter == 1 && stage == 2) {
            SceneManager.LoadScene("singlescene1_3");
        }
       

        if (chapter == 2 && stage == 1)
        {
            SceneManager.LoadScene("singlescene2_2");
        }
        if (chapter == 2 && stage == 2)
        {
            SceneManager.LoadScene("singlescene2_3");
        }


    }

}
