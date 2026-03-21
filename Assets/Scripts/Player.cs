using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec; // 키 입력 값 변수
    public float   speed;
    
    Rigidbody2D    rigid;
    SpriteRenderer sprite;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        /*
        // 힘 입력
        rigid.AddForce(inputVec);
        
        // 속도 제어
        rigid.velocity = inputVec;
        */
        
        // 위치 이동
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        
        // 플레이어 회전
        if (inputVec.x < 0)
        {
            sprite.flipX = true;
        }else if (inputVec.x > 0)
        {
            sprite.flipX = false;
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}