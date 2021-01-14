using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public void DestroyAtTheEnd() // destroy when the animation ends
    {
        Destroy(gameObject);
    }
}
