using UnityEngine;
using UnityEngine.UI;

public class Cosmetic_Previewer : MonoBehaviour {
	[SerializeField] Image cosmeticImage;
	[SerializeField] Text descriptionText;
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
		ChangePreview(GameManager.currentPlayer.spriteRenderer.sprite, "");
	}
}
