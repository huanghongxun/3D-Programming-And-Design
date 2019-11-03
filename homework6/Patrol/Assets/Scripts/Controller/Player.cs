using System;
using UnityEngine;

public class Player : EntityRenderee<PlayerModel, EntityRenderer>
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            model.OnCollisionWithEnemy(collision.gameObject);
        }
    }

    public void Update()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) movement += Vector3.forward;
        if (Input.GetKey(KeyCode.A)) movement += Vector3.left;
        if (Input.GetKey(KeyCode.S)) movement += Vector3.back;
        if (Input.GetKey(KeyCode.D)) movement += Vector3.right;

        if (movement != Vector3.zero)
        {
            GetComponent<Animator>().SetInteger("Speed", 10);
        }
        else
        {
            GetComponent<Animator>().SetInteger("Speed", 0);
        }

        transform.position += movement * Time.deltaTime * 3;
        transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
    }


    public class Factory : EntityFactory<Player, PlayerModel, EntityRenderer, Factory>
    {
        public override EntityRenderer InstantiateImpl(PlayerModel model)
        {
            return EntityRendererFactory.Instance.CreateGameObject<EntityRenderer>("Prefabs/Player");
        }
    }
}
