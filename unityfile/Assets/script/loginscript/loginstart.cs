using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginstart : MonoBehaviour
{
    // 게임오브젝트 팝업패널과 연결
    public GameObject authpanel;
    // Start is called before the first frame update
    void Start()
    {
        //씬이 시작 될 때 팝업창이 사라지게 함
        authpanel.gameObject.SetActive(false);
    }
    //팝업창이 사라지게하는 함수
    public void panelclose()
    {
        //팝업창이 사라지게하는 함수
        authpanel.gameObject.SetActive(false);
    }
}
