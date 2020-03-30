using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapinfo : MonoBehaviour
{

   public int mapid;

    public string chaptername;

    public void mapselect() {
        //게임 시작할때 플레이어프랩스 currentchapter 만들기

        int usermaxchapter = PlayerPrefs.GetInt("maxchapter");

        //mapid가 1이면 currentchapter =1;

        //mapid가 2이면 currentchapter =2;

        if(mapid > usermaxchapter) { 
        
           // 할수 없음
        
        }else{

           GameObject mapobj= GameObject.FindWithTag("mapobj");
            Destroy(mapobj);
            string mapname = "chapter" + mapid + "obj";
            var mainstart = GameObject.Find("mainstart").GetComponent<mainstart>();
            mainstart.mapselectpanel.SetActive(false);

            if(mapid == 1) {

                chaptername = "1.사막";


            }else if(mapid == 2) {

                chaptername = "2.초원";

            }

            GameObject.Find("chaptername").GetComponent<Text>().text = chaptername;


            GameObject newmap = Resources.Load<GameObject>("singleMap/" + mapname);
            Instantiate(newmap);

            PlayerPrefs.SetInt("currentchapter", mapid);

        }



        // 챕터 오브젝트 없애고 다른 챕터 오브젝트 생성

        // panel 닫기

    }

    

}
