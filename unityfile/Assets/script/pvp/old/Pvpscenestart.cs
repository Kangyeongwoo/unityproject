using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Pvpscenestart : MonoBehaviour
{

    public GameObject joystick;
    public GameObject remotecontrol;
    public GameObject player = null;
    public GameObject enemy = null;
    public Transform maincamera;
    public static string enemyindex;

    private void Awake()
    {
       
        var jsonPlayer = JSON.Parse(Findenemy.receivedata);
        enemyindex = jsonPlayer["userindex"];
        if (enemyindex == "0")
        {
            //상대가 1p 내가 2P
         //  Vector3 enemyfirstpos = new Vector3(1.6f, -0.46f, -14.96f);
            Quaternion enemyfirstrot = Quaternion.Euler(0, 0, 0);
            Vector3 enemyfirstpos = new Vector3(0,0, -14f);

        //    Vector3 playerfirstpos = new Vector3(1.6f, -0.46f, 13f);
            Quaternion playerfirstrot = Quaternion.Euler(0, 180, 0);
            Vector3 playerfirstpos = new Vector3(0, 0, 13f);

            Vector3 camerafirstpos = new Vector3(-0.2f, 26.8f, 8f);
            Quaternion camerafirstrot = Quaternion.Euler(85, 180, 0);


            player = Resources.Load<GameObject>("pvpplayer");

            Instantiate(player, playerfirstpos, playerfirstrot);

            enemy = Resources.Load<GameObject>("pvpenemy");

            Instantiate(enemy, enemyfirstpos, enemyfirstrot);

            maincamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();

            maincamera.position = camerafirstpos;
            maincamera.rotation = camerafirstrot;

        }
        else if(enemyindex == "1") {
            //상대가 2p 내가 1p
         //   Vector3 playerfirstpos = new Vector3(1.6f,-0.46f,-14.96f);
            Quaternion playerfirstrot = Quaternion.Euler(0, 0, 0);
            Vector3 playerfirstpos = new Vector3(0, 0, -14f);

         //  Vector3 enemyfirstpos = new Vector3(1.6f, -0.46f, 13f);
            Quaternion enemyfirstrot = Quaternion.Euler(0, 180, 0);
            Vector3 enemyfirstpos = new Vector3(0, 0, 13f);

            Vector3 camerafirstpos = new Vector3(-0.2f, 26.8f, -6f);
            Quaternion camerafirstrot = Quaternion.Euler(85, 0, 0);

            player = Resources.Load<GameObject>("pvpplayer");

            Instantiate(player, playerfirstpos, playerfirstrot);

            enemy = Resources.Load<GameObject>("pvpenemy");

            Instantiate(enemy, enemyfirstpos, enemyfirstrot);

            maincamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();

            maincamera.position = camerafirstpos;
            maincamera.rotation = camerafirstrot;



        }

    }

  

}
