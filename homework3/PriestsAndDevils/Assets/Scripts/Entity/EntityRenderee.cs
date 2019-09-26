using UnityEngine;
using System.Collections;

public class EntityRenderee<TEntityModel, TEntityRenderer> : Entity
    where TEntityModel : MonoBehaviour
    where TEntityRenderer : EntityRenderer
{
    public TEntityRenderer renderer;
    public TEntityModel model;
}
