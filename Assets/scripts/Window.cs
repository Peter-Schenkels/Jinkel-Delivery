 using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Window : MonoBehaviour
{

    public class WindowLet
    {
        public Sprite windowOpen;
        public AudioClip order;
        public AudioClip deliver;
        public AudioClip late;
    }

    public List<Sprite> windowOpen;
    public List<AudioClip> order;
    public List<AudioClip> deliver;
    public List<AudioClip> late;

    public float timerTime;
    public bool wantPizza { get; set; } = false;
    public GameObject speechBubble;
    GameObject camera;
    bool tooLate;

    SpriteRenderer spriteRenderer;
    Sprite windowClosed;
    GameObject speechBubbleGameObject;
    pizzaSpeechBubble activeSpeechBubble;
    PizzaHolding player;
    PlayerStats playerStats;
    int windowSpriteNr = 0;
    AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        windowClosed = spriteRenderer.sprite;
        GameObject playerTemp = GameObject.Find("Player");
        playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
        player = playerTemp.GetComponent<PizzaHolding>();
        windowSpriteNr = (int)UnityEngine.Random.Range(0, 9);
        audioPlayer = gameObject.AddComponent<AudioSource>();
        if(windowSpriteNr == 1 || windowSpriteNr == 5 || windowSpriteNr == 3)
        {
            audioPlayer.volume = 1.2f;

        }
        else
        {
            audioPlayer.volume = 0.3f;
        }
        audioPlayer.rolloffMode = AudioRolloffMode.Linear;
        audioPlayer.minDistance = 1;
        audioPlayer.maxDistance = 120;
        audioPlayer.spatialBlend = 1f;



    }

    public void orderPizza(float timerTime = 30f)
    {

        this.timerTime = timerTime;
        wantPizza = true;
        speechBubbleGameObject = Instantiate(speechBubble);
        speechBubbleGameObject.transform.parent = this.transform;
        activeSpeechBubble = speechBubbleGameObject.GetComponent<pizzaSpeechBubble>();
        speechBubbleGameObject.transform.position = new Vector3(this.transform.position.x + 1.3f, this.transform.position.y + 1.3f, -9f);
        tooLate = false;
        audioPlayer.PlayOneShot(order[windowSpriteNr]);
    }

    // Update is called once per framez
    void Update()
    {
        if (wantPizza)
        {
            spriteRenderer.sprite = windowOpen[windowSpriteNr];
            updatePizzaTimer();
        }
    }

    void updatePizzaTimer()
    {
        timerTime -= Time.deltaTime;
        activeSpeechBubble.updatePizzaTimerText(timerTime);
        if(timerTime < 0 && !tooLate)
        {
            tooLate = true;
            audioPlayer.PlayOneShot(late[windowSpriteNr]);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (wantPizza && collision.gameObject.tag == "pizza" && !player.isHoldingPizza)
        {
            pizzaCollision(collision.gameObject);
        }
    }

    void pizzaCollision(GameObject gameObject)
    {
        gameObject.active = false;
        wantPizza = false;
        player.isHoldingPizza = false;
        spriteRenderer.sprite = windowClosed;
        activeSpeechBubble.GetComponent<pizzaSpeechBubble>().deleteBubble();

        if (gameObject.GetComponent<pizzaSplat>().splatted)
        {
            playerStats.addMoney(3);
            playerStats.nrOfDeliveredOrders++;
            audioPlayer.PlayOneShot(late[windowSpriteNr]);
            
        }
        else if (timerTime > 0)
        {
            playerStats.addMoney(10);
            playerStats.nrOfDeliveredOrders++;
            audioPlayer.PlayOneShot(deliver[windowSpriteNr]);
        }
        else
        {
            playerStats.addMoney(0);
            playerStats.nrOfDeliveredOrders++;
            playerStats.nrOfPizzaNotInTime++;
            audioPlayer.PlayOneShot(late[windowSpriteNr]);
        }
    }
}
