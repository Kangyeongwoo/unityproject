using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mappanelstart : MonoBehaviour
{

    public List<int> mapidlist;
    public GameObject mapslot;
    public GameObject maplist;
    public string chaptername;
    // Start is called before the first frame update
    void Start()
    {
        maplist = GameObject.Find("maplist");
        mapidlist = new List<int>();
        mapidlist.Add(1);
        mapidlist.Add(2);


        foreach(int mapid in mapidlist) {


            string mapimagename = "chapter" + mapid;

            mapslot = Resources.Load<GameObject>("mapslot");

            GameObject map = Instantiate(mapslot);

            map.transform.SetParent(maplist.transform);
            map.transform.localScale = new Vector3(1, 1, 1);


            Texture2D itemtexture = Resources.Load<Texture2D>("mapimage/" + mapimagename);
            map.transform.GetComponent<RawImage>().texture = itemtexture;

            map.GetComponent<mapinfo>().mapid = mapid;

        }




    }



    public void cancel() {

        int playerchapter = PlayerPrefs.GetInt("maxchapter");

        int currentchapter = PlayerPrefs.GetInt("currentchapter");
        Debug.Log("currentchapter1" + currentchapter);

        if (currentchapter != 0)
        {
            playerchapter = currentchapter;
        }

        
        if (playerchapter == 1)
        {

            chaptername = "1.사막";


        }
        else if (playerchapter == 2)
        {

            chaptername = "2.초원";

        }

        string mapname = "chapter" + currentchapter + "obj";

        var mainstart = GameObject.Find("mainstart").GetComponent<mainstart>();
        mainstart.mapselectpanel.SetActive(false);

        GameObject.Find("chaptername").GetComponent<Text>().text = chaptername;


        GameObject newmap = Resources.Load<GameObject>("singleMap/" + mapname);
        Instantiate(newmap);

   

    }



}
