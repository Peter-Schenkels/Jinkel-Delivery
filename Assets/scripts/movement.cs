using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer spriteRenderer;
    public float moveSpeed = 1.0f;
    public List<Sprite> running;
    public float framerate;
    int frame = 1;
    int frameDirection = 1;

    float nextFrameTimer = 0;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        nextFrameTimer = framerate / 60;
    }

    void UpdateAnimation()
    {
        nextFrameTimer -= Time.deltaTime;
        if(nextFrameTimer < 0)
        {
            spriteRenderer.sprite = running[frame];       
            if (frame == 4 || frame == 0)
            {
                frameDirection *= -1;
            }
            frame += frameDirection;
            nextFrameTimer = framerate / 60;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.right * -moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = true;
            UpdateAnimation();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = false;
            UpdateAnimation();
        }
        else
        {
            spriteRenderer.sprite = running[2];
        }
    }
}
