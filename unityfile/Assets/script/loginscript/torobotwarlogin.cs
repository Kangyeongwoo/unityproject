using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 씬 전환에 필요한 using
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class torobotwarlogin : MonoBehaviour
{
    public void SceneChange_torobotwarlogin()
    {
        //뒤에 적힌 씬을 연다 현재 씬은 사라짐
        SceneManager.LoadScene("login_robotwar");
    }
}
