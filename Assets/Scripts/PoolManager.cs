using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀링(Object Pooling) 패턴을 구현하는 매니저 스크립트.
/// 자주 생성/삭제되는 오브젝트(예: 적, 투사체)를 미리 생성해두고 재사용함으로써
/// Instantiate/Destroy 호출 비용을 줄여 성능을 향상시킨다.
/// </summary>
public class PoolManager : MonoBehaviour
{
    /// <summary>
    /// 풀링할 프리팹 배열. 인스펙터에서 풀링 대상 프리팹을 등록한다.
    /// 배열 인덱스가 각 풀의 ID가 된다.
    /// </summary>
    public GameObject[] prefabs;

    /// <summary>
    /// 각 프리팹에 대응하는 오브젝트 풀 리스트 배열.
    /// pools[i]는 prefabs[i]의 비활성화된 오브젝트들을 보관한다.
    /// </summary>
    List<GameObject>[] pools;

    /// <summary>
    /// 오브젝트 초기화. 프리팹 수만큼 풀 리스트를 생성하고 초기화한다.
    /// </summary>
    void Awake()
    {
        // 프리팹 배열 크기만큼 풀 배열 할당
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>(); // 각 풀을 빈 리스트로 초기화
        }
    }

    /// <summary>
    /// 지정한 인덱스의 풀에서 비활성화된 오브젝트를 꺼내 반환한다.
    /// 재사용 가능한 오브젝트가 없으면 새로 생성한 뒤 풀에 추가하고 반환한다.
    /// </summary>
    /// <param name="i">꺼내올 풀의 인덱스 (prefabs 배열과 동일한 인덱스)</param>
    /// <returns>활성화할 준비가 된 GameObject</returns>
    public GameObject Get(int i)
    {
        GameObject select = null;
        
        // TODO: pools[i]에서 비활성화된 오브젝트를 찾아 반환하는 로직 구현 예정
        // 예시 로직:
        //   1. pools[i] 중 비활성화된(activeSelf == false) 오브젝트 검색
        //   2. 없으면 prefabs[i]를 Instantiate해서 pools[i]에 추가
        //   3. 찾은/생성한 오브젝트를 활성화(SetActive(true))하고 반환
        
        return select;
    }
}
