using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float       speed; // Enemy의 속도
    public Rigidbody2D target; // 플레이어

    bool isLive;

    Rigidbody2D    rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        
        isLive = true;
    }

    void FixedUpdate()
    {
        if (!isLive)
        {
            return;
        }
            
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive)
        {
            return;
        }
        
        spriter.flipX = target.position.x < rigid.position.x;
    }
}
