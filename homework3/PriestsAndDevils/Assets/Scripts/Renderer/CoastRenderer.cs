using UnityEngine;
using System.Collections;

public class CoastRenderer : EntityRenderer
{
    public class CoastRendererFactory : EntityRendererFactory<Coast, CoastModel, CoastRenderer>
    {
        public override string GetPath()
        {
            return "Prefabs/Coast";
        }
    }
}
