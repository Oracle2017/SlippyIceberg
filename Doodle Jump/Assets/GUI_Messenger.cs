using UnityEngine;
using UnityEngine.UI;

public class GUI_Messenger : MonoBehaviour {
	[SerializeField] public Image cosmeticImage;
	[SerializeField] public Text cosmeticDescription;
	[SerializeField] public Text amountOfDiamondsText;
	[HideInInspector] public int siblingIndex;
	[SerializeField] GameObject darkBackground;
	string lockedCosmeticName;
	RectTransform rectTransform;
	Vector3 targetScale;
	Vector3 scalingVelocity;

	// Use this for initialization
	public void Start () {
		rectTransform = GetComponent<RectTransform>();
		targetScale = rectTransform.localScale;
		darkBackground.SetActive(false);
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	public void Update () {
		if (!gameObject.activeSelf)
		{
			return;
		}

		rectTransform.localScale = Vector3.SmoothDamp(rectTransform.localScale, targetScale, ref scalingVelocity, 0.2f);
	}

	public void OpenWindow(int _siblingIndex, Cosmetic _lockedCosmetic)
	{
		Reset();
		lockedCosmeticName = _lockedCosmetic.name;
		siblingIndex = _siblingIndex;
		cosmeticImage.sprite = _lockedCosmetic.unlockedSprite;
		cosmeticDescription.text = _lockedCosmetic.description;
		amountOfDiamondsText.text = _lockedCosmetic.amountOfDiamonsNeeded.ToString();
		darkBackground.SetActive(true);
		gameObject.SetActive(true);
	}

	public void CloseWindow()
	{
		GameManager.isPausing = false;
		gameObject.SetActive(false);
		darkBackground.SetActive(false);
	}
		

	public void UnlockCosmetic()
	{
		PlayerPrefs.SetInt("unlockedCosmetics_"+lockedCosmeticName, 1);

		Score.totalAmountOfCoins -= int.Parse(amountOfDiamondsText.text);
		PlayerPrefs.SetInt("amountOfDiamonds", Score.totalAmountOfCoins);
		//Cosmetic_Previewer.singleton.amountOfCoins.text = Score.totalAmountOfCoins.ToString();

		PlayerPrefs.SetInt("currentCosmeticChildNr", siblingIndex);
		PlayerPrefs.SetString("currentCosmeticDescription", cosmeticDescription.text);

		GameManager.currentPlayer.spriteRenderer.sprite = cosmeticImage.sprite;

		Player_Mirror playerMirror = GameManager.currentPlayer.GetComponent<Player_Mirror>();

		if (playerMirror != null)
		{
			playerMirror.reflectionSprite.sprite = cosmeticImage.sprite;
		}

		CloseWindow();
	}

	public void Reset()
	{
		GameManager.isPausing = true;
		rectTransform.localScale = Vector3.zero;
	}
}
