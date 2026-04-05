/*
	최종 변경일: 2026.03.29
	수정자
	- 김남우
	-

	목적
	- 게임 전체를 관리하는 매니저 스크립트
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	// 씬 전체에서 유일하게 존재하는 GameManager 인스턴스.
	// 다른 스크립트에서 GameManager.instance.player 처럼 사용한다.
	public static GameManager instance;

	[Header("# Game Object")]
	// 플레이어 오브젝트의 Player 스크립트 참조.
	// 다른 매니저나 시스템에서 플레이어 정보(위치, 입력 벡터 등)에 접근할 때 사용한다.
	public Player player;
	public PoolManager pool;
	public LevelUp     uiLevelUp;
	public Result      uiResult;
	public GameObject  enemyCleaner;
	
	[Header("# Player Info")] 
	public int playerID;
	public float   HP;
	public float   maxHP;
	public int   level;
	public int   kill;
	public int   exp;
	public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

	[Header("# Game Control")]
	public bool isGameLive; // 게임이 진행 중인지 여부. 게임 오버나 일시정지 상태에서는 false로 설정한다.
	// 현재 게임의 플레이 시간 변수
	public float gameTime;
	public float maxGameTime = 2 * 10f;

	// 오브젝트 초기화. 현재 인스턴스를 싱글턴으로 등록한다.

	void Awake()
	{
		// 싱글턴 패턴: 이 스크립트가 부착된 오브젝트를 전역 인스턴스로 설정
		instance = this;
		// DontDestroyOnLoad(instance); // 씬 전환 시에도 파괴되지 않도록 하려면 주석 해제
	}

	public void GameStart(int id)
	{
		playerID = id;
		HP      = maxHP;
		
		player.gameObject.SetActive(true);
		uiLevelUp.Select(playerID %2);
		Resume();
		
		AudioManager.instance.PlayBgm(true);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
	}

	public void GameOver()
	{
		StartCoroutine(GameOverRoutine());
		isGameLive = false;
	}

	IEnumerator GameOverRoutine()
	{
		isGameLive = false;
		
		yield return new WaitForSeconds(0.5f);
		
		uiResult.gameObject.SetActive(true);
		uiResult.Lose();
		Stop();
		
		AudioManager.instance.PlayBgm(false);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
	}
	
	public void GameVictory()
	{
		StartCoroutine(GameVictoryRoutine());
		isGameLive = false;
	}

	IEnumerator GameVictoryRoutine()
	{
		isGameLive = false;
		enemyCleaner.SetActive(true);
		
		yield return new WaitForSeconds(0.5f);
		
		uiResult.gameObject.SetActive(true);
		uiResult.Win();
		Stop();
		
		AudioManager.instance.PlayBgm(false);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
	}

	public void GameRetry()
	{
		SceneManager.LoadScene("SampleScene");
	}

	void Update()
	{
		if(!isGameLive)
			return;
		
		gameTime += Time.deltaTime;
		
		if (gameTime > maxGameTime)
		{
			gameTime = maxGameTime;
			GameVictory();
		}
	}

	public void GetExp()
	{
		if(!isGameLive)
			return;
		
		exp++;
		if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
		{
			level++;
			exp = 0;
			uiLevelUp.Show();
		}
	}

	public void Stop()
	{
		isGameLive = false;
		Time.timeScale = 0f;
	}

	public void Resume()
	{
		isGameLive = true;
		Time.timeScale = 1f;
	}
}