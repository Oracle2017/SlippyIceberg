using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : Level {
	[SerializeField] GameObject blockPrefab;
	[SerializeField] float seperationDist;
	[SerializeField] int amountOfBlocks;
	[SerializeField] float startPosY;
	[SerializeField] float fallAfterSeconds = 0.4f;

	protected List<GameObject> blocksFolders;
	[HideInInspector] public static Vector3 blockSize;
	protected int blockNr;
	bool shouldBlockWait;
	//int amountOfBlockfoldersSpawned;
	//int amountOfBlockfoldersLimit = 10;

	bool shouldWait;
	protected bool canDomino;
	float moveWaitTimer;
	float waitTimer;
	bool firstBlocksInstantiated;

	public override void StartSettings () {
		blocksFolders = new List<GameObject>();
		Reset();
	}

	public override void UpdateSettings () {
		if (canDomino)
		{
			if (ShouldWait(3f))
			{
				return;
			}

			if (!firstBlocksInstantiated)
			{
				InstantiateBlocks();
				firstBlocksInstantiated = true;
			}
		}

		base.UpdateSettings();

		if (!isPrepared)
		{
			return;
		}
			
		if (!shouldBlockWait && canDomino)
		{
			StartCoroutine(BlocksFallDownDomino(fallAfterSeconds));
		}
	}

	bool ShouldWait(float _waitTime)
	{
		if (waitTimer < _waitTime)
		{
			waitTimer += Time.deltaTime;
			return true;
		}

		return false;
	}

	protected void InstantiateBlocks()
	{
		GameObject _blocksFolder = Instantiate(new GameObject("Blocks Folder"), Vector3.zero, Quaternion.identity);
		blocksFolders.Add(_blocksFolder);

		for (int i = 0; i < amountOfBlocks; i++)
		{
			Vector3 _pos = new Vector3(i * (blockSize.x + seperationDist), 0);
			//print("position block " + i + " = " + _pos);
			GameObject _blockGameObject = Instantiate(blockPrefab, _pos, Quaternion.identity, _blocksFolder.transform);
			_blockGameObject.GetComponent<FallingBlock>().canFall = false;
		}
			
		float _totalWidth = (amountOfBlocks - 1) * (blockSize.x + seperationDist);

		_blocksFolder.transform.position = new Vector3(- _totalWidth / 2, startPosY);
	}

	IEnumerator BlocksFallDownDomino(float _waitTime)
	{
		if (blocksFolders.Count <= 0)
		{
			yield break;
		}

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

		canDomino = true;
		waitTimer = 0f;
		firstBlocksInstantiated = false;
		//amountOfBlockfoldersSpawned = 0;
	}
}
