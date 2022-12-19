using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRotation : MonoBehaviour
{
    public float strength = 0;
    void LateUpdate()
    {
        transform.Rotate( 0f, strength * Time.deltaTime, 0f, Space.Self);
    }
}
