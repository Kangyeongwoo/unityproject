using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
//using LitJson;
using SimpleJSON;
using System.Net.Sockets;
using System.Threading;


public class Remotecontrol : MonoBehaviour
{
    //  public string receivedata;
    // Start is called before the first frame update
 
     //  GameObject remoteplayer = null;

    //플레이어 위치 , 방향 담을 vector 함수
    public Vector3 playerpos;

    public Vector3 destinationpos;

    public Quaternion playerrot;

    //  public Vector3 playerrot;

    public Transform remotetrans;

    public Rigidbody remoterigid;

    static public Socket clientSocket = null;

    public Animator anim2;

    public void Start()
    {
        anim2 = GameObject.FindWithTag("enemy").GetComponent<Animator>();
    }

    private void Update()
    {
    
            try
            {


                if (Control.receivedata != null)
                {
                    Debug.Log("test2" + Control.receivedata);

                    var jsonPlayer = JSON.Parse(Control.receivedata);


                            //현재 있는 원격 플레이어의 transform 받기
                            remotetrans = GameObject.FindWithTag("enemy").GetComponent<Transform>();
                             remoterigid = GameObject.FindWithTag("enemy").GetComponent<Rigidbody>();
                             if (jsonPlayer["movestate"] == "0")
                            {
                                anim2.SetBool("run",true);
                                float roty = float.Parse(jsonPlayer["playerrot"]);
                                Debug.Log(jsonPlayer["playerrot"]);
                                remotetrans.eulerAngles = new Vector3(0, roty, 0);
                                //remotetrans.transform.Translate(Vector3.forward * Time.deltaTime * 10f);

                                float[] destinationpos_js_array = new float[3];
                                for (int i = 0; i < 3; i++)
                                {
                                    destinationpos_js_array[i] = float.Parse(jsonPlayer["destinationpos"][i]);
                                }
                                destinationpos = new Vector3(destinationpos_js_array[0], destinationpos_js_array[1], destinationpos_js_array[2]);

                                remotetrans.transform.position = Vector3.MoveTowards(remotetrans.transform.position, destinationpos, 5f * Time.deltaTime);
                                
                   //             if(Vector3.Distance(remotetrans.transform.position, destinationpos) <= 0.5f) {
                  //      Debug.Log("Vector3.Distance"+ Vector3.Distance(remotetrans.transform.position, destinationpos));
                  //      anim2.SetBool("run", false);
                  //  }
                }
                            else if (jsonPlayer["movestate"] == "1")
                            {
                                anim2.SetBool("run", false);
                                remotetrans.transform.Translate(Vector3.forward * Time.deltaTime * 0f);
                                
                                float roty = float.Parse(jsonPlayer["playerrot"]);
                                remotetrans.eulerAngles = new Vector3(0, roty, 0);
                                float[] playerpos_js_array = new float[3];
                                for (int i = 0; i < 3; i++)
                                {

                                    playerpos_js_array[i] = float.Parse(jsonPlayer["playerpos"][i]);
                                }
                                playerpos = new Vector3(playerpos_js_array[0], playerpos_js_array[1], playerpos_js_array[2]);
                                Debug.Log("stop_playerpos" + playerpos);
                                Debug.Log("stop_playerpos_tr" + remotetrans.transform.position);
                                remotetrans.transform.position = Vector3.MoveTowards(remotetrans.transform.position, playerpos, 10f * Time.deltaTime);
                               

                }


                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        

    }
}