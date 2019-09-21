using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundScript : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.parent.position, direction * Time.deltaTime, speed * Time.deltaTime);
    }
}
