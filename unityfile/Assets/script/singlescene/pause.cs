using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{

    GameObject pausepanel;
    GameObject homepanel;
    GameObject equipskillslot;
    GameObject equipitemslot;
    public List<GameObject> allslot;

    GameObject joystick;
    // Start is called before the first frame update
    void Start()
    {
        allslot = new List<GameObject>();
        pausepanel = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().pausepanel;
        homepanel = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().homepanel;
      //  joystick = GameObject.Find("singlescenestart_cs").GetComponent<Singlescenestart>().joystick;
    }

    public void pauseclick() {

        Time.timeScale = 0f;
        pausepanel.SetActive(true);
      //  joystick.SetActive(false);
        GameObject equipskill = GameObject.Find("equipskill");
        GameObject equipitem = GameObject.Find("equipitem");

        equipskillslot = Resources.Load<GameObject>("singleskillslot");
        equipitemslot = Resources.Load<GameObject>("singleitemslot");
        //인벤토리에 프리팹을 만들고 스크립트에 내용입, 이미지 적용

        foreach (int ability in currentability.abilitylist) {

            //아이템 슬롯을 오브젝트로 만들기
            GameObject eqskillslot = Instantiate(equipskillslot);
            allslot.Add(eqskillslot);
            //아이템 리스트로 만들기
            eqskillslot.transform.SetParent(equipskill.transform);
            eqskillslot.transform.localScale = new Vector3(1, 1, 1);

            string imagename = "ability" + ability;
            RawImage itemimage = eqskillslot.GetComponent<RawImage>();
            Texture2D itemtexture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            itemimage.texture = itemtexture;

        }
        //현재가지고 있는 액티브 스킬 조각을 가진다
        foreach (DictionaryEntry activeskill in singleinventory.activeskillinventory)
        {
           
            GameObject eqitemslot = Instantiate(equipitemslot);
            allslot.Add(eqitemslot);
            //아이템 리스트로 만들기
            eqitemslot.transform.SetParent(equipitem.transform);
            eqitemslot.transform.localScale = new Vector3(1, 1, 1);


            eqitemslot.transform.GetChild(1).gameObject.GetComponent<Text>().text = activeskill.Value.ToString();

            //획득한 아이템 이미지 표시
            string imagename = "skill" + activeskill.Key;
            RawImage itemimage = eqitemslot.GetComponent<RawImage>();
            Texture2D itemtexture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            itemimage.texture = itemtexture;

            int activeid = (int)activeskill.Key;
            //이름 입력
            var activenamelist = GameObject.Find("dropitem_cs").GetComponent<dropitem>().activeskill;
              eqitemslot.transform.GetChild(2).gameObject.GetComponent<Text>().text = activenamelist[activeid].ToString();
            // 
           // eqitemslot.transform.GetChild(1).gameObject.GetComponent<Text>().text = "q";





            Debug.Log(" ac Key , Value :"+ activeskill.Key+","+ activeskill.Value);




        }
        //현재가지고 있는 패시브 스킬 조각을 가진다
        foreach (DictionaryEntry passiveskill in singleinventory.passiveskillinventory)
        {
           
            GameObject eqitemslot = Instantiate(equipitemslot);
            allslot.Add(eqitemslot);
            //아이템 리스트로 만들기
            eqitemslot.transform.SetParent(equipitem.transform);
            eqitemslot.transform.localScale = new Vector3(1, 1, 1);

            //개수 표시
            eqitemslot.transform.GetChild(1).gameObject.GetComponent<Text>().text = passiveskill.Value.ToString();

            //획득한 아이템 이미지 표시
            string imagename = "ability" + passiveskill.Key;
            RawImage itemimage = eqitemslot.GetComponent<RawImage>();
            Texture2D itemtexture = Resources.Load<Texture2D>("Skillimage/" + imagename);
            itemimage.texture = itemtexture;

            int passiveid = (int)passiveskill.Key;
            //이름 입력
            var passivenamelist = GameObject.Find("dropitem_cs").GetComponent<dropitem>().passiveskill;
              eqitemslot.transform.GetChild(2).gameObject.GetComponent<Text>().text = passivenamelist[passiveid].ToString();
            // 
           //eqitemslot.transform.GetChild(1).gameObject.GetComponent<Text>().text = "q";
           Debug.Log(" pa Key , Value :" + passiveskill.Key + "," + passiveskill.Value);


        }


    }

    public void pausehome() {

        homepanel.SetActive(true);

    }
    public void pausehome_cancel()
    {

        homepanel.SetActive(false);

    }
    public void pausehome_ok()
    {
        currentability.abilitylist = new List<int>();
        //  currentability.currenthpvalue;
        //  currentability.currenthp;
        // monsterspawn.result = null;
        singleinventory.goldinventory = 0;
        singleinventory.passiveskillinventory = new Hashtable();
        singleinventory.activeskillinventory = new Hashtable();
        //스태틱 초기화
        //아이템 전달 x
       // joystick.SetActive(true);
        SceneManager.LoadScene("mainscene");
    }


    public void pauseback() {


        foreach (GameObject slot in allslot) {

            Destroy(slot);

        }

      // joystick.SetActive(true);
        pausepanel.SetActive(false);
        Time.timeScale = 1f;

    }
}
