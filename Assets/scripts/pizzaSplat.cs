using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pizzaSplat : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer spriteRenderer;
    public Sprite splattedPizzaPie;
    public bool splatted;
    public AudioClip pizzaSplatClip;
    AudioSource audioPlayer;
    public PlayerStats playerStats;
    void Start()
    {
        splatted = false;
        audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.rolloffMode = AudioRolloffMode.Linear;
        audioPlayer.minDistance = 1;
        audioPlayer.maxDistance = 120;
        audioPlayer.spatialBlend = 1f;
        playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Scooter" || collision.gameObject.tag == "Floor" && !splatted)
        {
            splatted = true;
            spriteRenderer.sprite = splattedPizzaPie;
            audioPlayer.PlayOneShot(pizzaSplatClip);
            playerStats.nrOfDroppedPizza++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
