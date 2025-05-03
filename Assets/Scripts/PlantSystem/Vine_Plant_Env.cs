using DG.Tweening;
using UnityEngine;

public class Vine_Plant_Env : Plant_Base_Env
{
    public float MaxLength;
    public float Growspeed;
    public GameObject GrowObject;


    Vector3 placementOffset = Vector3.up * 0.2f;
    protected override void OnPlacement()
    {
        TryPlaceOn(transform.position + placementOffset, Vector3.down, placementDistance, placementLayer, transform, Quaternion.identity);

        float duration = MaxLength/Growspeed;
        GrowObject.transform.DOScaleY(MaxLength, duration);
    }

    [Button]
    public override bool CanPlaceOn(Vector3 startpoint)
    {
        return CanPlaceOn(startpoint + placementOffset, Vector3.down, placementDistance, placementLayer, transform, Quaternion.identity);
    }


}
