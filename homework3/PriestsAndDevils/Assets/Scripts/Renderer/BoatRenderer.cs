
public class BoatRenderer : EntityRenderer
{
    public class BoatRendererFactory : EntityRendererFactory<Boat, BoatModel, BoatRenderer>
    {
        public override string GetPath()
        {
            return "Prefabs/OldBoat";
        }
    }
}
