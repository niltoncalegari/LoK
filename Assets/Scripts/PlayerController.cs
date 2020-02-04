using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpSpeed;
    public float dustYPositon;
    public float dustDownYPositon;
    public float dustXPositon;
    public float dustDownXPositon;
    [HideInInspector]
    public bool isJump;
    [HideInInspector]
    public Rigidbody2D myRB;

    private Animator anim;
    private Animator animWaepon;

    private Transform groundPoint;
    public LayerMask whatIsGround;
    public float groundRadius;
    [HideInInspector]
    public bool grounded;

    public AudioSource jumpSound;

    private CameraController theCamera;
    public float shakeAmount;

    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;
    [HideInInspector]
    public bool knockBack;

    private GameObject jumpDust;
    private GameObject jumDustDown;
    private GameObject weapon;

    const string horizontal = "Horizontal";
    const string jump = "Jump";
    const string fire1 = "Fire1";
    const string fire2 = "Fire2";

    private PlayerHealthManager playerHealth;

    // Use this for initialization
    void Start()
    {
        weapon = getChildGameObject(gameObject, "Weapon");
        groundPoint = getChildGameObject(gameObject, "GroundPoint").transform;
        //shotPoint = getChildGameObject(gameObject, "ShotPoint").transform;
        jumpDust = (GameObject)Resources.Load("Prefabs/Jump_Dust", typeof(GameObject));
        jumDustDown = (GameObject)Resources.Load("Prefabs/Jump_Dust_Down", typeof(GameObject));
        myRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        theCamera = FindObjectOfType<CameraController>();
        playerHealth = GetComponent<PlayerHealthManager>();
        myRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        animWaepon = weapon.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundPoint.position, groundRadius, whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {

        if (knockBack)
        {
            knockbackCounter -= Time.deltaTime;
            myRB.velocity = new Vector2(-knockbackForce * transform.localScale.x, myRB.velocity.y);

            if (knockbackCounter <= 0)
            {
                knockBack = false;
            }
        }
        else if (!playerHealth.dead)
        {
            Move();
            Jump();
            Flip();
        }

        if (playerHealth.dead)
        {
            if (myRB.velocity.x > 0.1f)
                myRB.velocity = new Vector2(myRB.velocity.x / 2f, myRB.velocity.y);
            else
                myRB.velocity = new Vector2(0f, myRB.velocity.y);
        }

        anim.SetFloat("Jump", Mathf.Abs(myRB.velocity.y));
        anim.SetFloat("Move", Mathf.Abs(myRB.velocity.x));
        anim.SetBool("Grounded", grounded);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("On Enemy");
            myRB.velocity = new Vector2(myRB.velocity.x, jumpSpeed * 0.75f);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (isJump && grounded)
            {
                var positionOk = new Vector2(transform.position.x + dustDownXPositon, transform.position.y - dustDownYPositon);

                Instantiate(jumDustDown, positionOk, transform.rotation);
                isJump = false;
            }

        }
    }

    private void Move()
    {        
        myRB.velocity = new Vector2(Input.GetAxis(horizontal) * moveSpeed, myRB.velocity.y);
    }

    private void Flip()
    {
        if (Input.GetAxisRaw(horizontal) > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (Input.GetAxisRaw(horizontal) < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

    }

    private void Jump()
    {
        if (Input.GetButtonDown(jump) && grounded)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpSpeed);
            isJump = true;
            var positionOk = new Vector2(transform.position.x + dustXPositon, transform.position.y - dustYPositon);
            Instantiate(jumpDust, positionOk, transform.rotation);
            jumpSound.Play();
        }
        if (!grounded && !isJump)
        {
            isJump = true;
        }
    }



    public void Knockback()
    {
        knockbackCounter = knockbackLength;
        myRB.velocity = new Vector2(-knockbackForce * transform.localScale.x, knockbackForce);
        knockBack = true;
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}