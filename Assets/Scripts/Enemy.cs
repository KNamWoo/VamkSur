/*
    최종 변경일: 2026.03.29
    수정자
    - 김남우
    -

    목적
    - 적의 동작을 담당하는 스크립트
*/

using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float                       speed;  // 적의 이동 속도 (인스펙터에서 설정)
    public float                       curHP;  // 현재 HP
    public float                       maxHP;  // 최대 HP
    public Rigidbody2D                 target; // 추적 대상인 플레이어의 Rigidbody2D (인스펙터에서 연결)
    public RuntimeAnimatorController[] animCon;

    bool isLive; // 적이 살아있는지 여부 (false이면 이동 및 애니메이션을 중단)

    Rigidbody2D        rigid;   // 이 오브젝트의 Rigidbody2D 컴포넌트
    Collider2D         collider; // 이 오브젝트의 Collider2D 컴포넌트 (충돌 감지에 사용)
    SpriteRenderer     spriter; // 이 오브젝트의 SpriteRenderer 컴포넌트 (좌우 반전에 사용)
    Animator           anim;    // 이 오브젝트의 Animator 컴포넌트 (애니메이션 재생에 사용)
    WaitForFixedUpdate wait;    // 고정 프레임마다 대기하는 YieldInstruction (코루틴에서 사용)
    
    // 오브젝트 초기화. 필요한 컴포넌트를 가져오고 생존 상태를 true로 설정한다.
    void Awake()
    {
        rigid   = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim    = GetComponent<Animator>();
        wait    = new WaitForFixedUpdate();
    }
    
    // 물리 업데이트. 고정 프레임마다 플레이어 방향으로 이동한다.
    // MovePosition을 사용해 물리 기반으로 이동하며, 관성이 생기지 않도록 속도를 0으로 초기화한다.
    
    void FixedUpdate()
    {
        // 사망이나 피격, 게임이 멈춘 상태라면 이동하지 않음
        if (!GameManager.instance.isGameLive || !isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
            
        // 플레이어 위치에서 현재 위치를 빼서 이동 방향 벡터 계산
        Vector2 dirVec  = target.position - rigid.position;
        // 방향 벡터를 정규화한 뒤 속도와 물리 델타 타임을 곱해 이번 프레임의 이동량 계산
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        
        // 계산된 이동량만큼 물리적으로 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
        // MovePosition 후 남은 관성(velocity)을 제거해 미끄러짐 방지
        rigid.linearVelocity = Vector2.zero;
    }
    
    // 렌더링 직전 업데이트. 플레이어가 적의 왼쪽에 있으면 스프라이트를 좌우 반전시킨다.
    void LateUpdate()
    {
        // 사망이나 게임이 멈춘 상태라면 스프라이트 반전 처리 생략
        if (!isLive || !GameManager.instance.isGameLive)
            return;
        
        // 플레이어가 적보다 왼쪽에 있으면 flipX = true (스프라이트 좌우 반전)
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // 스크립트가 활성화되며 실행됨
    void OnEnable()
    {
        target               = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive               = true;
        collider.enabled     = true;
        rigid.simulated      = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        curHP                = maxHP;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHP = data.maxHP;
        curHP = maxHP;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        curHP -= collision.GetComponent<Bullet>().damage;
        StartCoroutine("KnockBack"); // = knockback()
        
        if (curHP > 0)
        {
            // 피격
            anim.Play("Hit");
        }
        else
        {
            // 적 사망
            isLive               = false; // 사망 상태로 전환하여 이동 및 애니메이션 중단
            collider.enabled     = false;
            rigid.simulated      = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 프레임까지 대기
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3f, ForceMode2D.Impulse); // 넉백 효과 적용 (힘의 크기와 방향 조절)
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
