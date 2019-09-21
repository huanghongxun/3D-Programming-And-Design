using UnityEngine;

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
                var entity = hit.collider.GetComponent<Entity>();
                if (entity)
                {
                    entity.OnAction(entity);
                }
            }
        }
    }
}