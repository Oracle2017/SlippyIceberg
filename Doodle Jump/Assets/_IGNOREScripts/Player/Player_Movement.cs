using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour {
	Rigidbody2D rb;

	[Header("MOVE SETTINGS")]
	// not using this atm
	[SerializeField] int moveSpeed;
	[Space(10)]

	[Header("OTHER")]
	[SerializeField] bool phoneInput;



	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
	}

	void Movement()
	{
		/*if (Input.GetButton("Horizontal"))
		{
			float value = Input.GetAxis ("Horizontal"); 
			float posX = value * speed * Time.deltaTime;
			rb.velocity = new Vector2(posX, rb.velocity.y);

			print(rb.velocity );
		}*/
		if (phoneInput)
		{
			rb.velocity = new Vector2(Input.acceleration.x * 2 * moveSpeed, rb.velocity.y);
			return;
		}


		if (Input.GetKey(KeyCode.LeftArrow))
		{
			//float velX =  speed * Time.deltaTime;
			rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
		}

		else if (Input.GetKey(KeyCode.RightArrow))
		{
			//float velX =  speed * Time.deltaTime;
			rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
		}

		else {
			rb.velocity = new Vector2(0, rb.velocity.y);
		}
			

	}

}
