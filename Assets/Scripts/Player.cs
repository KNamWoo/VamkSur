using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec; // 키 입력 값
    public float   speed;    // 플레이어의 걷는 속도
    public float   curspeed; // 현재 속도
    public bool    isSprint; // 달리는 상태
    
    Rigidbody2D    rigid; // 플레이어의 rigidbody를 받아오기 위한 변수
    SpriteRenderer sprite; // 플레이어의 sprite를 제어하기위한 변수
    Animator       anim; // 플레이어의 animation 제어를 위한 변수

    void Awake()
    {
        rigid    = GetComponent<Rigidbody2D>();
        sprite   = GetComponent<SpriteRenderer>();
        anim     = GetComponent<Animator>();
        isSprint = false;
    }

    void FixedUpdate()
    {
        /*
        // 힘 입력
        rigid.AddForce(inputVec);
        
        // 속도 제어
        rigid.velocity = inputVec;
        */

        if (isSprint)
        {
            curspeed = speed * 1.5f;
        }
        else
        {
            curspeed = speed;
        }
        
        // 위치 이동
        Vector2 nextVec = inputVec * curspeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        // 플레이어 회전
        if (inputVec.x != 0)
        {
            sprite.flipX = inputVec.x < 0;
        }
        
        // 플레이어 애니메이션
        if (inputVec.x != 0 || inputVec.y != 0)
        {
            anim.Play("Run");
        }
        else
        {
            anim.Play("Stand");
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        isSprint = !isSprint;
    }
}