using UnityEngine;

public class Jumppad_Plant_Env : Plant_Base_Env
{

    Vector3 placementStartOffset = Vector3.up * 0.2f;
    protected override void OnPlacement()
    {
        TryPlaceOn(transform.position + placementStartOffset, Vector3.down, placementDistance, placementLayer, transform, Quaternion.identity);
    }

    public override bool CanPlaceOn(Vector3 startpoint)
    {
        return CanPlaceOn(startpoint + placementStartOffset, Vector3.down, placementDistance, placementLayer, transform, Quaternion.identity);
    }
}
