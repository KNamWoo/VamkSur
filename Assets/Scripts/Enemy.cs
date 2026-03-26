using System;
using UnityEngine;

/// <summary>
/// 적(Enemy) 캐릭터의 동작을 담당하는 스크립트.
/// 플레이어를 향해 일정 속도로 추적하고, 플레이어 방향에 따라 스프라이트를 반전시킨다.
/// </summary>
public class Enemy : MonoBehaviour
{
    public float       speed;  // 적의 이동 속도 (인스펙터에서 설정)
    public Rigidbody2D target; // 추적 대상인 플레이어의 Rigidbody2D (인스펙터에서 연결)

    bool isLive; // 적이 살아있는지 여부 (false이면 이동 및 애니메이션을 중단)

    Rigidbody2D    rigid;   // 이 오브젝트의 Rigidbody2D 컴포넌트
    SpriteRenderer spriter; // 이 오브젝트의 SpriteRenderer 컴포넌트 (좌우 반전에 사용)

    /// <summary>
    /// 오브젝트 초기화. 필요한 컴포넌트를 가져오고 생존 상태를 true로 설정한다.
    /// </summary>
    void Awake()
    {
        rigid   = this.GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        
        isLive = true; // 기본적으로 생존 상태로 시작
    }

    /// <summary>
    /// 물리 업데이트. 고정 프레임마다 플레이어 방향으로 이동한다.
    /// MovePosition을 사용해 물리 기반으로 이동하며, 관성이 생기지 않도록 속도를 0으로 초기화한다.
    /// </summary>
    void FixedUpdate()
    {
        // 사망 상태라면 이동하지 않음
        if (!isLive)
        {
            return;
        }
            
        // 플레이어 위치에서 현재 위치를 빼서 이동 방향 벡터 계산
        Vector2 dirVec  = target.position - rigid.position;
        // 방향 벡터를 정규화한 뒤 속도와 물리 델타 타임을 곱해 이번 프레임의 이동량 계산
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        
        // 계산된 이동량만큼 물리적으로 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
        // MovePosition 후 남은 관성(velocity)을 제거해 미끄러짐 방지
        rigid.linearVelocity = Vector2.zero;
    }

    /// <summary>
    /// 렌더링 직전 업데이트. 플레이어가 적의 왼쪽에 있으면 스프라이트를 좌우 반전시킨다.
    /// </summary>
    void LateUpdate()
    {
        // 사망 상태라면 스프라이트 반전 처리 생략
        if (!isLive)
        {
            return;
        }
        
        // 플레이어가 적보다 왼쪽에 있으면 flipX = true (스프라이트 좌우 반전)
        spriter.flipX = target.position.x < rigid.position.x;
    }
}
