using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pvpdata;
using FlatBuffers;
using System.Net.Sockets;
using System.Threading;

public class hprestorespawn : MonoBehaviour
{

    public GameObject hprestore;
    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;
    public static List<GameObject> hpobjlist = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (Pvpscenestart_flat.enemyindex == 1) {
            hprestore = Resources.Load<GameObject>("hprestore");
            StartCoroutine(hpsqawn());
        }
       
            
    }


    IEnumerator hpsqawn() {
        while (true)
        {
            if (hpobjlist.Count <= 10)
            {

                yield return new WaitForSeconds(5f);

                Debug.Log("enemyindex" + Pvpscenestart_flat.enemyindex);

                float randomx = Random.Range(-6, 6);

                float randomz = Random.Range(-6, 6);

                Vector3 hppos = new Vector3(randomx, 0f, randomz);



                builder = new FlatBufferBuilder(1024);
                Item.StartItem(builder);
                Item.AddItempos(builder, Vec3.CreateVec3(builder, randomx, 0f, randomz));


                Offset<Item> itemoffset = Item.EndItem(builder);

                Game.StartGame(builder);
                Game.AddItem(builder, itemoffset);
                Game.AddTablenum(builder, 1);

                Offset<Game> game = Game.EndGame(builder);

                builder.Finish(game.Value);
                sendBuffer = builder.SizedByteArray();
                sendbb = new ByteBuffer(sendBuffer);

                if (Findenemy_flat.stream.CanWrite)
                {
                    //데이터를 서버에 스트림으로 보낸다 bytearray
                    Findenemy_flat.stream.Write(sendBuffer, 0, sendBuffer.Length);
                    Findenemy_flat.stream.Flush();
                }



                Debug.Log("enemyindex2" + Pvpscenestart_flat.enemyindex);

                GameObject test = Instantiate(hprestore, hppos, Quaternion.identity);

                hpobjlist.Add(test);

                Debug.Log("enemyindex3" + Pvpscenestart_flat.enemyindex);

            }
        }
    }
}
