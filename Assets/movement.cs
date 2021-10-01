using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer spriteRenderer;
    public float moveSpeed = 1.0f;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.right * -moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = true;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = false;
        }
    }
}
