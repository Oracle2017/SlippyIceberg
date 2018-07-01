using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {
	
	float startJumpY;
	float endJumpY;
	bool isJumping;
	float velocityY;
	float previousPosY;

	[Header("JUMP SETTINGS")]
	[Range(0.0f, 1.0f)]
	[SerializeField] float jumpSpeed;
	[SerializeField] int jumpHeight;
	[SerializeField] public float landingSpeed = 1;
	[SerializeField] bool canFall;


	// Use this for initialization
	void Start () {
		/*startJumpY = transform.position.y - jumpHeight;
		endJumpY = transform.position.y;*/
	}

	public void UpdateSettings(bool _isTouchingGround)
	{
		if (!isJumping)
		{
			if (Input.GetKeyDown(KeyCode.Space) && _isTouchingGround)
			{
				Jump();
			}

			else if (!_isTouchingGround && canFall)
			{
				transform.position -= new Vector3(0, landingSpeed * Time.deltaTime, 0);
			}
		}


		else 
		{
			/*velocityY = transform.position.y - previousPosY;
			previousPosY = transform.position.y;*/

			float newPosY = Mathf.Lerp(transform.position.y, endJumpY, jumpSpeed);
			transform.position = new Vector2(transform.position.x, newPosY);

			if (transform.position.y >= (endJumpY - 0.5f))
			{
				isJumping = false;
			}
		}
	}

	void Jump()
	{
		startJumpY = transform.position.y;
		endJumpY = transform.position.y + jumpHeight;
		isJumping = true;
	}

	public void Reset()
	{
		endJumpY = 0f;
		isJumping = false;
	}


}
