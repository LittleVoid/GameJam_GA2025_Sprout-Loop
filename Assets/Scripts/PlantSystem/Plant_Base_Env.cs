using UnityEngine;

public class Plant_Base_Env : MonoBehaviour
{
    public Plant_Base_SO Data;
    public float placementDistance;
    public LayerMask placementLayer;

   
    private void OnEnable()
    {
        OnPlacement();
    }

    [Button]
    public virtual void OnPlacementButton()
    {
        OnPlacement();
    }

    [Button]
    public virtual void OnPlacementDeath()
    {
        OnPlacement();
    }

    protected virtual void OnPlacement()
    {

    }

    public bool TryPlaceOn(Vector3 startpoint, Vector3 dir, float maxDistance, LayerMask placementLayer, Transform obj, Quaternion rotation)
    {
        var result = ExtensionMethods.GetNextHitpointOnTerrain(startpoint, dir, maxDistance, placementLayer);
        if (result != startpoint)
        {
            obj.gameObject.SetActive(true);

            obj.transform.position = result;
            obj.transform.rotation = rotation;
            return true;
        }
        return false;
    }
    public virtual bool CanPlaceOn(Vector3 startpoint)
    {
        return true;
    }
    protected virtual bool CanPlaceOn(Vector3 startpoint, Vector3 dir, float maxDistance, LayerMask placementLayer, Transform obj, Quaternion rotation)
    {
        var result = ExtensionMethods.GetNextHitpointOnTerrain(startpoint, dir, maxDistance, placementLayer);
        if (result != startpoint)
        {
            return true;
        }
        return false;
    }


    //will die because of lilypad or death water...
}


