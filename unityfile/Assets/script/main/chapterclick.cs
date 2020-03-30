using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chapterclick : MonoBehaviour
{
   public void chapterpanel() {

      var mainstart =  GameObject.Find("mainstart").GetComponent<mainstart>();

        mainstart.mapselectpanel.SetActive(true);
   }

    private void OnMouseDown()
    {
        var mainstart = GameObject.Find("mainstart").GetComponent<mainstart>();

        mainstart.mapselectpanel.SetActive(true);

        Destroy(this.gameObject);
    }
}
