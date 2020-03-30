using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossstionbullet : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.forward * 0.3f);
    }
}
