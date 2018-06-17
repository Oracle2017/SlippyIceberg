using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] Player player; // Just used as prefab
	public static Player currentPlayer;
	[SerializeField] Score score;
	[SerializeField] GameOver gameOver;

	// Use this for initialization
	void Start () {
		// TODO: remove this in the end, just for debugging
		currentPlayer = GameObject.FindObjectOfType<Player>() as Player;

		if (currentPlayer == null)
		{
			Instantiate(player.gameObject, player.transform.position, player.transform.rotation);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (!currentPlayer.isDead)
		{
			currentPlayer.UpdatePlayer();
			score.UpdateScore();
		}

		else 
		{
			gameOver.DisplayWindow();
		}
	}

	public void RestartGame()
	{
		currentPlayer.Reset();
		gameOver.CloseWindow();
		score.Reset();
	}
}
