using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pvpdata;
using FlatBuffers;
using System.Net.Sockets;
using System.Threading;

public class Mapeffect_cs : MonoBehaviour
{
    public List<GameObject> floorlist = new List<GameObject>();
    public GameObject floorpac;
    public GameObject fire;
    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;
   

    private void Start()
    {
       
         floorpac = GameObject.Find("floorpac");
        for (int i=0; i<20; i++) {

            floorlist.Add(floorpac.transform.GetChild(i).gameObject);
           
         }
        if (Pvpscenestart_flat.enemyindex == 1)
        {
            StartCoroutine(mapeffect());
        }
    }

    IEnumerator mapeffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);

            Debug.Log("enemyindex" + Pvpscenestart_flat.enemyindex);

            int[] selectmappart = getRandomInt(5, 0, 19);


            builder = new FlatBufferBuilder(1024);
            Mapeffect.StartMapeffect(builder);
            Mapeffect.AddEffectnum1(builder, selectmappart[0]);
            Mapeffect.AddEffectnum2(builder, selectmappart[1]);
            Mapeffect.AddEffectnum3(builder, selectmappart[2]);
            Mapeffect.AddEffectnum4(builder, selectmappart[3]);
            Mapeffect.AddEffectnum5(builder, selectmappart[4]);

            Offset<Mapeffect> mapeffectarray = Mapeffect.EndMapeffect(builder);

                    Game.StartGame(builder);
                    Game.AddMapeffect(builder, mapeffectarray);
                    Game.AddTablenum(builder, 2);
              
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
           

            StartCoroutine(mapfire(selectmappart));


        }
    }



    IEnumerator mapfire(int[] selectmappart) {

        fire=Resources.Load<GameObject>("Plasma");
        List<GameObject> plasma = new List<GameObject>();

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                int number = selectmappart[i];
                GameObject selectpart = floorlist[number];
                selectpart.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < 5; i++)
            {
                int number = selectmappart[i];
                GameObject selectpart = floorlist[number];
                selectpart.GetComponent<MeshRenderer>().material.color = new Color(156/255f, 90/255f, 54/255f);
            }
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i < 5; i++)
        {
            int number = selectmappart[i];
            GameObject selectpart = floorlist[number];
            plasma.Add(Instantiate(fire , selectpart.GetComponent<Transform>().position, Quaternion.identity));

        }

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {
            Destroy(plasma[i]);

        }

    }








    //범위 내 중복없는 난수 발생기

    public int[] getRandomInt(int length, int min, int max) //max포함안함 => (length)<(max-min)이어야함

    {

        int[] randArray = new int[length];

        bool isSame;



        for (int i = 0; i < length; ++i)

        {

            while (true)

            {

                randArray[i] = Random.Range(min, max);

                isSame = false;



                for (int j = 0; j < i; ++j)

                {

                    if (randArray[j] == randArray[i])

                    {

                        isSame = true;

                        break;

                    }

                }

                if (!isSame) break;

            }

        }

        return randArray;

    }





}

