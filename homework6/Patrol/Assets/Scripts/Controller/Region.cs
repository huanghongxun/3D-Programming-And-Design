using System;
using UnityEngine;

public class Region : EntityRenderee<RegionModel, EntityRenderer>
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            model.OnCollisionWithPlayer(collider.gameObject);
        }
    }

    public class Factory : EntityFactory<Region, RegionModel, EntityRenderer, Factory>
    {
        public override EntityRenderer InstantiateImpl(RegionModel model)
        {
            return EntityRendererFactory.Instance.CreateGameObject<EntityRenderer>("Prefabs/Collider");
        }
    }
}
