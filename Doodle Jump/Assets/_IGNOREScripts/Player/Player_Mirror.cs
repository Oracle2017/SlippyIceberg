using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mirror : MonoBehaviour {
	Bounds cameraBounds;
	float spriteWidth;
	GameObject playerReflection;
	SpriteRenderer playerSprite;
	[HideInInspector] public SpriteRenderer reflectionSprite;
	Animator anim;

	// Use this for initialization
	void Start () {
		InstantiatePlayerReflection();

		cameraBounds = CameraExtensions.OrthographicBounds(Camera.main);
		playerSprite = GetComponent<SpriteRenderer>();
		spriteWidth = playerSprite.sprite.bounds.size.x;
	}

	// Update is called once per frame
	void Update () {
		Mirror();
		Reflection();
	}

	void Mirror()
	{
		
		if (transform.position.x > cameraBounds.extents.x)
		{
			float additionalDist = transform.position.x - cameraBounds.extents.x;
			transform.position = new Vector2(-cameraBounds.extents.x + additionalDist, transform.position.y);
		}

		else if (transform.position.x < -cameraBounds.extents.x)
		{
			float subDist = -cameraBounds.extents.x - transform.position.x; 
			transform.position = new Vector2(cameraBounds.extents.x - subDist, transform.position.y);
		}
	}

	void InstantiatePlayerReflection()
	{
		playerReflection = new GameObject(gameObject.name + " Reflection");
		reflectionSprite = playerReflection.AddComponent<SpriteRenderer>();
		reflectionSprite.sprite = GetComponent<SpriteRenderer>().sprite;
		if (GetComponent<Animator>() != null)
		{
			anim = playerReflection.AddComponent<Animator>();
			anim.runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
		}

		/*
		// Add collider to the reflection
		EdgeCollider2D edgeCollider2D = playerReflection.AddComponent<EdgeCollider2D>();
		//edgeCollider2D.size = GetComponent<EdgeCollider2D>().size;
		edgeCollider2D.offset = GetComponent<EdgeCollider2D>().offset;
		playerReflection.tag = gameObject.tag;
		playerReflection.layer = gameObject.layer;

		// pretty useless i think
		playerReflection.transform.localScale = transform.localScale;
		playerReflection.transform.rotation = transform.rotation;
		playerReflection.transform.position = new Vector2(0, 0);*/
	}

	void Reflection()
	{
		playerReflection.transform.rotation = transform.rotation;
		reflectionSprite.flipX = playerSprite.flipX;
		playerReflection.transform.localScale = transform.lossyScale;
		//playerReflection.transform.rotation = transform.rotation;

		if ((transform.position.x + spriteWidth * transform.localScale.x / 2) >= cameraBounds.extents.x)
		{
			playerReflection.SetActive(true);
			float posX = -cameraBounds.extents.x  + (transform.position.x  - cameraBounds.extents.x);
			playerReflection.transform.position = new Vector2(posX, transform.position.y);
		}

		else if ((transform.position.x - spriteWidth * transform.localScale.x / 2) <= -cameraBounds.extents.x)
		{
			playerReflection.SetActive(true);
			float posX = transform.position.x  + cameraBounds.extents.x * 2;
			playerReflection.transform.position = new Vector2(posX, transform.position.y);
		}

		else {
			playerReflection.SetActive(false);
		}
	}
}
