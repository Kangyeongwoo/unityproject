using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getsettext : MonoBehaviour
{

    public InputField id;
    public InputField pw;
    public InputField pw2;

    public void test() {

        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        Debug.Log(id.text + pw.text + pw2.text);
        yield return 1;
    }

}
