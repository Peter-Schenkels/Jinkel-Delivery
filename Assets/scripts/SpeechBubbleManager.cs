using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleManager : MonoBehaviour
{
    public List<pizzaSpeechBubble> bubbles;
    public Transform leftZone;
    public Transform rightZone;
    uint leftCollumnIndex = 0;
    uint rightCollumnIndex = 0;

    enum CollumnNumbers
    {
        left = 0,
        right = 1,
        none = 2
    }

    // Start is called before the first frame update
    void Start()
    {
        leftZone = transform.GetChild(0);
        rightZone = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        keepOnScreen();
    }

    void determineCollumn(pizzaSpeechBubble bubble)
    {
        if (bubble != null)
        {
            bubble.transform.position = bubble.position;
            if (bubble.transform.position.x > rightZone.position.x)
            {
                bubble.transform.position = new Vector3(rightZone.transform.position.x, rightZone.transform.position.y - rightCollumnIndex * 1.3f, bubble.position.z);
                bubble.flipped = true;
                rightCollumnIndex++;
            }
            else if (bubble.transform.position.x < leftZone.position.x)
            {
                bubble.transform.position = new Vector3(leftZone.transform.position.x, leftZone.transform.position.y - leftCollumnIndex * 1.3f, bubble.position.z);
                bubble.flipped = false;
                leftCollumnIndex++;
            }
            else
            {
                bubble.flipped = false;
                bubble.collumnNumber = (int)CollumnNumbers.none;
            }
        }
    }


    public void keepOnScreen()
    {
        foreach (var bubble in bubbles)
        {
            determineCollumn(bubble); ;

        }
        leftCollumnIndex = 0;
        rightCollumnIndex = 0;
    }

    


}
