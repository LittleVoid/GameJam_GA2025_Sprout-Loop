using UnityEngine;

public abstract class Plant_Base_Env : MonoBehaviour
{
    public Plant_Base_SO Data;
    public float placementDistance;
    public LayerMask placementLayer;

    private void OnEnable()
    {
        Debug.Log($"Spawned plan type of {this.GetType().Name} GO: {name}");
        PlacePlant();
    }

    [Button]
    protected abstract void PlacePlant();
    public abstract bool CanPlaceAtPosition(Vector3 startpoint);

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

    protected bool CanPlaceAtPositionInternal(Vector3 startpoint, Vector3 dir, float maxDistance, LayerMask placementLayer)
    {
        var result = ExtensionMethods.GetNextHitpointOnTerrain(startpoint, dir, maxDistance, placementLayer);
        if (result != startpoint)
        {
            return true;
        }
        return false;
    }
}
