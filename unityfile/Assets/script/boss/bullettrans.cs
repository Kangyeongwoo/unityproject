using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullettrans : MonoBehaviour
{
    void Update()
    {
       
        transform.Translate(Vector3.up * 0.8f);
    }
}
