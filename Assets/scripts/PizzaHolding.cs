using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaHolding : MonoBehaviour
{
    // Start is called before the first frame update
    public float force = 10;
    public Collider2D collider;

    public bool isHoldingPizza = false;
    SpriteRenderer spriteRenderer;
    GameObject pizza;
    Rigidbody2D pizzaRigidbody;
    Collider2D pizzaCollider;
    Vector2 pizzaOffset;
    float pizzaCooldownTimer = 0;
    public AudioClip throwPizzaClip;
    public AudioClip holdPizzaClip;
    public AudioSource audioPlayer;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioPlayer = gameObject.AddComponent<AudioSource>();
    }

    public void reset()
    {
        isHoldingPizza = false;
        pizzaCooldownTimer = 0;
    }
       
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "pizza" && !isHoldingPizza && pizzaCooldownTimer <= 0)
        {
            isHoldingPizza = true;
            pizza = collision.gameObject;
            pizzaCollider = pizza.GetComponent<Collider2D>();
            pizzaCollider.isTrigger = true;
            pizzaRigidbody = pizza.GetComponent<Rigidbody2D>();
            pizzaRigidbody.angularVelocity = 0;
            pizzaRigidbody.velocity = new Vector2(0, 0);
            audioPlayer.PlayOneShot(holdPizzaClip);
        }
    }

    void throwPizza()
    {
        if(isHoldingPizza)
        {
            if(pizza != null)
            {
                isHoldingPizza = false;
                pizzaCooldownTimer = 0.5f;
                pizza.transform.position += new Vector3(0, 1f, 0);
                pizzaRigidbody.SetRotation(0);
                pizzaRigidbody.velocity = new Vector2(0,0);
                pizzaRigidbody.angularVelocity = 0;
                if (spriteRenderer.flipX == true)
                {
                    pizzaRigidbody.AddForce(new Vector2(-0.03f, 0.076f) * force);
                }
                else
                {
                    pizzaRigidbody.AddForce(new Vector2(0.03f, 0.076f) * force);
                }
                pizzaCollider.isTrigger = false;
                audioPlayer.PlayOneShot(throwPizzaClip);
            }
        }
    }


    void updatePizzaCooldown()
    {
        pizzaCooldownTimer -= Time.deltaTime;
    }

        // Update is called once per frame
    void Update()
    {
        if (isHoldingPizza)
        {
            if(pizza != null)
            {
                pizza.transform.position = new Vector3(gameObject.transform.position.x + pizzaOffset.x, gameObject.transform.position.y + pizzaOffset.y, pizza.transform.position.z);
            }
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            throwPizza();
        }
        updatePizzaCooldown();
    }
}
