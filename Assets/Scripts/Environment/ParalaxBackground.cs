using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    [SerializeField] private float paralaxMovement;

    private Transform cameraTransform;
    private float startPos;
    private float length;

    private void Start()
    {
        startPos = transform.position.x;
        cameraTransform = Camera.main.transform;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (cameraTransform.position.x * (1 - paralaxMovement));
        float dist = (cameraTransform.position.x * paralaxMovement);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length - 10) startPos += length;
        else if (temp < startPos - length + 10) startPos -= length;
    }
}
