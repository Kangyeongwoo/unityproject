using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class passwordcheck : MonoBehaviour
{
    //첫번째 패스워드 필드
    public InputField password1;
    //두번째 패스워드 필드
    public InputField password2;
    //패스워드 일치, 불일치, 등의 상태를 나타내는 텍스트 창
    public Text pwchecktext;

    //pwcheck하는 변수
    public string pwcheck = "0";

    //첫번째 패스워드를 체크하는 함수
    public void password1_check() {

        //패스워드 길이가 4에서 15 사이 일때
        if (password1.text.Length >= 4 && password1.text.Length <= 15)
        {

            Debug.Log("1 , 패스워드1 형식 ok ");
            if (password2.text == "")
            {
                Debug.Log("2 , 아래가 비어있어 ok ");
                //체크는 기본 0
                pwcheck = "0";
                //결과 값 표출 텍스트에 공백; 
                pwchecktext.text = "";
            }
            else
            {
                Debug.Log("2 , 아래 값이 있음 ");
                if (password1.text == password2.text)
                {
                    Debug.Log("3 , 일치 표시 ");
                    //체크를 1
                    pwcheck = "1";
                    //결과 값 표출 텍스트에 비밀번호 일치; 
                    pwchecktext.text = "비밀번호 일치";

                }
                else
                {
                    Debug.Log("3 , 불일치 표시 ");
                    //체크를 0
                    pwcheck = "0";
                    //결과 값 표출 텍스트에 비밀번호 불일치; 
                    pwchecktext.text = "비밀번호 불일치";
                }

            }

        } //패스워드 길이가 4보다 작을때
        else {
            Debug.Log("1 , 패스워드1 형식 no ");
            //결과 값 표출 텍스트에 형식을 지켜주세요; 
            pwchecktext.text = "형식을 지켜주세요";
        }

    }

    //두번째 패스워드를 체크하는 함수
    public void password2_check() {

       
        if (password2.text.Length >= 4 && password2.text.Length <= 15) {

            Debug.Log("1 , 패스워드2 형식 ok ");

            //위에가 비어있을때
            if (password1.text == "")
        {
                Debug.Log("2 , 위가 비어있어 ok ");
                //체크는 기본 0
                pwcheck = "0";
                //결과 값 표출 텍스트에 공백; 
                pwchecktext.text = "";
            }
        else
            {//위에가 비어있지 않을 때
                Debug.Log("2 , 위가 안비어있어 ");
                // 패스워드 두개가 같으면
                if (password1.text == password2.text)
            {
                    Debug.Log("3 , 일치 표시 ");
                    //체크는 1
                    pwcheck = "1";
                    //결과 값 표출 텍스트에 비밀번호 일치; 
                    pwchecktext.text = "비밀번호 일치";
                
            }
            else
            {
                    Debug.Log("3 , 불일치 표시 ");
                    pwcheck = "0";
               
                    //결과 값 표출 텍스트에 비밀번호 불일치; 
                    pwchecktext.text = "비밀번호 불일치";
            }

        }

        }

        //패스워드 길이가 4보다 작을때
        else
        {
            Debug.Log("1 , 패스워드2 형식 no ");

            pwchecktext.text = "형식을 지켜주세요";
        }

    }
}
