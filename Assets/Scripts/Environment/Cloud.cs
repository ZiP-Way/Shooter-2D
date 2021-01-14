using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float cloudSpeed = 0.05f;

    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x - cloudSpeed, transform.position.y);

        if(transform.position.x < endPoint.position.x)
        {
            transform.position = new Vector2(startPoint.position.x, transform.position.y);
        }
    }
}
