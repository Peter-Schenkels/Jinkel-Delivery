using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaHolding : MonoBehaviour
{
    // Start is called before the first frame update
    bool isHoldingPizza = false;
    public float force = 10;
    public BoxCollider2D collider;
    SpriteRenderer spriteRenderer;
    GameObject pizza;
    BoxCollider2D pizzaCollider;
    Vector2 pizzaOffset;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
       
    }

        
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "pizza" && !isHoldingPizza)
        {
            //Physics2D.IgnoreCollision(collider, collision.gameObject.GetComponent<Collider2D>()); 
            isHoldingPizza = true;
            pizza = collision.gameObject;
            pizza.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    void throwPizza()
    {
        if(isHoldingPizza)
        {
            pizza.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.4f, 0);
            if(spriteRenderer.flipX == true)
            {
                pizza.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.5f, 1) * force);
            }
            else
            {
                pizza.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.5f, 1) * force);
            }
            pizza.GetComponent<Collider2D>().isTrigger = false;
            isHoldingPizza = false;
        }
    }

        // Update is called once per frame
    void Update()
    {
        if (isHoldingPizza)
        {
            pizza.transform.position = new Vector3(gameObject.transform.position.x + pizzaOffset.x, gameObject.transform.position.y + pizzaOffset.y, 0);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            throwPizza();
        }
    }
}
