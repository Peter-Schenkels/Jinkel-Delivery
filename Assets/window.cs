using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class window : MonoBehaviour
{

    public Sprite windowOpen;
    private DateTime time;
    bool wantPizza = false;
    SpriteRenderer spriteRenderer;
    Sprite windowClosed;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        windowClosed = spriteRenderer.sprite;
        //UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        wantPizza = UnityEngine.Random.RandomRange(-1, 2) > 0;
        print(UnityEngine.Random.RandomRange(-1, 2));
    }

    // Update is called once per frame
    void Update()
    {
        if(wantPizza)
        {
            spriteRenderer.sprite = windowOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "pizza" && wantPizza)
        {
            collision.gameObject.active = false;
            spriteRenderer.sprite = windowClosed;
            wantPizza = false;
            print("hohohohohohohoh");
        }
    }

}
