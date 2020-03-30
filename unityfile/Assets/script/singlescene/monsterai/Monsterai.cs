using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Monsterai : MonoBehaviour
{
    //몬스터의 nav 행동 네가지 
    public enum CurrentState { idle, trace, attack, dead };
    //가장 기본 행동은 idle
    public CurrentState curState = CurrentState.idle;

    //몬스터의 위치방향
    private Transform _transform;

    //플레이어의 위치방향
    private Transform playerTransform;

    //네비게이션 관련 컴포넌트
    private NavMeshAgent nvAgent;

    //최대 추적 거리 애니메이터 넣자
    public float traceDist = 40.0f;

    //최대 공격 거리 애니메이터 넣자
    public float attckDist = 2f;

    //죽었는지 살았는지 애니메이터 넣자
    private bool isDead = false;

    private Animation monsterani;


    public const string IDLE = "Anim_Idle";
    public const string RUN = "Anim_Run";
    public const string ATTACK = "Anim_Attack";
    public const string DAMAGE = "Anim_Damage";
    public const string DEATH = "Anim_Death";



    private void Start()
    {
        //몬스터의 위치정보 쓰기
        _transform = this.gameObject.GetComponent<Transform>();
        Debug.Log("monster x:" + _transform.position.x + " y: " + _transform.position.y + " z: " + _transform.position.z);

        //플레이어의 위치 쓰기
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Debug.Log("playerTransform x:" + playerTransform.position.x + " y: " + playerTransform.position.y + " z: " + playerTransform.position.z);

        //navmeshagent 쓰기
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        nvAgent.speed = 10f;

        monsterani = this.gameObject.GetComponent<Animation>();

        //이 시정에 CheckState() 시작 yield return 될 때 아래 꺼 실행
        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    IEnumerator CheckState()
    {

        while (!isDead)
        {
            //죽지 않았으면

            //0.2초 딜레이
            yield return new WaitForSeconds(0.1f);

            //몬스터와 플레이어 사이 거리 계산
            float dist = Vector3.Distance(playerTransform.position, _transform.position);
        //    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!dist:" + dist);

          
            if (dist <= attckDist)
            {
                //거리가 공격거리보다 작을 때 공격을 한다
                curState = CurrentState.attack;
               

            }
            else if (dist <= traceDist)
            {
                //거리가 공격 거리보다 크고 추격거리보다 작을 때 추격
                curState = CurrentState.trace;
               
            }
            else
            {
                //추적거리보다 멀면 idle상태
                curState = CurrentState.idle;
              
            }


        }

    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {

            switch (curState)
            {

                //현재 상태가 idle일때
                case CurrentState.idle:
                    //네비게이션 스탑
                    nvAgent.isStopped = true;
                    monsterani.CrossFade(IDLE);
                    break;

                //현재상태가 추적일때
                case CurrentState.trace:
                    //네비게이션 목적지는 플레이어 위치
                    monsterani.CrossFade(RUN);
                    nvAgent.destination = playerTransform.position;
                    //스탑을 해제
                    nvAgent.isStopped = false;

                    break;

                //공격상태일 때
                case CurrentState.attack:
                    nvAgent.isStopped = true;

                    Vector3 vec = playerTransform.position - _transform.position;
                    vec.Normalize();
              //      Debug.Log("target.localPosition:" + playerTransform.localPosition);

                    //타겟과 플레이어 사이의 벡터를 이용해 플레이어가 타겟을 바라보게 한다
                    Quaternion q = Quaternion.LookRotation(vec);
                    _transform.rotation = q;

                    monsterani.CrossFade(ATTACK);

                    break;

            }
            yield return null;
        }
    }
}
