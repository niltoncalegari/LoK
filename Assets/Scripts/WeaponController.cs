using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D snowBall;

    public float speed = 20f;
    public string gunButton = "Fire1_P1";
    private PlayerController playerCtrl;
    private float sprayShot = 1f;


    void Awake()
    {
        snowBall = (Rigidbody2D)Resources.Load("Prefabs/SnowBall", typeof(Rigidbody2D));
        playerCtrl = transform.root.GetComponent<PlayerController>();
    }


    void Update()
    {
        if (Input.GetButtonDown(gunButton))
        {
            InstantiateSnowBall(playerCtrl);
        }
    }

    public void InstantiateSnowBall(PlayerController playerLocalScale)
    {
        Rigidbody2D snowBallInstance = playerCtrl.transform.localScale.x > 0 ?
            Instantiate(snowBall, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D :
            Instantiate(snowBall, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
        Physics2D.IgnoreCollision(snowBallInstance.GetComponent<Collider2D>(), playerCtrl.GetComponent<Collider2D>());
        snowBallInstance.velocity = playerCtrl.transform.localScale.x > 0 ?
            new Vector2(speed, Random.Range(-sprayShot, sprayShot)) :
            new Vector2(-speed, Random.Range(-sprayShot, sprayShot));
    }
}
