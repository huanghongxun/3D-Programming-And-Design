using UnityEditor;
using UnityEngine;

public class TraceAction : MonoBehaviour
{
    private GameObject target;

    public float speed;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        transform.rotation =
            Quaternion.LookRotation(target.transform.position - gameObject.transform.position, Vector3.up);
    }

    public TraceAction StartTrace(GameObject target, float speed)
    {
        TraceAction action = gameObject.AddComponent<TraceAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }
}
