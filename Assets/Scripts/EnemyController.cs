using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
    
	public float moveSpeed;
	private Rigidbody2D myRB;

	public PlayerController thePlayer;

	public int damageOnTouch;

	public Transform wallCheck;
	public float wallRadius;
	public LayerMask whatIsWall;

	private bool moveRight;

	public float knockbackForce;
	public float knockbackLength;
	private float knockbackCounter;
	private bool knockBack;

	// Use this for initialization
	void Start () {
		myRB = GetComponent<Rigidbody2D>();
		thePlayer = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		

		if(Physics2D.OverlapCircle(wallCheck.position, wallRadius,whatIsWall))
			moveRight = !moveRight;

		if(knockBack)
		{
			knockbackCounter -= Time.deltaTime;
			myRB.velocity = new Vector2(knockbackForce * transform.localScale.x, myRB.velocity.y);

			if(knockbackCounter <= 0)
			{
				knockBack = false;
			}
			return;
		}

		if(!moveRight)
		{
			myRB.velocity = new Vector2(-moveSpeed, myRB.velocity.y);
			transform.localScale = new Vector3(1f,1f,1f);
		}
		if(moveRight)
		{
			myRB.velocity = new Vector2(moveSpeed, myRB.velocity.y);
			transform.localScale = new Vector3(-1f,1f,1f);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damageOnTouch);
		}
	}

	public void Knockback()
	{
		knockbackCounter = knockbackLength;
		myRB.velocity = new Vector2(knockbackForce * transform.localScale.x, knockbackForce);
		knockBack = true;
	}
}