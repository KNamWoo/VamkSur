using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; // 프리펩 변수
    List<GameObject>[]  pools; // 리스트

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>(); // pool의 리스트 초기화
        }
    }

    public GameObject Get(int i)
    {
        GameObject select = null;
        
        
        return select;
    }
}
