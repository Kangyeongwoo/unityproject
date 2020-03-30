using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class registerstart : MonoBehaviour
{

    public Text text;

    //팝업창 패널
    public GameObject authpanel;

    // Start is called before the first frame update
    void Start()
    {  //시작할때 동작하는 함수
        //패널 비활성화
        authpanel.gameObject.SetActive(false);
    }
    //패널 닫는 함수
    public void panelclose() {
        //패널 비활성화
        authpanel.gameObject.SetActive(false);
        //아이디 밑에 있는 결과 표출 창 공백으로 수정
        text.text = "";
    }
}
