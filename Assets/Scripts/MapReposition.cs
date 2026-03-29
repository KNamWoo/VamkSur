/*
    최종 변경일: 2026.03.29
    수정자
    - 김남우
    -

    목적
    - 맵이나 적이 카메라를 벗어났을 때 플레이어의 이동 방향으로 재배치. 무한 반복 맵처럼 보이도록 구현한다
*/

using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapReposition : MonoBehaviour
{
    // GameManager를 통해 가져온 Player 스크립트 참조 (플레이어 위치·입력 벡터 접근용)
    private Player player;
    
    // 이 오브젝트의 Collider2D (적 오브젝트의 활성 여부 확인에 사용)
    Collider2D coll;
    
    // 게임 시작 시 GameManager에서 플레이어 참조를 가져온다.
    // (GameManager.instance가 Awake에서 초기화되므로 Start에서 접근)
    void Start()
    {
        player = GameManager.instance.player;
    }

    
    // 오브젝트 초기화. 이 오브젝트의 Collider2D를 가져온다.
    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    
    // 이 오브젝트가 "Area" 태그를 가진 트리거 콜라이더 영역을 벗어났을 때 호출된다.
    // 오브젝트의 태그에 따라 재배치 방식이 다르다:
    //   - "Ground": 플레이어와의 거리 차이가 큰 축 방향으로 40유닛 이동 (무한 맵 구현)
    //   - "Enemy" : 플레이어 이동 방향으로 20유닛 + 랜덤 오프셋 이동 (적 재스폰 효과)
    
    // collider가 트리거 collider에서 벗어났을때 작동함
    void OnTriggerExit2D(Collider2D collision)
    {
        // "Area" 태그가 아닌 콜라이더와의 이벤트는 무시
        if (!collision.CompareTag("Area"))
            return;
        
        Vector3 playerPos = player.transform.position; // 플레이어의 현재 월드 위치
        Vector3 myPos     = transform.position;        // 이 오브젝트의 현재 월드 위치
        
        // 플레이어와 이 오브젝트 사이의 X, Y 거리 차이 계산
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = player.inputVec; // 플레이어의 현재 이동 입력 방향 벡터
        // 플레이어가 왼쪽으로 이동하면 -1, 오른쪽이면 +1
        float dirX = playerDir.x < 0 ? -1 : 1;
        // 플레이어가 아래쪽으로 이동하면 -1, 위쪽이면 +1
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                // 가로 거리가 더 크면 X축 방향으로 재배치 (좌우 이동 시 타일 순환)
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                // diffY(세로 거리)가 dirY(-1 또는 1)보다 크면 Y축 방향으로 재배치
                // ※ 원래 의도는 diffY > diffX(세로 거리 > 가로 거리) 비교일 수 있음
                else if (diffY > dirY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                // 적의 콜라이더가 활성화되어 있을 때만 재배치 (비활성화된 적은 이동 안 함)
                if (coll.enabled)
                {
                    // 플레이어 이동 방향으로 20유닛 이동 + 겹침 방지를 위한 랜덤 오프셋 추가
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}
