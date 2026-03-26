using UnityEngine;

/// <summary>
/// 게임 전체를 관리하는 싱글턴(Singleton) 매니저 스크립트.
/// 씬 어디서든 GameManager.instance 를 통해 플레이어 정보 등 공용 데이터에 접근할 수 있다.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 씬 전체에서 유일하게 존재하는 GameManager 인스턴스.
    /// 다른 스크립트에서 GameManager.instance.player 처럼 사용한다.
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// 플레이어 오브젝트의 Player 스크립트 참조.
    /// 다른 매니저나 시스템에서 플레이어 정보(위치, 입력 벡터 등)에 접근할 때 사용한다.
    /// </summary>
    public Player player;

    /// <summary>
    /// 오브젝트 초기화. 현재 인스턴스를 싱글턴으로 등록한다.
    /// </summary>
    void Awake()
    {
        // 싱글턴 패턴: 이 스크립트가 부착된 오브젝트를 전역 인스턴스로 설정
        instance = this;
        // DontDestroyOnLoad(instance); // 씬 전환 시에도 파괴되지 않도록 하려면 주석 해제
    }
}
