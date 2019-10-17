using UnityEngine;

public class Arrow : EntityRenderee<ArrowModel, ArrowRenderer>
{
    void Update()
    {
        model.CheckAlive();
    }

    public void AimAt(Vector3 direction)
    {
        model.AimAt(direction);
    }

    public void Shoot(Vector3 direction)
    {
        model.Shoot(direction);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            model.Hit(contact.point);
            break;
        }
    }

    public class Factory : EntityFactory<Arrow, ArrowModel, ArrowRenderer, Factory>
    {
        public override ArrowRenderer InstantiateImpl(ArrowModel model)
        {
            return EntityRendererFactory.Instance.CreateGameObject<ArrowRenderer>("Prefabs/Arrow");
        }
    }
}
