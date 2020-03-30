using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleinventory : MonoBehaviour
{
    //id, 개수
    public static Hashtable passiveskillinventory = new Hashtable();

    public static Hashtable activeskillinventory = new Hashtable();

    public static int goldinventory = 0;

    public List<GameObject> itemtemp = new List<GameObject>();
}
