using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerClassBase<PlayerManager>
{
	// GameUI 프리펩 컴포넌트
	[SerializeField] private GameUIInstance _GameUIPrefab;

	// 플레이어 캐릭터 객체를 나타냅니다.
	private PlayerCharacter _PlayerCharacter;

	// 플레이어 캐릭터에 대한 읽기 전용 프로퍼티
	public PlayerCharacter playerCharacter => _PlayerCharacter = _PlayerCharacter ?? 
		GameObject.Find("PlayerCharacter").GetComponent<PlayerCharacter>();

	// GameUI 인스턴스를 나타냅니다
	public GameUIInstance gameUI => _GameUIPrefab;

	public override void InitializeManagerClass()
	{
	}

	public override void OnSceneChanged(string newSceneName)
	{		
	}
}
