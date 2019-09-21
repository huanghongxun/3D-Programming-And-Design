using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 表示一个可以移动的实体，比如船、人。
/// 属于 Controller
/// </summary>
public class Entity : MonoBehaviour
{
    /// <summary>
    /// 默认行为是将点击事件返回上级处理
    /// </summary>
    /// <param name="entity"></param>
    public virtual void OnAction(Entity entity)
    {
        transform.parent?.GetComponent<Entity>()?.OnAction(entity);
    }


}
