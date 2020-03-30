using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropitem : MonoBehaviour
{
    public int dropitemid;

    public Hashtable passiveskill = new Hashtable();

    public Hashtable activeskill = new Hashtable();

    public Hashtable passivescript = new Hashtable();

    private void Start()
    {

     


        activeskill.Add(1, "특대미사일 스킬조각");
        activeskill.Add(2, "보호막 스킬조각");

        passiveskill.Add(3, "사선총알 스킬조각");//
        passiveskill.Add(4, "전방총알 스킬조각");//
        passiveskill.Add(5, "후방총알 스킬조각");//
        passiveskill.Add(6, "측면총알 스킬조각");//
        passiveskill.Add(7, "블레이즈 스킬조각");//
        passiveskill.Add(8, "반동의벽 스킬조각");//
        passiveskill.Add(9, "관통 스킬조각");//


        passivescript.Add(3, "사선 총알 추가");
        passivescript.Add(4, "전방 총알 추가");
        passivescript.Add(5, "후방 총알 추가");
        passivescript.Add(6, "측면 총알 추가");
        passivescript.Add(7, "공격력 증가");
        passivescript.Add(8, "튕기는 총알");
        passivescript.Add(9, "관통하는 총알");
    }
}
