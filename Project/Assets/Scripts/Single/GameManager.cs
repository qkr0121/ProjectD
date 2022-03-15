using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : GameManagerBase
{
	protected override void InitializeManagerClasses()
	{
		RegisterManagerClass<PlayerManager>();
		RegisterManagerClass<InputManager>();
		RegisterManagerClass<ResourceManager>();
	}
}
