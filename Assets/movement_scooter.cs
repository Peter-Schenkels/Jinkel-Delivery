using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement_scooter : MonoBehaviour
{
    // Start is called before the first frame update
    public bool PlayerOnScooter = false;
    public GameObject player;
    Collider2D playerCollider;
    SpriteRenderer playerSpriteRenderer;

    public BoxCollider2D collider;
    public SpriteRenderer spriteRenderer;
    public float moveSpeed;

    void Start()
    {
        playerCollider = player.GetComponent<Collider2D>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerOnScooter = true;
            player.GetComponent<movement>().enabled = false;
            playerCollider.isTrigger = true;
            playerSpriteRenderer.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerOnScooter)
        {
            player.transform.position = gameObject.transform.position;
            if(Input.GetKeyDown(KeyCode.S))
            {
                PlayerOnScooter = false;
                player.transform.position = new Vector3(gameObject.transform.position.x + (collider.size.x + 0.1f) * System.Convert.ToInt32(!spriteRenderer.flipX), gameObject.transform.position.y, 0);
                player.GetComponent<movement>().enabled = true;
                player.GetComponent<Collider2D>().isTrigger = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.right * -moveSpeed * Time.deltaTime;
                spriteRenderer.flipX = true;
                playerSpriteRenderer.flipX = true;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                spriteRenderer.flipX = false;
                playerSpriteRenderer.flipX = false;
            }

        }
    }
}
