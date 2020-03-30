using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tobossready : MonoBehaviour
{
   
    public void Tobossready() {


        findteam.socketConnection.Close();
        SceneManager.LoadScene("bosssceneready");


    }

}
