using System;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool    isLeft; // 왼손 여부
    public SpriteRenderer spriter; // 반전시킬 손

    SpriteRenderer player;

    Vector3 rightpos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightposR = new Vector3(-0.15f, -0.15f, 0);
    
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotR = Quaternion.Euler(0, 0, -135);
    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if(isLeft)
        {
            // 근접
            transform.localRotation = isReverse ? leftRotR : leftRot;
            spriter.flipY           = isReverse;
            spriter.sortingOrder = isReverse ? 8 : 12;
        } else
        {
            // 원거리
            transform.localPosition = isReverse ? rightposR : rightpos;
            spriter.flipX           = isReverse;
            spriter.sortingOrder    = isReverse ? 12 : 8;
        }
    }
}
