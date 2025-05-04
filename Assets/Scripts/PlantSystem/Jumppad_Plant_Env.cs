using UnityEngine;

public class Jumppad_Plant_Env : Plant_Base_Env
{
    Vector3 placementStartOffset = Vector3.up * 0.2f;

    protected override void PlacePlant()
    {
        TryPlaceOn(transform.position + placementStartOffset, Vector3.down, placementDistance, placementLayer, transform, Quaternion.identity);
    }

    public override bool CanPlaceAtPosition(Vector3 startpoint)
    {
        audioManager.PlaySFX("Sprout");
        return CanPlaceAtPositionInternal(startpoint + placementStartOffset, Vector3.down, placementDistance, placementLayer);
    }
}
