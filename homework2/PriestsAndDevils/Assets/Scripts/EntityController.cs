﻿using UnityEngine;

/// <summary>
/// 负责将点击事件发送给 Entity.OnAction
/// </summary>
public class EntityController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                for (var t = hit.collider.transform; t; t = t.parent)
                    if (t.GetComponent<DisableEntityAction>())
                        return;

                for (var t = hit.collider.transform; t; t = t.parent)
                {
                    var entity = t.GetComponent<Entity>();
                    if (!entity) continue;
                    entity.OnAction(entity);
                    break;
                }
            }
        }
    }
}
