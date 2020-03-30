using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterbullettrans : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.forward * 0.8f);
    }
}
