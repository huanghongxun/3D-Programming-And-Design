using UnityEngine;
using System;


/// <summary>
/// 表示一个可以移动的实体，比如船、人。
/// 属于 Controller
/// </summary>
public class Entity : MonoBehaviour
{
    public event EventHandler<ActionEventArgs> Action;

    /// <summary>
    /// 默认行为是将点击事件返回上级处理
    /// </summary>
    /// <param name="entity"></param>
    public virtual void OnAction(GameObject gameObject)
    {
        transform.parent?.GetComponent<Entity>()?.OnAction(gameObject);
        Action?.Invoke(this, new ActionEventArgs
        {
            gameObject = gameObject
        });
    }

    public virtual void OnCollect()
    {
    }

    public virtual void OnSpawn()
    {
    }

    public class ActionEventArgs
    {
        public GameObject gameObject { get; set; }
    }
}
