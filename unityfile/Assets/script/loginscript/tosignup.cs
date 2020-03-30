using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class tosignup : MonoBehaviour
{
    public void SceneChange_tosignup(){
        // 회원가입을 위한 씬으로 이동 
        SceneManager.LoadScene("login_robotwar_signup");
    }
    public void SceneChange_tologin()
    {
        // 회원가입을 위한 씬으로 이동 
        SceneManager.LoadScene("login");
    }
}
