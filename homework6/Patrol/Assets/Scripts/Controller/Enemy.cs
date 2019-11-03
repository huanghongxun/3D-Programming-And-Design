using System;
using UnityEngine;

public class Enemy : EntityRenderee<EnemyModel, EntityRenderer>
{
    public void Trace(GameObject target, float speed)
    {
        model.target = target;
        model.speed = speed;
    }

    public void Wander()
    {
        model.target = null;
    }

    public class Factory : EntityFactory<Enemy, EnemyModel, EntityRenderer, Factory>
    {
        public override EntityRenderer InstantiateImpl(EnemyModel model)
        {
            return EntityRendererFactory.Instance.CreateGameObject<EntityRenderer>("Prefabs/Enemy");
        }
    }
}
