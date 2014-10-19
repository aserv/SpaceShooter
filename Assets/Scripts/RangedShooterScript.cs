using UnityEngine;
using System.Collections;

public class RangedShooterScript : EnemyBase {
	public int damage;
	public GameObject projectile;
	public float maxRange;
	public float minRange;
	private GameObject player;
	private Vector2 direction;
	private Transform projectileSpawn;
	private Animator animator;
	private int animFireHash;
	private bool firing;
	private float distance;

	// Use this for initialization
	void Start () {
		SetUp ();
		player = GameObject.FindGameObjectWithTag ("Player");
		projectileSpawn = gameObject.transform.GetChild(0);
		animator = this.GetComponent<Animator> ();
		animFireHash = Animator.StringToHash ("Fire");
	}
	
	void Update () {
		if (player != null) {
			direction = (Vector2)(player.transform.position - transform.position);
		}
		distance = direction.magnitude;
		targetAngle = Vector2.Angle (direction, Vector2.right);
		if (distance > maxRange) {
			targetVelocity = direction.normalized * speed;
		} else if (distance < minRange) {
			targetVelocity = direction.normalized * -speed;
		} else {
			targetVelocity = Vector2.zero;
		}
		if (direction.y < 0) {
			targetAngle = 360 - targetAngle;
		}
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("PinkChargedAnim")){
			animator.SetBool(animFireHash, TargetAquired());
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("PinkFiringAnim")) {
			firing = true;
		}
		if (firing && animator.GetCurrentAnimatorStateInfo(0).IsName("PinkChargeAnim")) {
			Instantiate(projectile, projectileSpawn.position, transform.rotation);
			firing = false;
		}
	}
	
	public override void PlayerKilled () {
		player = null;
	}

	bool TargetAquired () {
		RaycastHit2D raycast = Physics2D.Raycast (rigidbody2D.position, (Vector2)projectileSpawn.position - rigidbody2D.position, 50, 1<<11);
		return raycast.collider != null;
	}
}
