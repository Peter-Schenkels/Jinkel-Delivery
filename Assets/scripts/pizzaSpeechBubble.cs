using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pizzaSpeechBubble : MonoBehaviour
{
    // Start is called before the first frame update
    public Text bubbleText;
    float timerTime = 0;
    bool pizzaFlag = false;
    GameObject camera;
    public int collumnNumber = 0;
    List<pizzaSpeechBubble> bubbles;
    SpriteRenderer sprite;
    public bool flipped = false;
    public Sprite unhappy;
    public Sprite stage1;
    public Sprite stage2;
    public Sprite stage3;
    public SpriteRenderer sign;

    public Vector3 position;
    void Start()
    {
        if(bubbleText == null)
        {
            bubbleText = gameObject.AddComponent<Text>();
        }
        position = transform.position;
        camera = GameObject.Find("Main Camera");
        bubbles = camera.GetComponent<SpeechBubbleManager>().bubbles;
        bubbles.Add(this);
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //keepOnScreen();
        sprite.flipX = flipped;
        if(flipped)
        {
            sign.transform.localPosition = new Vector3(-0.31f, 0.14f, -0.01f);
        }
        else
        {
            sign.transform.localPosition = new Vector3(0.31f, 0.14f, -0.01f);
        }
    }

    public void updatePizzaTimerText(float timerTime)
    {
        if (timerTime < 0)
        {
            sign.sprite = unhappy;
            pizzaFlag = false;
        }
        else
        {
            if (timerTime > 20) 
            {
                sign.sprite = stage1;
            }
            else if(timerTime > 12.5f)
            {
                sign.sprite = stage2;
            }
            else if(timerTime > 7.5f)
            {
                sign.sprite = stage3;
            }

        }
    }

    public void deleteBubble()
    {
        this.gameObject.active = false;
        bubbles.Remove(this);
        Destroy(this);
    }
}
