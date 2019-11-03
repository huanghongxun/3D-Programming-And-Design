using UnityEngine;
using System.Collections.Generic;

public class EnemyModel : Model
{
    public int score { get; set; }

    public GameObject target { get; set; }

    public float speed { get; set; }

    public Queue<Vector3> points { get; set; }

    public override void Start()
    {
        base.Start();

        Vector3 targetPoint = points.Dequeue();
        points.Enqueue(targetPoint);
        gameObject.transform.position = targetPoint;
    }

    public override void Update()
    {
        if (!target)
        {
            var traceAction = gameObject.GetComponent<TraceAction>();
            if (traceAction)
            {
                Object.Destroy(traceAction);
            }

            var moveAction = gameObject.GetComponent<MoveAction>();
            if (!moveAction)
            {
                // 没有目标的情况下沿指定点移动
                Vector3 targetPoint = points.Dequeue();
                points.Enqueue(targetPoint);

                MoveAction action = gameObject.AddComponent<MoveAction>();
                action.MovePosition(targetPoint);
                action.SetSpeed(0.45f);
                action.StartAction();
            }

            // 敌人巡逻时使用走路的动画
            gameObject.GetComponent<Animator>().SetInteger("Speed", 3);
        }
        else
        {
            var moveAction = gameObject.GetComponent<MoveAction>();
            if (moveAction)
            {
                Object.Destroy(moveAction);
            }

            var traceAction = gameObject.GetComponent<TraceAction>();
            if (!traceAction)
            {
                TraceAction action = gameObject.AddComponent<TraceAction>();
                action.StartTrace(target, speed);
            }

            // 敌人追赶玩家时使用跑步的动画
            gameObject.GetComponent<Animator>().SetInteger("speed", 7);
        }
    }

}
