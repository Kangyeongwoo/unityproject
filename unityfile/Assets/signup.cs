
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;



public class signup : MonoBehaviour {

    public InputField signup_id;
    public InputField signup_pw;
    public InputField signup_pw2;



public void signup_check()
{
    StartCoroutine(Upload());
}

IEnumerator Upload()
{

                WWWForm w = new WWWForm();
                w.AddField("select", "submit");
                w.AddField("id", signup_id.text);
                w.AddField("pw", signup_pw.text);
                w.AddField("pw2", signup_pw2.text);
      

    UnityWebRequest www = UnityWebRequest.Post("http://49.247.131.90/signup.php", w);
    yield return www.SendWebRequest();

    if (www.isNetworkError || www.isHttpError)
    {
        Debug.Log(www.error);
    }
    else
    {
            string result = www.downloadHandler.text;


    }
 

}
}

