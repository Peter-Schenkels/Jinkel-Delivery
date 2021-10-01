using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pizzaSplat : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer spriteRenderer;
    public Sprite splattedPizzaPie;
    public bool splatted = false;
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Scooter" || collision.gameObject.tag == "Floor")
        {
            splatted = true;
            spriteRenderer.sprite = splattedPizzaPie;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
