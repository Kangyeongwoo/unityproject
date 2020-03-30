using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System;

public class equipmentstart : MonoBehaviour
{
    //통신 후 받는 결과 값
    public string result;

    //하단 아이템슬롯 담아둘 빈객체
    public GameObject itemslot;

    //하단 스킬슬롯 담아둘 빈객체
    public GameObject skillslot;

    public GameObject skillpartslot;

    //아이템 슬롯의 포지션
    public Vector3 itemslotpos = new Vector3(0f, 0f, 0f);

    //아이템 슬롯의 방향
    public Quaternion itemslotrot = Quaternion.Euler(0, 0, 0);

    //슬롯이 들어갈 아이템인벤토리자체
    public GameObject Iteminventory;
    //스킬슬롯이 들어갈 스킬인벤토리자체
    public GameObject Skillinventory;

    public GameObject Skillpartinventory;


    //db의 자신id의 모든 아이템을 담아둘 리스트
    public List<GameObject> itemobjlist;
    //db의 자신id의 모든 스킬을 담아둘 리스트
    public List<GameObject> skillobjlist;

    public List<GameObject> skillpartobjlist;

    //위쪽 무기 슬롯
    public GameObject weaponeslot;
    //위쪽 방어구 슬롯
    public GameObject armorslot;
    //위쪽 스킬 1 슬롯
    public GameObject skill1slot;
    //위쪽 스킬 2 슬롯
    public GameObject skill2slot;

    //아이템 눌렀을때 나오는 패널
    public GameObject itempanel;

    //스킬 눌렀을때 나오는 패널
    public GameObject skillpanel;

    public GameObject skillpartpanel;

    public GameObject skillpartnumberpanel;


    //스킬 패널에 있는 버튼 (스킬1등록)
    public GameObject bt1name;
    //스킬 패널에 있는 버튼 (스킬2등록)
    public GameObject bt2name;
    //스킬 패널에 있는 버튼 (스킬해제)
    public GameObject bt3name;



    //스킬 패널에 있는 버튼 (스킬1등록)
    public GameObject itembt1name;
    //스킬 패널에 있는 버튼 (스킬2등록)
    public GameObject itembt2name;
    //스킬 패널에 있는 버튼 (스킬해제)
    public GameObject itembt3name;


    //표시될 내 캐릭터의 빈 오브젝트
    public GameObject eqcharacter;
    //내 캐릭터를 실제로 만든 오브젝트
    public GameObject eqcharacterreal;

    //총합 공격력
    public GameObject totalatk;
    //총합 hp
    public GameObject totalhp;


    public List<int> newskilllist;
    public List<int> sellitemlist;
    public List<int> sellskilllist;
    public int resultgold;

    public Hashtable sellskillpartlist;


    void Awake()
    {
        itempanel = GameObject.Find("itempanel");
        skillpanel = GameObject.Find("skillpanel");
        skillpartpanel = GameObject.Find("skillpartpanel");
        skillpartnumberpanel = GameObject.Find("skillpartnumberpanel");


        bt1name = GameObject.Find("skill1bt");
        bt2name = GameObject.Find("skillsell");
        bt3name = GameObject.Find("skillcancel");

        itembt1name = GameObject.Find("equipbt");
        itembt2name = GameObject.Find("itemsell");
        itembt3name = GameObject.Find("equipnotbt");

        //스킬 , 아이템 패널 숨기기
        skillpanel.SetActive(false);
        itempanel.SetActive(false);
        skillpartpanel.SetActive(false);
        skillpartnumberpanel.SetActive(false);

        skillobjlist = new List<GameObject>();
        itemobjlist = new List<GameObject>();
        skillpartobjlist = new List<GameObject>();

        Iteminventory = GameObject.Find("Iteminventory");
        Skillinventory = GameObject.Find("Skillinventory");
        Skillpartinventory = GameObject.Find("Skillpartinventory");
        totalatk = GameObject.Find("totalatk");
        totalhp = GameObject.Find("totalhp");


        newskilllist = new List<int>();
        sellitemlist = new List<int>();
        sellskilllist = new List<int>();
        sellskillpartlist = new Hashtable();

        StartCoroutine(itemcheck());

    }


    // Start is called before the first frame update
    void Start()
    {
        //내 캐릭터 만들기
        eqcharacter = Resources.Load<GameObject>("equipment_player");

        eqcharacterreal = Instantiate(eqcharacter);

        //시작할 때 저장한 장착하고 있는 아이템의 id를 확인한.
        string gunid_pp = PlayerPrefs.GetString("gunid");

        string armorid_pp = PlayerPrefs.GetString("armorid");

        string totalpower_pp = PlayerPrefs.GetString("totalpower");

        string totalhp_pp = PlayerPrefs.GetString("totalhp");

        //시작할때 저장한 최대 공격력 최대 체력을 저장한다
        totalatk.GetComponent<Text>().text = totalpower_pp;
        totalhp.GetComponent<Text>().text = totalhp_pp;

        //캐릭터의 손
        GameObject weaponhand = GameObject.FindWithTag("weaponhand");

        if (!gunid_pp.Equals("0")) {

            //캐릭터가 장비하고 있는 총의 이름을 만듦
            string gunname = "gun" + gunid_pp;

            //총 오브젝트를 만든다
            GameObject handgun_ob = Resources.Load<GameObject>("Weapone/" + gunname);
            GameObject handgun = Instantiate(handgun_ob);


            Vector3 gunpos = new Vector3(0, 0, 0);
            Quaternion gunrot = Quaternion.Euler(0, 90, -90);
            Vector3 gunscale = new Vector3(3, 1, 1);

            //총을 손의 하위로 만든다
            handgun.transform.SetParent(weaponhand.transform);
            //총의 스케일 , 위치 , 방향을 지정해줌
            handgun.transform.localScale = gunscale;
            handgun.transform.localPosition = gunpos;
            handgun.transform.localRotation = gunrot;
        }

        resultgold = PlayerPrefs.GetInt("gold");
        string nickname = PlayerPrefs.GetString("nickname");

        GameObject.Find("goldtext").GetComponent<Text>().text = "G : " + resultgold.ToString();

        GameObject.Find("nicknametext").GetComponent<Text>().text = nickname;



    }


    IEnumerator itemcheck()
    {
        //서버에서 케릭터의 아이템 리스트를 받아온다

        string id = PlayerPrefs.GetString("id");
        Debug.Log("id" + id);

        string pw = PlayerPrefs.GetString("pw");
        Debug.Log("pw" + pw);

        int userindex = PlayerPrefs.GetInt("userindex");


        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("id", id);
        w.AddField("pw", pw);
        w.AddField("userindex", userindex);
        Debug.Log("2 , form 구성 아이디 비밀번호 로그인방식");


        Debug.Log("3 , register.php에 form 전달");
        UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/useritem.php", w);

        Debug.Log("4 , 통신 반환값 전달");
        yield return www.SendWebRequest();


        //서버 연결 실패
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("5, 네트워크 통신 error: " + www.error);

        }
        else //서버 연결 성공
        {

            result = www.downloadHandler.text;
            Debug.Log("5, 네트워크 통신 성공 result: " + result);

            //json 데이터로 아이템 데이터, 스킬데이터를 받아온다
            var jsonPlayer = JSON.Parse(result);

            //useritemData 로 아이템에 대한 정보를 받을 수 있다
            int useritemDatalength = jsonPlayer["useritemData"].Count;

            //전체 아이템 수만큼 반복문으로 슬롯을 만든다
            for (int i=0; i< useritemDatalength; i++) {

              var useritemData = jsonPlayer["useritemData"][i];

                Debug.Log("useritemData : " + useritemData["itemname"]);

                //인벤토리에 프리팹을 만들고 스크립트에 내용입, 이미지 적용
                itemslot = Resources.Load<GameObject>("itemslot");

                //아이템 슬롯을 오브젝트로 만들기
                GameObject item = Instantiate(itemslot);

                //아이템 리스트로 만들기
                itemobjlist.Add(item);

                // GameObject.Find("itemslot(Clone)").name = useritemData["itemid"];
                // GameObject.Find(useritemData["itemid"]).transform.SetParent(Iteminventory.transform);
                // GameObject.Find(useritemData["itemid"]).transform.localScale = new Vector3(1, 1, 1);
                // GameObject.Find(useritemData["itemid"]).GetComponent<iteminfo>().atk = "10"; 
               
                //아이템 슬롯을 인벤토리의 하위로 만든다
                item.transform.SetParent(Iteminventory.transform);
                item.transform.localScale = new Vector3(1, 1, 1);
                // test.transform.GetComponent<iteminfo>().atk = "10";
                string itemtype = useritemData["itemtype"];
                string itemid = useritemData["itemid"];

                //인벤토리의 아이템 슬롯에 이미지를 적용
                string imagename = itemtype + itemid;
                RawImage itemimage = item.GetComponent<RawImage>();
                Texture2D itemtexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
                itemimage.texture = itemtexture;


                //아이템창 슬롯에 있는 iteminfo에 데이터 저장
                item.GetComponent<iteminfo>().itemname = useritemData["itemname"];
                item.GetComponent<iteminfo>().itemtype = useritemData["itemtype"];
                item.GetComponent<iteminfo>().itemid = useritemData["itemid"];
                item.GetComponent<iteminfo>().itemdescription = useritemData["itemdescription"];
                item.GetComponent<iteminfo>().atk = useritemData["atk"];
                item.GetComponent<iteminfo>().def = useritemData["def"];
                item.GetComponent<iteminfo>().atkspeed = useritemData["atkspeed"];
                item.GetComponent<iteminfo>().equiped = useritemData["equiped"];
                item.GetComponent<iteminfo>().rare = useritemData["rare"];
                item.GetComponent<iteminfo>().price = useritemData["price"];
                item.GetComponent<iteminfo>().useritemindex = useritemData["useritemindex"];
                item.GetComponent<iteminfo>().listindex = i;


                if (useritemData["equiped"].Equals("0")) {
                    //장비를 착용하지 않았을때
                    //착용 안한 아이템은 저장하고 끝
                }

                else if(useritemData["equiped"].Equals("1"))
                {
                    //장비를 착용했을 때
                    //착용했으면 착용한 장비가 안보이게 하고 위쪽에 장착 슬롯에 아이템을 보낸다
                    item.SetActive(false);

                    if (useritemData["itemtype"].Equals("gun")) {

                        //장착한 아이템이 총인지 갑옷인지에 따라 이미지와 내용을 저장한다
                        weaponeslot = GameObject.Find("weaponeslot");
                        RawImage weaponeimage = weaponeslot.GetComponent<RawImage>();
                        Texture2D weaponetexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
                        weaponeimage.texture = weaponetexture;
                        weaponeimage.color = new Color(255,255,255);

                        weaponeslot.GetComponent<guninfo>().itemname = useritemData["itemname"];
                        weaponeslot.GetComponent<guninfo>().itemtype = useritemData["itemtype"];
                        weaponeslot.GetComponent<guninfo>().itemid = useritemData["itemid"];
                        weaponeslot.GetComponent<guninfo>().itemdescription = useritemData["itemdescription"];
                        weaponeslot.GetComponent<guninfo>().atk = useritemData["atk"];
                        weaponeslot.GetComponent<guninfo>().def = useritemData["def"];
                        weaponeslot.GetComponent<guninfo>().atkspeed = useritemData["atkspeed"];
                        weaponeslot.GetComponent<guninfo>().equiped = useritemData["equiped"];
                        weaponeslot.GetComponent<guninfo>().rare = useritemData["rare"];
                        weaponeslot.GetComponent<guninfo>().price = useritemData["price"];
                        weaponeslot.GetComponent<guninfo>().useritemindex = useritemData["useritemindex"];
                        weaponeslot.GetComponent<guninfo>().listindex = i;



                    }
                    else if (useritemData["itemtype"].Equals("armor")) {

                        //장착한 아이템이 총인지 갑옷인지에 따라 이미지와 내용을 저장한다
                        armorslot = GameObject.Find("armorslot");
                        RawImage armorimage = armorslot.GetComponent<RawImage>();
                        Texture2D armortexture = Resources.Load<Texture2D>("Itemimage/" + imagename);
                        armorimage.texture = armortexture;
                        armorimage.color = new Color(255, 255, 255);

                        armorslot.GetComponent<armorinfo>().itemname = useritemData["itemname"];
                        armorslot.GetComponent<armorinfo>().itemtype = useritemData["itemtype"];
                        armorslot.GetComponent<armorinfo>().itemid = useritemData["itemid"];
                        armorslot.GetComponent<armorinfo>().itemdescription = useritemData["itemdescription"];
                        armorslot.GetComponent<armorinfo>().atk = useritemData["atk"];
                        armorslot.GetComponent<armorinfo>().def = useritemData["def"];
                        armorslot.GetComponent<armorinfo>().atkspeed = useritemData["atkspeed"];
                        armorslot.GetComponent<armorinfo>().equiped = useritemData["equiped"];
                        armorslot.GetComponent<armorinfo>().rare = useritemData["rare"];
                        armorslot.GetComponent<armorinfo>().price = useritemData["price"];
                        armorslot.GetComponent<armorinfo>().useritemindex = useritemData["useritemindex"];
                        armorslot.GetComponent<armorinfo>().listindex = i;

                    }

                }


            }

            //스킬창 만들기
            int userskillDatalength = jsonPlayer["userskillData"].Count;


            // Debug.Log("useritemDatasize : " + useritemDatalength);

            //userskillData스킬을 확인
            //총 스킬 개수 만큼 반복하며 스킬 슬롯을 만든다
            for (int i = 0; i < userskillDatalength; i++)

            {
                //일단 슬롯을 다 만든다
                var userskillData = jsonPlayer["userskillData"][i];
                //스킬 데이터를 슬롯으로 만든다 
                Debug.Log("useritemData : " + userskillData["skillname"]);

                //인벤토리에 프리팹을 만들고 스크립트에 내용입, 이미지 적용
                skillslot = Resources.Load<GameObject>("skillslot");

                //아이템 슬롯을 오브젝트로 만들기
                GameObject skill = Instantiate(skillslot);

                //아이템 리스트로 만들기
                skillobjlist.Add(skill);

                // GameObject.Find("itemslot(Clone)").name = useritemData["itemid"];
                // GameObject.Find(useritemData["itemid"]).transform.SetParent(Iteminventory.transform);
                // GameObject.Find(useritemData["itemid"]).transform.localScale = new Vector3(1, 1, 1);
                // GameObject.Find(useritemData["itemid"]).GetComponent<iteminfo>().atk = "10"; 

                //스킬슬롯을 스킬 인벤토리의 하위로 만든다
                skill.transform.SetParent(Skillinventory.transform);
                skill.transform.localScale = new Vector3(1, 1, 1);

                // test.transform.GetComponent<iteminfo>().atk = "10";
                string skilltype = userskillData["skilltype"];
                string skillid = userskillData["skillid"];

                string imagename = skilltype + skillid;
                if (userskillData["skilltype2"].Equals("active")) {

                    imagename = "skill" + skillid;


                }else if (userskillData["skilltype2"].Equals("passive")) {

                    imagename = "ability" + skillid;

                }



                //스킬 인벤토리의 슬롯에 이미지를 적용한다
                RawImage skillimage = skill.GetComponent<RawImage>();
                Texture2D skilltexture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                skillimage.texture = skilltexture;
               // skillimage.color = new Color(255, 255, 255);

                //스킬 인벤토리에 개수를 표시 -> 이건 파편으로 바꾸고 재료 인벤토리에서 처리하자
                var skillcount = skill.transform.GetChild(0);
                Text skillcount_tx = skillcount.GetComponent<Text>();
               // string skillcount_st = "x" + userskillData["skillcount"];
               // skillcount_tx.text = skillcount_st;

                skill.GetComponent<skillinfo>().skillname = userskillData["skillname"];
                skill.GetComponent<skillinfo>().skilltype = userskillData["skilltype"];
                skill.GetComponent<skillinfo>().skilltype2 = userskillData["skilltype2"];
                skill.GetComponent<skillinfo>().skillid = userskillData["skillid"];
                skill.GetComponent<skillinfo>().skilldescription = userskillData["skilldescription"];
                skill.GetComponent<skillinfo>().atk = userskillData["atk"];
                skill.GetComponent<skillinfo>().skillcount = userskillData["skillcount"];
                skill.GetComponent<skillinfo>().equiped = userskillData["equiped"];
                skill.GetComponent<skillinfo>().price = userskillData["price"];
                skill.GetComponent<skillinfo>().useritemindex = userskillData["useritemindex"];
                skill.GetComponent<skillinfo>().listindex = i;



                if (userskillData["equiped"].Equals("0"))
                {
                    //장비를 착용하지 않았을때
                }
                else if (userskillData["equiped"].Equals("1"))
                {
                    /*

                    //skill1에 착용했을 때 위쪽 슬롯에 이미지 적용과 데이터 저장
                    skill1slot = GameObject.Find("skill1slot");
                    RawImage skill1image = skill1slot.GetComponent<RawImage>();
                    Texture2D skill1texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                    skill1image.texture = skill1texture;
                    skill1image.color = new Color(255, 255, 255);

                    skill1slot.GetComponent<skill1info>().skillname = userskillData["skillname"];
                    skill1slot.GetComponent<skill1info>().skilltype = userskillData["skilltype"];
                    skill1slot.GetComponent<skill1info>().skillid = userskillData["skillid"];
                    skill1slot.GetComponent<skill1info>().skilldescription = userskillData["skilldescription"];
                    skill1slot.GetComponent<skill1info>().atk = userskillData["atk"];
                    skill1slot.GetComponent<skill1info>().skillcount = userskillData["skillcount"];
                    skill1slot.GetComponent<skill1info>().equiped = userskillData["equiped"];
                    skill1slot.GetComponent<skill1info>().listindex = i;
                    */
                    skill.SetActive(false);
                    if (userskillData["skilltype2"].Equals("active"))
                    {
                        skill1slot = GameObject.Find("skill1slot");
                        RawImage skill1image = skill1slot.GetComponent<RawImage>();
                        Texture2D skill1texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                        skill1image.texture = skill1texture;
                        skill1image.color = new Color(255, 255, 255);

                        skill1slot.GetComponent<skill1info>().skillname = userskillData["skillname"];
                        skill1slot.GetComponent<skill1info>().skilltype = userskillData["skilltype"];
                        skill1slot.GetComponent<skill1info>().skilltype2 = userskillData["skilltype2"];
                        skill1slot.GetComponent<skill1info>().skillid = userskillData["skillid"];
                        skill1slot.GetComponent<skill1info>().skilldescription = userskillData["skilldescription"];
                        skill1slot.GetComponent<skill1info>().atk = userskillData["atk"];
                        skill1slot.GetComponent<skill1info>().skillcount = userskillData["skillcount"];
                        skill1slot.GetComponent<skill1info>().equiped = userskillData["equiped"];
                        skill1slot.GetComponent<skill1info>().price = userskillData["price"];
                        skill1slot.GetComponent<skill1info>().useritemindex = userskillData["useritemindex"];
                        skill1slot.GetComponent<skill1info>().listindex = i;
                 
                       }else if (userskillData["skilltype2"].Equals("passive")) {

                        skill2slot = GameObject.Find("skill2slot");
                        RawImage skill2image = skill2slot.GetComponent<RawImage>();
                        Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                        skill2image.texture = skill2texture;
                        skill2image.color = new Color(255, 255, 255);

                        skill2slot.GetComponent<skill2info>().skillname = userskillData["skillname"];
                        skill2slot.GetComponent<skill2info>().skilltype = userskillData["skilltype"];
                        skill2slot.GetComponent<skill2info>().skilltype2 = userskillData["skilltype2"];
                        skill2slot.GetComponent<skill2info>().skillid = userskillData["skillid"];
                        skill2slot.GetComponent<skill2info>().skilldescription = userskillData["skilldescription"];
                        skill2slot.GetComponent<skill2info>().atk = userskillData["atk"];
                        skill2slot.GetComponent<skill2info>().skillcount = userskillData["skillcount"];
                        skill2slot.GetComponent<skill2info>().equiped = userskillData["equiped"];
                        skill2slot.GetComponent<skill2info>().price = userskillData["price"];
                        skill2slot.GetComponent<skill2info>().useritemindex = userskillData["useritemindex"];
                        skill2slot.GetComponent<skill2info>().listindex = i;

                    }
                }
                /*
                else if (userskillData["equiped"].Equals("2"))
                {

                    skill.SetActive(false);

                    //skill2에 착용했을 때 위쪽 슬롯에 이미지 적용과 데이터 저장
                    skill2slot = GameObject.Find("skill2slot");
                    RawImage skill2image = skill2slot.GetComponent<RawImage>();
                    Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                    skill2image.texture = skill2texture;
                    skill2image.color = new Color(255, 255, 255);

                    skill2slot.GetComponent<skill2info>().skillname = userskillData["skillname"];
                    skill2slot.GetComponent<skill2info>().skilltype = userskillData["skilltype"];
                    skill2slot.GetComponent<skill2info>().skillid = userskillData["skillid"];
                    skill2slot.GetComponent<skill2info>().skilldescription = userskillData["skilldescription"];
                    skill2slot.GetComponent<skill2info>().atk = userskillData["atk"];
                    skill2slot.GetComponent<skill2info>().skillcount = userskillData["skillcount"];
                    skill2slot.GetComponent<skill2info>().equiped = userskillData["equiped"];
                    skill2slot.GetComponent<skill2info>().listindex = i;


                }

                else if (userskillData["equiped"].Equals("3"))
                {
                    //skill1,skill2에 착용했을 때
                    //위쪽 슬롯에 이미지 적용과 데이터 저장
                    skill1slot = GameObject.Find("skill1slot");
                    RawImage skill1image = skill1slot.GetComponent<RawImage>();
                    Texture2D skill1texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                    skill1image.texture = skill1texture;
                    skill1image.color = new Color(255, 255, 255);

                    skill1slot.GetComponent<skill1info>().skillname = userskillData["skillname"];
                    skill1slot.GetComponent<skill1info>().skilltype = userskillData["skilltype"];
                    skill1slot.GetComponent<skill1info>().skillid = userskillData["skillid"];
                    skill1slot.GetComponent<skill1info>().skilldescription = userskillData["skilldescription"];
                    skill1slot.GetComponent<skill1info>().atk = userskillData["atk"];
                    skill1slot.GetComponent<skill1info>().skillcount = userskillData["skillcount"];
                    skill1slot.GetComponent<skill1info>().equiped = userskillData["equiped"];
                    skill1slot.GetComponent<skill1info>().listindex = i;

                    skill2slot = GameObject.Find("skill2slot");
                    RawImage skill2image = skill2slot.GetComponent<RawImage>();
                    Texture2D skill2texture = Resources.Load<Texture2D>("Skillimage/" + imagename);
                    skill2image.texture = skill2texture;
                    skill2image.color = new Color(255, 255, 255);

                    skill2slot.GetComponent<skill2info>().skillname = userskillData["skillname"];
                    skill2slot.GetComponent<skill2info>().skilltype = userskillData["skilltype"];
                    skill2slot.GetComponent<skill2info>().skillid = userskillData["skillid"];
                    skill2slot.GetComponent<skill2info>().skilldescription = userskillData["skilldescription"];
                    skill2slot.GetComponent<skill2info>().atk = userskillData["atk"];
                    skill2slot.GetComponent<skill2info>().skillcount = userskillData["skillcount"];
                    skill2slot.GetComponent<skill2info>().equiped = userskillData["equiped"];
                    skill2slot.GetComponent<skill2info>().listindex = i;


                }
                */


            }


            //스킬창 만들기
            int activepartdatalength = jsonPlayer["activepartData"].Count;


            // Debug.Log("useritemDatasize : " + useritemDatalength);

            //userskillData스킬을 확인
            //총 스킬 개수 만큼 반복하며 스킬 슬롯을 만든다
            for (int i = 0; i < activepartdatalength; i++) {


                var activepartData = jsonPlayer["activepartData"][i];
                //스킬 데이터를 슬롯으로 만든다 
                string itemid = activepartData["itemid"];

                string itemcount = activepartData["itemcount"];
                //인벤토리에 프리팹을 만들고 스크립트에 내용입, 이미지 적용
                skillpartslot = Resources.Load<GameObject>("skillpartslot");
                //아이템 슬롯을 오브젝트로 만들기
                GameObject partslot = Instantiate(skillpartslot);

                //아이템 리스트로 만들기
                 skillpartobjlist.Add(partslot);

                //스킬슬롯을 스킬 인벤토리의 하위로 만든다
                partslot.transform.SetParent(Skillpartinventory.transform);
                partslot.transform.localScale = new Vector3(1, 1, 1);

                string itemname = "skill" + itemid;


                RawImage itemimage = partslot.GetComponent<RawImage>();
                Texture2D itemtexture = Resources.Load<Texture2D>("Skillimage/" + itemname);
                itemimage.texture = itemtexture;

                partslot.transform.GetChild(1).gameObject.GetComponent<Text>().text = itemcount;

                var partinfo = partslot.GetComponent<skillpartinfo>();

                partinfo.skillid = activepartData["itemid"];
                partinfo.skillname = activepartData["skillname"];
                partinfo.skilltype = activepartData["itemtype"];
                partinfo.skillcount = activepartData["itemcount"];
                partinfo.skilltype2 = activepartData["skilltype2"];
                partinfo.skilldescription = activepartData["skilldescription"];
                partinfo.price = activepartData["price"];
                partinfo.useritemindex = activepartData["useritemindex"];
                partinfo.listindex = skillpartobjlist.Count-1;
                partinfo.atk= activepartData["atk"]; 
                partinfo.cooltime = activepartData["cooltime"]; 
                partinfo.equiped = "0";

         


            }

            //스킬창 만들기
            int pessivepartdatalength = jsonPlayer["pessivepartData"].Count;

            // Debug.Log("useritemDatasize : " + useritemDatalength);

            //userskillData스킬을 확인
            //총 스킬 개수 만큼 반복하며 스킬 슬롯을 만든다
            for (int i = 0; i < pessivepartdatalength; i++)
            {


                var pessivepartData = jsonPlayer["pessivepartData"][i];
                //스킬 데이터를 슬롯으로 만든다 
                string itemid = pessivepartData["itemid"];
                string itemcount = pessivepartData["itemcount"];

                //인벤토리에 프리팹을 만들고 스크립트에 내용입, 이미지 적용
                skillpartslot = Resources.Load<GameObject>("skillpartslot");
                //아이템 슬롯을 오브젝트로 만들기
                GameObject partslot = Instantiate(skillpartslot);

                //아이템 리스트로 만들기
                skillpartobjlist.Add(partslot);

                //스킬슬롯을 스킬 인벤토리의 하위로 만든다
                partslot.transform.SetParent(Skillpartinventory.transform);
                partslot.transform.localScale = new Vector3(1, 1, 1);

                string itemname = "ability" + itemid;
               
                RawImage itemimage = partslot.GetComponent<RawImage>();
                Texture2D itemtexture = Resources.Load<Texture2D>("Skillimage/" + itemname);
                itemimage.texture = itemtexture;

                partslot.transform.GetChild(1).gameObject.GetComponent<Text>().text = itemcount;

                var partinfo = partslot.GetComponent<skillpartinfo>();

                partinfo.skillid = pessivepartData["itemid"];
                partinfo.skillname = pessivepartData["skillname"];
                partinfo.skilltype = pessivepartData["itemtype"];
                partinfo.skillcount = pessivepartData["itemcount"];
                partinfo.skilltype2 = pessivepartData["skilltype2"];
                partinfo.skilldescription = pessivepartData["skilldescription"];
                partinfo.price = pessivepartData["price"];
                partinfo.useritemindex = pessivepartData["useritemindex"];
                partinfo.listindex = skillpartobjlist.Count - 1;
                partinfo.atk = pessivepartData["atk"]; 
                partinfo.cooltime = pessivepartData["cooltime"]; 
                partinfo.equiped = "0"; 
            }

        }

    }



}
