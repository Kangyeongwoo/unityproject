using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bossdata;
using FlatBuffers;
using System.Net.Sockets;
using System.Threading;

public class bossplayerskillfire : MonoBehaviour
{

    Transform playergunpos;

    int skill1id;

    GameObject skill1real;

    GameObject skill1;

    GameObject playergun;

    public static int skilled;

    Transform player;
    Transform target;
    private FlatBufferBuilder builder;
    private byte[] sendBuffer;
    private ByteBuffer sendbb;

    // Start is called before the first frame update
    void Start()
    {
        skilled = 0;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        var playerinfo = GameObject.FindWithTag("Player").GetComponent<bossplayerinfo>();
        skill1id = playerinfo.playerskill1id;
        playergun = GameObject.FindWithTag("Playergun");

    }

    public void skill()
    {
        if (Mathf.Approximately(bossplayercontrol.fillPercentage, 1f))

        {
            Debug.Log("skillstart");
            if (skill1id == 1)
            {
                target = GameObject.FindWithTag("enemy").GetComponent<Transform>();
                Debug.Log("skillstart2");
                skilled = 1;
                //skillfire = 1 데이터 전송
                string skillname = "skill" + skill1id.ToString();

                Vector3 vec = target.position - player.position;
                vec.Normalize();
                Debug.Log("target.localPosition:" + target.localPosition);

                //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                Quaternion q = Quaternion.LookRotation(vec);
                player.rotation = q;
                Debug.Log("player.rotation:" + q);



                //보내는 데이터 버퍼 (id, nickname, userindex, 위치, 방향, 목적지, 공격상태 , 이동 상태 ,맞은 상태  ,)
                builder = new FlatBufferBuilder(1024);
                var idoffset = builder.CreateString(PlayerPrefs.GetString("id"));
                var nicknameoffset = builder.CreateString(PlayerPrefs.GetString("nickname"));
                Player.StartPlayer(builder);
                Player.AddStartstate(builder, PlayerStart.Play);
                Player.AddUserindex(builder, PlayerPrefs.GetInt("userindex"));
                Player.AddId(builder, idoffset);
                Player.AddNickname(builder, nicknameoffset);
                Player.AddPlayerpos(builder, Vec3.CreateVec3(builder, player.position.x, player.position.y, player.position.z));
                Player.AddPlayerrot(builder, player.eulerAngles.y);
                Player.AddMovestate(builder, MoveState.Stop);
                Player.AddDestinationpos(builder, Vec3.CreateVec3(builder, player.position.x, player.position.y, player.position.z));
                Player.AddAttacked(builder, 0);
                Player.AddFire(builder, 0);
                Player.AddSkillfire(builder, 1);
                Offset<Player> pypplayer = Player.EndPlayer(builder);
                //   builder.Finish(pypplayer.Value);

                //   sendBuffer = builder.SizedByteArray();
                //   sendbb = new ByteBuffer(sendBuffer);


                Game.StartGame(builder);
                Game.AddPlayer(builder, pypplayer);
                Game.AddTablenum(builder, 0);
                Offset<Game> game = Game.EndGame(builder);

                builder.Finish(game.Value);
                sendBuffer = builder.SizedByteArray();
                sendbb = new ByteBuffer(sendBuffer);

                if (findteam.stream.CanWrite)
                {
                    findteam.stream.Write(sendBuffer, 0, sendBuffer.Length);
                    findteam.stream.Flush();
                }



                Debug.Log("skillplayerrot1:" + player.eulerAngles.y);
                Debug.Log("skillplayerrot2:" + player.localEulerAngles.y);
                Debug.Log("skillplayerrot3:" + player.rotation.y);
                Debug.Log("skillplayerrot4:" + player.localRotation.y);


                //총구에서 스킬이 나가도록
                playergunpos = GameObject.FindWithTag("Playergun").GetComponent<Transform>();

                skill1 = Resources.Load<GameObject>("Bossskill/" + skillname);


                Quaternion skillrot = Quaternion.Euler(90, playergunpos.transform.eulerAngles.y, 0);

                skill1real = Instantiate(skill1, playergunpos.transform.position, skillrot);

                bossplayercontrol.currentValue = 0f;
                bossplayercontrol.fillImage.fillAmount = 0;
                bossplayercontrol.fillPercentage = 0;

              //  skill1real.transform.SetParent(playergun.transform);

                StartCoroutine(delay());


            }

        }
    }


    public IEnumerator delay()
    {

        yield return new WaitForSeconds(1f);
       // Destroy(skill1real);
       // yield return new WaitForSeconds(0.2f);
        skilled = 0;

    }
}
