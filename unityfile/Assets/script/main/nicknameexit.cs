using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class nicknameexit : MonoBehaviour
{
   public void nickname_exit() {

        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("login");
    }
}
