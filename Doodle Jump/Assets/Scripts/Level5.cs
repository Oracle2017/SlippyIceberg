using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5 : Level3 {
	bool isWaiting;

	public override void StartSettings () {
		blocksFolders = new List<GameObject>();
		Reset();
	}

	public override void UpdateSettings () {
		base.UpdateSettings();

		print("isPrepared = " + isPrepared);

		if (!isPrepared)
		{
			return;
		}


		if (!isWaiting)
		{
			StartCoroutine(BlocksFallDownGap(5));
		}
	}

	IEnumerator BlocksFallDownGap(float _waitTime)
	{
		isWaiting = true;

		InstantiateBlocks();

		GameObject _blocksFolder = blocksFolders[blocksFolders.Count-1];
		int r = Random.Range(0, _blocksFolder.transform.childCount);

		for (int i = 0; i < _blocksFolder.transform.childCount; i++)
		{
			_blocksFolder.transform.GetChild(i).GetComponent<FallingBlock>().canFall = true;

		}

		Destroy(_blocksFolder.transform.GetChild(r).gameObject);

		yield return new WaitForSeconds(_waitTime);

		isWaiting = false;
	}

	public override void Reset()
	{
		base.Reset();
		canDomino = false;
		isWaiting = false;
	}
}
