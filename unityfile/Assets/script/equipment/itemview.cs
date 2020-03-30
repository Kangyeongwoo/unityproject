using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemview : MonoBehaviour
{

 


    public void cancel() {
        //아이템패널이 꺼짐
        GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().itempanel.SetActive(false);

    }

    public void skillcancel()
    {
        //스킬 패널이 꺼짐
        GameObject.Find("equipmentstart_cs").GetComponent<equipmentstart>().skillpanel.SetActive(false);

    }


}
