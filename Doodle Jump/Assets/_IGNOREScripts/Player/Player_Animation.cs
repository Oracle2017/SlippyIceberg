using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour {
	Rigidbody2D rb;

	// Scalling animation
	Vector3 initialLocalScale;
	[SerializeField] float minScale = 1;
	[SerializeField] float maxScale = 3;
	Sprite sprite;
	Vector3 spriteScale; // in Unity units


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>().sprite;
		spriteScale = new Vector3(1, sprite.bounds.size.y, 1);
		initialLocalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		ScalingAnimation();
	}

	void ScalingAnimation()
	{
		if (Player_Jump.isJumping)
		{
			float scaleYMultiplier =  Utils.Map(rb.position.y, Player_Jump.startJumpY, Player_Jump.endJumpY, minScale, maxScale);
			scaleYMultiplier = Mathf.Clamp(scaleYMultiplier, minScale, maxScale);
			Vector3 _previousLocalScale = transform.localScale;
			transform.localScale = new Vector3(initialLocalScale.x, initialLocalScale.y * scaleYMultiplier, initialLocalScale.z);

			rb.position -= Vector2.Scale(Utils.AbsY((transform.localScale - _previousLocalScale) / 2), spriteScale);
		}

		else 
		{
			//float scaleYMultiplier =  maxScale - (Utils.Map(rb.position.y, Player_Jump.startJumpY, Player_Jump.endJumpY, minScale, maxScale) - minScale);
			float scaleYMultiplier =  Utils.Map(rb.position.y, Player_Jump.startJumpY, Player_Jump.endJumpY, minScale, maxScale);
			scaleYMultiplier = Mathf.Clamp(scaleYMultiplier, minScale, maxScale);
			Vector3 _previousLocalScale = transform.localScale;
			transform.localScale = new Vector3(initialLocalScale.x, initialLocalScale.y * scaleYMultiplier, initialLocalScale.z);

			rb.position += Vector2.Scale(Utils.AbsY((transform.localScale - _previousLocalScale) / 2), spriteScale);
		}
	}
}
