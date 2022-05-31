using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement_scooter : MonoBehaviour
{
    // Start is called before the first frame update
    public bool PlayerOnScooter = false;
    public bool playerTouchScooterFlag = false;
    public GameObject player;
    Collider2D playerCollider;
    SpriteRenderer playerSpriteRenderer;
    movement playerMovement;
    Rigidbody2D playerRigidBody;
    Rigidbody2D scooterRigidBody;
    AudioSource motorSound;

    public Collider2D scooterCollider;
    public SpriteRenderer spriteRenderer;
    public float moveSpeed;

    float scooterCooldownTimerConstant = 0.75f;
    float scooterCooldownTimer;
    bool scooterCooldownFlag = true;
    public GameObject explosion;
    public GameObject FrontLight;
    public Vector3 offsetFrontLightFlipped;

    void Start()
    {
        playerCollider = player.GetComponent<Collider2D>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        playerMovement = player.GetComponent<movement>();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        scooterRigidBody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(scooterCollider, playerCollider, true);
        motorSound = GetComponent<AudioSource>();
    }

    void updateMotorSound()
    {
        motorSound.pitch = 0.8f + ((Mathf.Abs(scooterRigidBody.velocity.x) / moveSpeed) * 2.5f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (scooterRigidBody.rotation < -90 && scooterRigidBody.rotation > -180 || scooterRigidBody.rotation > 90 && scooterRigidBody.rotation < 180)
            {
                scooterRigidBody.rotation = 0;
                scooterRigidBody.AddForce(new Vector2(0, 1));
            }

        }
    }

    public void PlayerOnScooterCheck(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!PlayerOnScooter && Input.GetKey(KeyCode.S) && scooterCooldownFlag)
            {
                PlayerOnScooter = true;
                playerMovement.enabled = false;
                playerSpriteRenderer.flipX = spriteRenderer.flipX;
                scooterCooldownFlag = false;
                playerRigidBody.simulated = false;
                scooterCooldownTimer = 1;
            }
            playerTouchScooterFlag = false;
        }
    }
    void ScooterMovement()
    {
        if (PlayerOnScooter)
        {
            player.transform.position = gameObject.transform.position + new Vector3(0, 0.3f, 0.01f);
            if (Input.GetKey(KeyCode.S) && scooterCooldownFlag)
            {
                player.transform.position += new Vector3(0, 1, 0);
                PlayerOnScooter = false;
                scooterCooldownFlag = false;
                playerMovement.enabled = true;
                playerRigidBody.simulated = true;
                scooterCooldownTimer = 1;
                return;
            }
            if (Mathf.Abs(scooterRigidBody.velocity.x) < moveSpeed)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    scooterRigidBody.AddForce(Vector3.right * -800 * Time.deltaTime);
                    playerSpriteRenderer.flipX = true;
                    transform.localScale = new Vector3(-1, 1, 1);

                }
                if (Input.GetKey(KeyCode.D))
                {
                    scooterRigidBody.AddForce(Vector3.right * 800 * Time.deltaTime);
                    playerSpriteRenderer.flipX = false;
                    transform.localScale = new Vector3(1, 1, 1);
                }
            } 
        }
        if ((scooterRigidBody.rotation < -30 || scooterRigidBody.rotation > 30))
        {
            scooterRigidBody.angularVelocity = 0;
        }

    }


    void updateTimer()
    {
        if (scooterCooldownTimer <= 0 && !scooterCooldownFlag)
        {
            scooterCooldownTimer = scooterCooldownTimerConstant;
            scooterCooldownFlag = true;
        }
        else
        {
            scooterCooldownTimer -= Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        updateTimer();
        ScooterMovement();
        updateMotorSound();
    }
}
