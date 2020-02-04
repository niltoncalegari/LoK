using UnityEngine;
using System.Collections;

public class EnemyHealthManager : MonoBehaviour {
    
	public int startingHealth;
	private int currentHealth;

	public SpriteRenderer[] bodyParts;
	public float flashLength;
	private float flashCounter;

	private EnemyController theEC;
	private Animator anim;
	private Rigidbody2D myRB;

	public float deathSpin;

	public Sprite[] brokenParts;
	private bool dead;

	public GameObject explosion;

	public Rigidbody2D[] RBParts;
	public float explosionForce;

	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;
		theEC = GetComponent<EnemyController>();
		anim = GetComponent<Animator>();
		myRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentHealth <= 0 && !dead)
		{
			//gameObject.SetActive(false);

			theEC.enabled = false;
			anim.enabled = false;

			myRB.constraints = RigidbodyConstraints2D.None;
			myRB.AddTorque(deathSpin * -transform.localScale.x);
			gameObject.layer = LayerMask.NameToLayer("Dead");

			for(int i = 0; i < bodyParts.Length; i++)
			{
				bodyParts[i].sprite = brokenParts[i];
			}

			Instantiate(explosion, transform.position, transform.rotation);

			for(int i = 0; i < RBParts.Length; i++)
			{
				RBParts[i].isKinematic = false;
				RBParts[i].AddTorque(deathSpin);
				RBParts[i].velocity = new Vector2(Random.Range(-explosionForce, explosionForce), Random.Range(-explosionForce, explosionForce));
			}

			dead = true;
		}

		if(flashCounter > 0)
		{
			flashCounter -= Time.deltaTime;

			if(flashCounter <= 0)
			{
				for(int i = 0; i < bodyParts.Length; i++)
				{
					bodyParts[i].material.SetFloat("_FlashAmount", 0);
				}
			}
		}
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		theEC.Knockback();
		Flash();
	}

	public void Flash()
	{
		for(int i = 0; i < bodyParts.Length; i++)
		{
			bodyParts[i].material.SetFloat("_FlashAmount", 1);
		}
		flashCounter = flashLength;
	}
}