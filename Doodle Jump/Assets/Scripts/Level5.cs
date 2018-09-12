using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5 : Level3 {
	bool isWaiting = true;

	public override void UpdateSettings () {
		base.UpdateSettings();

		if (!isPrepared)
		{
			return;
		}

		if (!isWaiting)
		{
			StartCoroutine(BlocksFallDownGap(2));
		}
	}

	IEnumerator BlocksFallDownGap(float _waitTime)
	{
		isWaiting = true;

		InstantiateBlocks();
		int r = Random.Range(0, blocksFolders.Count);

		GameObject _blocksFolder = blocksFolders[blocksFolders.Count-1];

		for (int i = 0; i < _blocksFolder.transform.childCount; i++)
		{
			FallingBlock _block = _blocksFolder.transform.GetChild(i).GetComponent<FallingBlock>();
			_block.canFall = true;
		}

		Destroy(_blocksFolder.transform.GetChild(r));

		yield return new WaitForSeconds(_waitTime);

		isWaiting = false;
	}

	public override void Reset()
	{
		base.Reset();
	}
}
