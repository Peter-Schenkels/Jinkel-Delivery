using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainationScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UI;
    AudioSource audioPlayer;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }


    public void ShowUI()
    {
        if(!Input.GetKeyDown(KeyCode.Space))
        {
            audioPlayer.Play();
            if(UI.active)
            {
                UI.active = false;
            }
            else
            {
                UI.active = true;
            }
        }
    }
}
