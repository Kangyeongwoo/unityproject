using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class remoteplayerhpbar : MonoBehaviour
{

    //몬스터 체력바에 나타나는 체력
    public Text remotehp_text;

    //몬스터의 체력바 
    public Slider remotehpbar;

    public int remotehp;

    // Start is called before the first frame update
    void Start()
    {
        //enemy의 enemyinfo 안에서 hp를 가지고 와서 표시해준다
        var remotedata = this.gameObject.GetComponent<bossremoteplayerinfo>();
        remotehp = remotedata.remoteplayerhp;
        remotehp_text.text = remotehp.ToString();
    }

}
