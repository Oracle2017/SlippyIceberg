using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : Level {
	[SerializeField] GameObject blockPrefab;
	[SerializeField] float seperationDist;
	[SerializeField] int amountOfBlocks;
	[SerializeField] float startPosY;
	[SerializeField] float fallAfterSeconds = 0.4f;

	List<GameObject> blocksFolders;
	[HideInInspector] public static Vector3 blockSize;
	int blockNr;
	bool shouldBlockWait;
	//int amountOfBlockfoldersSpawned;
	//int amountOfBlockfoldersLimit = 10;

	bool shouldWait;
	float moveWaitTimer;

	public override void StartSettings () {
		blocksFolders = new List<GameObject>();
		Reset();
	}

	public override void UpdateSettings () {
		base.UpdateSettings();

		if (!isPrepared)
		{
			return;
		}

		/*if (shouldWait)
		{
			print("is waiting");
			Wait(1);
			return;
		}*/
			
		if (!shouldBlockWait)
		{
			StartCoroutine(BlocksFallDownDomino(fallAfterSeconds));
		}
	}

	void InstantiateBlocks()
	{
		GameObject _blocksFolder = Instantiate(new GameObject("Blocks Folder"), Vector3.zero, Quaternion.identity);
		blocksFolders.Add(_blocksFolder);

		for (int i = 0; i < amountOfBlocks; i++)
		{
			Vector3 _pos = new Vector3(i * (blockSize.x + seperationDist), 0);
			print("position block " + i + " = " + _pos);
			Instantiate(blockPrefab, _pos, Quaternion.identity, _blocksFolder.transform);
		}
			
		float _totalWidth = (amountOfBlocks - 1) * (blockSize.x + seperationDist);

		_blocksFolder.transform.position = new Vector3(- _totalWidth / 2, startPosY);
	}

	IEnumerator BlocksFallDownDomino(float _waitTime)
	{
		/*for (int i = 0; i < blocksFolder.transform.childCount; i++)
		{
			yield return new WaitForSeconds(_waitTime);
			FallingBlock _block = blocksFolder.transform.GetChild(i).GetComponent<FallingBlock>();
			_block.canFall = true;
		}*/
		GameObject _blocksFolder = blocksFolders[blocksFolders.Count-1];
		if (blockNr >= _blocksFolder.transform.childCount)
		{
			//InstantiateBlocks();
			//blockNr = 0;

			InstantiateBlocks();
			blockNr = 0;
			shouldBlockWait = false;
			//amountOfBlockfoldersSpawned++;
			yield break;
		}

		else 
		{
			shouldBlockWait = true;
			FallingBlock _block = _blocksFolder.transform.GetChild(blockNr).GetComponent<FallingBlock>();
			_block.canFall = true;
			yield return new WaitForSeconds(_waitTime);
			blockNr++;
			shouldBlockWait = false;
		}
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

		for (int i = 0; i < blocksFolders.Count; i++)
		{
			Destroy(blocksFolders[i]);
		}
		blocksFolders.Clear();

		shouldStabilize = true;
		shouldCenterPosition = true;
		shouldScale = true;


		shouldWait = true;
		moveWaitTimer = 0;

		blockSize = blockPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size;
		blockNr = 0;
		shouldBlockWait = false;
		//amountOfBlockfoldersSpawned = 0;

		InstantiateBlocks();
	}
}
