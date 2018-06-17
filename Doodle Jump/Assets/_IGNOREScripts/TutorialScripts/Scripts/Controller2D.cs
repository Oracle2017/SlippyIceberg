using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public LayerMask collisionMask;
	public LayerMask playerMask;

	const float skinWidth = .015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float maxClimbAngle = 80;
	float maxDescendAngle = 75;

	float horizontalRaySpacing;
	float verticalRaySpacing;

	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;

	public float minSlipVelocity = 2;
	public float maxSLipVelocity = 4;

	Vector3 tempVelocity;



	void Start() {
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	void Update()
	{
		//ReadjustPlayer();
	}

	public void Move(Vector3 velocity) {
		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.velocityOld = velocity;


		if (velocity.y < 0)
		{
			DescendSlope(ref velocity);
		}

		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		tempVelocity = velocity;


		transform.Translate (velocity);
	}

	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);

			if (hit) {

				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (collisions.descendingSlope)
					{
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance-skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (collisions.climbingSlope) {
						velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}

					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);

			if (hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisions.climbingSlope) {
					velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}

		if (collisions.climbingSlope)
		{
			float directionX = Mathf.Sign(velocity.x);
			rayLength = Mathf.Abs(velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1)? raycastOrigins.bottomLeft: raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != collisions.slopeAngle)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}

		}

	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		Vector2 rayOrigin = (directionX == -1)? raycastOrigins.bottomRight: raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

		if (hit)
		{
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
			{
				if (Mathf.Sign(hit.normal.x) == directionX)
				{
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
					{
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;

						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}

	void UpdateRaycastOrigins() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
		
	public float SlipVelocityX()
	{
		int amountOfFlips = (int) Mathf.Abs(collisions.slopeAngle / 90);


		float slipVelocity = Utils.Map(Mathf.Abs(collisions.slopeAngle % 90), 0, 89, minSlipVelocity, maxSLipVelocity);
		//float slipVelocity = (maxSLipVelocity - minSlipVelocity) * (Mathf.Abs(collisions.slopeAngle) % 90 - 0) / (89 - 0) + minSlipVelocity;
		float velocityX;

		if (amountOfFlips % 2 == 0)
		{
			velocityX = -slipVelocity;
		}

		else 
		{
			velocityX = slipVelocity;
		}

		velocityX *= Mathf.Sign(collisions.slopeAngle);

		//print("collisions.slopeAngle = " + collisions.slopeAngle);
		//print("amountOfFlips = " + amountOfFlips);
		//print("velocityX = " + velocityX);


		return velocityX;
	}

	public void ReadjustPlayer()
	{
		/*float direction = (PerlinNoise.platformSingleton.rotationZChange < 0)? 1: -1;
		Vector2 rayOrigin = (PerlinNoise.platformSingleton.transform.localRotation.z < 0)? raycastOrigins.bottomLeft: raycastOrigins.bottomRight;
		// direction is not known yet
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin + (Vector2) tempVelocity,  (Vector2)PerlinNoise.platformSingleton.transform.up.normalized * direction, Mathf.Infinity, collisionMask);
		RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin + (Vector2) tempVelocity,  (Vector2)PerlinNoise.platformSingleton.transform.up.normalized * -direction, Mathf.Infinity, collisionMask);
		print("hit distance = " + hit.distance);
		Debug.DrawRay(rayOrigin + (Vector2) tempVelocity, (Vector2) PerlinNoise.platformSingleton.transform.up.normalized * direction * 20, Color.red);
		Debug.DrawRay(rayOrigin + (Vector2) tempVelocity, (Vector2) PerlinNoise.platformSingleton.transform.up.normalized * -direction * 20, Color.yellow);


		if (hit.collider != null)
		{
			float rayOriginDirection = (PerlinNoise.platformSingleton.transform.localRotation.z < 0)? 1: -1;
			transform.position = hit.point + new Vector2(collider.bounds.size.x / 2 * rayOriginDirection + skinWidth * -rayOriginDirection, collider.bounds.size.y / 2);
			print("raycast 1 (red) hit somthing");
		}

		else if (hit2.collider != null)
		{
			float rayOriginDirection = (PerlinNoise.platformSingleton.transform.localRotation.z < 0)? 1: -1;
			transform.position = hit.point + new Vector2(collider.bounds.size.x / 2 * rayOriginDirection, collider.bounds.size.y / 2);
			//transform.position = hit2.point;
			print("raycast 2 (yellow) hit somthing");
		}*/

		transform.RotateAround(PerlinNoise.platformSingleton.transform.position, new Vector3(0, 0, 1), PerlinNoise.platformSingleton.rotationZChange);
		transform.localRotation = Quaternion.identity;
		print("PerlinNoise.platformSingleton.transform.position = " + PerlinNoise.platformSingleton.transform.position);
		print("PerlinNoise.platformSingleton.rotationZChange = " + PerlinNoise.platformSingleton.rotationZChange.ToString());
	}

	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public float slopeAngle, slopeAngleOld;
		public bool descendingSlope;
		public Vector3 velocityOld;

		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

}