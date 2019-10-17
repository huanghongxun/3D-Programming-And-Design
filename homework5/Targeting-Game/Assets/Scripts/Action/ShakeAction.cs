using UnityEngine;
using System.Collections;

public class ShakeAction : MonoBehaviour
{
    private Vector3 center;
    private Vector3 axis;
    private float duration;
    private float period;
    private float angle;
    private float speed;
    private int times;
    private float time = 0;
    private Vector3 position;
    private Quaternion rotation;

    public void Shake(Vector3 center, Vector3 axis, float duration, float speed, float angle)
    {
        this.center = center;
        this.axis = axis;
        this.duration = duration;
        this.speed = speed;
        this.angle = angle;
        this.position = transform.localPosition;
        this.rotation = transform.localRotation;
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time > duration) time = duration;
        transform.localPosition = position;
        transform.localRotation = rotation;

        if (time >= duration)
        {
            Destroy(this);
        }
        else
        {
            transform.RotateAround(center, axis, angle * Mathf.Pow(0.4f, time) * Mathf.Sin(Mathf.Log(time + 1) * speed));
        }
    }
}
