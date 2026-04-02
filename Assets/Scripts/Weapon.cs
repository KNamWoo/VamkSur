using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int   id;       // 현재 무기의 id
    public int   prefabId; // 무기 프리팹의 id
    public int   count;    // 최대 무기 수
    public float speed;    // 무기의 속도
    public float damage;   // 무기의 damage

    Player player;
    float  timer;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if(!GameManager.instance.isGameLive)
            return;
        
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // 시계방향으로 회전
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage =  damage * Character.Damage;
        this.count  += count;

        if (id == 0)
        {
            Batch();
        }
        
        // 새 무기가 장비의 효과를 받을 수 있도록
        // player의 모든 gear에 ApplyGear를 실행하게 전달
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic set
        name             = "Weapon " + data.itemID;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        
        // Property Set
        id     = data.itemID;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }
        
        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed; // 시계방향으로 회전
                Batch();
                break;
            default:
                speed = 0.5f * Character.WeaponRate; // 발사 간격 (초)
                break;
        }
        
        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        
        // 새 무기가 장비의 효과를 받을 수 있도록
        // player의 모든 gear에 ApplyGear를 실행하게 전달
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1로 무한 관통하게 설정
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
        {
            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // count를 관통 수치로 사용
        
    }
}
