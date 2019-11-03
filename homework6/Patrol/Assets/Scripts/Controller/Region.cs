using System;
using UnityEngine;

public class Region : EntityRenderee<RegionModel, EntityRenderer>
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            model.OnCollisionWithPlayer(collision.gameObject);
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
