using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitor : MonoBehaviour {
	[SerializeField] GameObject diamonds2X;
	[SerializeField] float messageTime;
	[SerializeField] GameObject glitterParticleEffect;
	[SerializeField] GameObject birthdayGlitterParticleEffect;
	GameObject currentGameObject;
	bool isBlinking;
	bool canBlink;

	public void StartSettings()
	{
		Reset();
	}

	public void StageCompleted()
	{
		GameObject _startusParticle = Instantiate(glitterParticleEffect, glitterParticleEffect.transform.position, Quaternion.identity);
		Destroy(_startusParticle, 5f);
		GameObject _birthdayParticle = Instantiate(birthdayGlitterParticleEffect, birthdayGlitterParticleEffect.transform.position, Quaternion.identity);
		Destroy(_birthdayParticle, 5f);
	}

	// Update is called once per frame
	void Update () {
		if (!isBlinking && canBlink)
		{
			StartCoroutine(Blinker(0.5f, currentGameObject));
		}
	}

	IEnumerator Blinker(float _waitTime, GameObject _gameObject)
	{
		canBlink = true;
		isBlinking = true;
		yield return new WaitForSeconds(_waitTime);
		_gameObject.SetActive(!_gameObject.activeSelf);
		isBlinking = false;
		yield return new WaitForSeconds(messageTime);
		_gameObject.SetActive(false);
		canBlink = false;
	}

	public void Diamonds2X()
	{
		currentGameObject = diamonds2X;
		canBlink = true;
	}

	public void Reset()
	{
		if (currentGameObject != null)
		{
			currentGameObject.SetActive(false);
		}
		canBlink = false;
	}
}
