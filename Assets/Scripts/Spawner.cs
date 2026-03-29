using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
	public Transform[] spawnPoint;
	public SpawnData[] spawnData;
	float              timer; // 적 소환 쿨타임
	int                level; // 게임의 레벨 (난이도)

	void Awake()
	{
		spawnPoint = GetComponentsInChildren<Transform>();
	}

	void Update()
	{
		timer += Time.deltaTime;
		level =  Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

		if (timer > spawnData[level].spawnTime)
		{
			timer = 0f;
			Spawn();
		}
	}

	void Spawn()
	{
		GameObject enemy = GameManager.instance.pool.Get(0);
		// spawnPoint의 0번의 위치는 자기 자신의 위치이므로 1부터 지정하여 넣어줌
		enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
		enemy.GetComponent<Enemy>().Init(spawnData[level]);
	}
}

[Serializable]
public class SpawnData
{
	public float spawnTime;
	public int   spriteType;
	public int   maxHP;
	public float speed;
}