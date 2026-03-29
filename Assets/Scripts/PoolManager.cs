/*
    최종 변경일: 2026.03.29
    수정자
    - 김남우
    -

    목적
    - Object Pooling을 구현하기 위한 매니저 스크립트로 자주 생성/삭제되는 오브젝트를
      미리 생성해두고 재사용하여 생성/삭제 호출 비용을 줄여 성능을 향상시킨다
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 풀링할 프리팹 배열. 인스펙터에서 풀링 대상 프리팹을 등록한다.
    // 배열 인덱스가 각 풀의 ID가 된다.
    public GameObject[] prefabs;

    
    // 각 프리팹에 대응하는 오브젝트 풀 리스트 배열.
    // pools[i]는 prefabs[i]의 비활성화된 오브젝트들을 보관한다.
    List<GameObject>[] pools;

    // 오브젝트 초기화. 프리팹 수만큼 풀 리스트를 생성하고 초기화한다.
    void Awake()
    {
        // 프리팹 배열 크기만큼 풀 배열 할당
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>(); // 각 풀을 빈 리스트로 초기화
        }
    }

    // 지정한 인덱스의 풀에서 비활성화된 오브젝트를 꺼내 반환한다.
    // 재사용 가능한 오브젝트가 없으면 새로 생성한 뒤 풀에 추가하고 반환한다.
    public GameObject Get(int i)
    {
        GameObject select = null;

        foreach (GameObject item in pools[i])
        {
            // 선택된 풀의 비활성화된 오브젝트 접근
            if (!item.activeSelf)
            {
                // 비활성 오브젝트 발견 시 select에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        
        // 비활성 오브젝트를 찾을 수 없음
        if (!select)
        {
            // 새로 생성하여 select에 할당
            // 프리팹을 새로 생성하여 해당하는 object의 위치에 자식으로 생성한다.
            select = Instantiate(prefabs[i], transform);
            pools[i].Add(select);
        }
        
        return select;
    }
}
