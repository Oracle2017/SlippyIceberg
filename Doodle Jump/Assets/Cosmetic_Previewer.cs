using UnityEngine;
using UnityEngine.UI;

public class Cosmetic_Previewer : MonoBehaviour {
	[SerializeField] Image cosmeticImage;
	[SerializeField] Text descriptionText;
	[SerializeField] public Text amountOfCoins;
	public static Cosmetic_Previewer singleton;

	void Start()
	{
		if (singleton == null)
			singleton = this;
	}


	// Use this for initialization
	public void ChangePreview (Sprite _newSprite, string _description) {
		cosmeticImage.sprite = _newSprite;
		descriptionText.text = _description;

	}

	public void Reset()
	{
		string _description = PlayerPrefs.GetString("currentCosmeticDescription");
		ChangePreview(GameManager.currentPlayer.spriteRenderer.sprite, _description);
		amountOfCoins.text = Score.totalAmountOfCoins.ToString();
	}
}
