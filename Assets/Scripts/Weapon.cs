using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int   id;       // 현재 무기의 id
    public int   prefabId; // 무기 프리팹의 id
    public int   count;    // 최대 무기 수
    public float speed;    // 무기의 속도
    public float damage;   // 무기의 damage

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // 시계방향으로 회전
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelUp(20, 5);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
    }

    void Init()
    {
        switch (id)
        {
            case 0:
                speed = -150; // 시계방향으로 회전
                Batch();
                break;
            default:
                break;
        }
    }

    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet        = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform; // 현재 오브젝트의 자식으로 설정
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count; // 총 count 개의 총알이 균등하게 배치되도록 회전 벡터 계산
            bullet.Rotate(rotVec);
            // 플레이어로부터 1.5f만큼 떨어진 위치에서 회전
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1); // -1로 무한 관통하게 설정
        }
    }

    void OnJump()
    {

    }
}
