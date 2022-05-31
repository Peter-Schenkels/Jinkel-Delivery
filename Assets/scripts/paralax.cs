using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralax : MonoBehaviour
{

    public float intensity;
    public GameObject camera;
    public Vector3 center;
    Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camera.transform.position.x / intensity + center.x, camera.transform.position.y / intensity + center.y, center.z);
    }
}
