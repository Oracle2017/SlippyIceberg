using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : Level {
	[SerializeField] GameObject blockPrefab;
	[SerializeField] float seperationDist;
	[SerializeField] int amountOfBlocks;
	[SerializeField] float startPosY;
	[SerializeField] float fallAfterSeconds = 0.4f;
	[SerializeField] float scaleXTarget = 1.6f;
	Vector3 currentMoveVelocity;
	GameObject blocksFolder;
	[HideInInspector] public static Vector3 blockSize;
	bool isGrowing;
	int blockNr;
	bool shouldBlockWait;
	int amountOfBlockfoldersSpawned;
	int amountOfBlockfoldersLimit = 10;

	bool shouldWait;
	float moveWaitTimer;

	public override void StartSettings () {
		Reset();
	}

	public override void UpdateSettings () {
		if (shouldWait)
		{
			print("is waiting");
			Wait(1);
			return;
		}

		CenterPlatform();

		if (!isGrowing &&
			GameManager.currentPlatform.transform.position.x >= GameManager.currentPlatform.startPos.x - 0.1f &&
			GameManager.currentPlatform.transform.position.x <= GameManager.currentPlatform.startPos.x + 0.1f) 
		{
			LevelGrow();
			isGrowing = true;
		}

		// Wait til scaling is done
		if (!(GameManager.currentPlatform.transform.localScale.x >= scaleXTarget - 0.1f &&
			GameManager.currentPlatform.transform.localScale.x <= scaleXTarget + 0.1f))
		{
			return;
		}
			
		base.UpdateSettings();

		if (!IsWaiting(GameManager.waitTime) && !shouldBlockWait && amountOfBlockfoldersSpawned < amountOfBlockfoldersLimit)
		{
			StartCoroutine(BlocksFallDownDomino(fallAfterSeconds));
		}
	}

	void CenterPlatform()
	{
		GameManager.currentPlatform.transform.position = Vector3.SmoothDamp(
			GameManager.currentPlatform.transform.position,
			GameManager.currentPlatform.startPos,
			ref currentMoveVelocity, 
			1f);
	}

	void InstantiateBlocks()
	{
		blocksFolder = Instantiate(new GameObject("Blocks Folder"), Vector3.zero, Quaternion.identity);

		for (int i = 0; i < amountOfBlocks; i++)
		{
			Vector3 _pos = new Vector3(i * (blockSize.x + seperationDist), 0);
			print("position block " + i + " = " + _pos);
			Instantiate(blockPrefab, _pos, Quaternion.identity, blocksFolder.transform);
		}
			
		float _totalWidth = (amountOfBlocks - 1) * (blockSize.x + seperationDist);

		blocksFolder.transform.position = new Vector3(- _totalWidth / 2, startPosY);
	}

	IEnumerator BlocksFallDownDomino(float _waitTime)
	{
		/*for (int i = 0; i < blocksFolder.transform.childCount; i++)
		{
			yield return new WaitForSeconds(_waitTime);
			FallingBlock _block = blocksFolder.transform.GetChild(i).GetComponent<FallingBlock>();
			_block.canFall = true;
		}*/
		if (blockNr >= blocksFolder.transform.childCount)
		{
			//InstantiateBlocks();
			//blockNr = 0;

			InstantiateBlocks();
			blockNr = 0;
			shouldBlockWait = false;
			amountOfBlockfoldersSpawned++;
			yield break;
		}

		else 
		{
			shouldBlockWait = true;
			FallingBlock _block = blocksFolder.transform.GetChild(blockNr).GetComponent<FallingBlock>();
			_block.canFall = true;
			yield return new WaitForSeconds(_waitTime);
			blockNr++;
			shouldBlockWait = false;
		}
	}

	void LevelGrow()
	{
		print("Level Grow!");
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(scaleXTarget, 1.5f, false);
	}

	void Wait(float _waitTime)
	{
		if (moveWaitTimer < _waitTime)
		{
			moveWaitTimer+= Time.deltaTime;
			return;
		}

		shouldWait = false;
	}

	public override void Reset()
	{
		base.Reset();

		shouldWait = true;
		moveWaitTimer = 0;

		blockSize = blockPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size;


		isGrowing = false;
		currentMoveVelocity = Vector3.zero;
		//blocksFolder = Instantiate(new GameObject("Blocks Folder"), Vector3.zero, Quaternion.identity);
		blockNr = 0;
		shouldBlockWait = false;
		amountOfBlockfoldersSpawned = 0;

		InstantiateBlocks();
	}
}
