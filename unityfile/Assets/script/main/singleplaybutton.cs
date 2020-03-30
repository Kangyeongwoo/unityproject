using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class singleplaybutton : MonoBehaviour
{

   

    public void singleplay() {

        int playerchapter = PlayerPrefs.GetInt("maxchapter");

        int currentchapter = PlayerPrefs.GetInt("currentchapter");
        Debug.Log("currentchapter1"+currentchapter);

        if (currentchapter != 0) {
            playerchapter = currentchapter;
        }


        if (playerchapter == 1)
        {
            Debug.Log("currentchapter2" + currentchapter);
            SceneManager.LoadScene("singlescene1_1");

        }else if (playerchapter == 2) {
            Debug.Log("currentchapter3" + currentchapter);
            SceneManager.LoadScene("singlescene2_1");

        }

    }


}
