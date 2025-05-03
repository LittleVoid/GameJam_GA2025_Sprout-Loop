using DG.Tweening;
using UnityEngine;

public class Vine_Plant_Env : Plant_Base_Env
{
    public float MaxLength;
    public float Growspeed;
    public GameObject GrowObject;

    protected override void OnPlacement()
    {
        float duration = MaxLength/Growspeed;
        GrowObject.transform.DOScaleY(MaxLength, duration);
    }
}
