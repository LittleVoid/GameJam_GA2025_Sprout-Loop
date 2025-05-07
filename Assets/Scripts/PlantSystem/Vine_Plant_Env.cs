using DG.Tweening;
using UnityEngine;

public class Vine_Plant_Env : Plant_Base_Env
{
    public float MaxLength;
    public float Growspeed;
    public GameObject GrowObject;

    Vector3 placementOffset = Vector3.up * 0.2f;

    protected override void PlacePlant()
    {
        TryPlaceOn(transform.position + placementOffset, Vector3.down, placementDistance, placementLayer, transform, Quaternion.identity);

        float duration = MaxLength / Growspeed;
        GrowObject.transform.DOScaleY(MaxLength, duration);
    }

    [Button]
    public override bool CanPlaceAtPosition(Vector3 startpoint)
    {
        AudioManager.Instance.PlaySFX("Grow");
        return CanPlaceAtPositionInternal(startpoint + placementOffset, Vector3.down, placementDistance, placementLayer);
    }
}
