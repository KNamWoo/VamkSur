/*
    최종 변경일: 2026.03.29
    수정자
    - 김남우
    -

    목적
    - 플레이어 캐릭터의 전반적인 작동을 담당
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2                     inputVec; // 플레이어의 이동 입력 방향 벡터 (WASD 또는 방향키 입력값)
    public float                       speed;    // 기본 이동 속도 (인스펙터에서 설정)
    public float                       curspeed; // 현재 실제 이동 속도 (스프린트 여부에 따라 변동)
    public bool                        isSprint; // 스프린트 상태 여부 (true이면 속도 1.5배 적용)
    public Scanner                     scanner;
    public Hand[]                      hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D    rigid;  // 물리 이동에 사용할 Rigidbody2D 컴포넌트
    SpriteRenderer sprite; // 좌우 이동 방향에 따른 스프라이트 반전에 사용할 SpriteRenderer
    Animator       anim;   // 이동/정지 상태에 따른 애니메이션 재생에 사용할 Animator

    // 오브젝트 초기화. 필요한 컴포넌트를 가져오고 기본 상태를 설정한다.
    void Awake()
    {
        rigid    = GetComponent<Rigidbody2D>();
        sprite   = GetComponent<SpriteRenderer>();
        anim     = GetComponent<Animator>();
        isSprint = false; // 시작 시 스프린트 비활성화
        scanner = GetComponent<Scanner>();
        hands    = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        speed                          *= Character.Speed;
        anim.runtimeAnimatorController =  animCon[GameManager.instance.playerID];
    }

    // 물리 업데이트. 고정 프레임마다 플레이어를 입력 방향으로 이동시킨다.
    // 스프린트 상태에 따라 이동 속도를 1.5배 증가시킨다.
    void FixedUpdate()
    {
        // 게임이 멈췄을때 움직이지 않도록
        if(!GameManager.instance.isGameLive)
            return;
        
        /*
        // [미사용] 힘 기반 이동 (관성이 남아 미끄러지는 느낌)
        rigid.AddForce(inputVec);

        // [미사용] 속도 직접 설정 (물리 무시, 즉각적인 이동)
        rigid.velocity = inputVec;
        */

        // 스프린트 중이면 기본 속도의 1.5배, 아니면 기본 속도 적용
        if (isSprint)
        {
            curspeed = speed * 1.5f;
        }
        else
        {
            curspeed = speed;
        }

        // 입력 벡터 * 현재 속도 * 물리 프레임 시간 = 이번 프레임의 이동량
        Vector2 nextVec = inputVec * curspeed * Time.fixedDeltaTime;
        // 계산된 이동량만큼 물리적으로 위치 이동 (다른 콜라이더와 상호작용 유지)
        rigid.MovePosition(rigid.position + nextVec);
    }

    // 렌더링 직전 업데이트. 이동 방향에 따라 스프라이트를 반전하고 애니메이션을 재생한다.
    void LateUpdate()
    {
        // 게임이 멈췄을때 움직이지 않도록
        if(!GameManager.instance.isGameLive)
            return;
        
        // 좌우 입력이 있을 때만 스프라이트 반전 처리 (제자리 정지 시 방향 유지)
        if (inputVec.x != 0)
        {
            // 왼쪽 입력이면 flipX = true (스프라이트 좌우 반전), 오른쪽이면 false
            sprite.flipX = inputVec.x < 0;
        }

        // 이동 입력이 있으면 달리기 애니메이션, 없으면 기본 서있는 애니메이션 재생
        if (inputVec.x != 0 || inputVec.y != 0)
        {
            anim.Play("Run");
        }
        else
        {
            anim.Play("Stand");
        }
    }

    // Unity Input System에서 "Move" 액션이 발생할 때 자동으로 호출된다.
    // WASD 또는 방향키 입력값을 Vector2로 받아 inputVec에 저장한다.
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    // Unity Input System에서 "Sprint" 액션이 발생할 때 자동으로 호출된다.
    // 호출될 때마다 스프린트 상태를 토글(켜기/끄기)한다.
    void OnSprint(InputValue value)
    {
        isSprint = !isSprint; // 스프린트 상태 토글
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isGameLive)
            return;
        
        GameManager.instance.HP -= Time.deltaTime * 10f;
        
        if(GameManager.instance.HP <= 0)
        {
            for(int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.Play("Dead");
            GameManager.instance.GameOver();
        }
    }
}