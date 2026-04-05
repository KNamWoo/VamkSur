using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; // 무기의 데미지
    public float per; //관통수치

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rigid.linearVelocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 트리거로 들어온게 적이거나 근접무기면 실행하지 않고 리턴
        if (!collision.CompareTag("Enemy") || per == -100)
        {
            return;
        }

        per--;

        if (per < 0)
        {
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area") || per == -100)
            return;
        
        gameObject.SetActive(false);
    }
}
