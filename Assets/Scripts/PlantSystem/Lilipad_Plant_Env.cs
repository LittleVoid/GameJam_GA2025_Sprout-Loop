using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Lilipad_Plant_Env : Plant_Base_Env
{
    public LayerMask waterLayer, wallLayer;

    // Prefab comprises of two different lillies, disable the unneded one.
    public GameObject WallLily;
    public GameObject WaterLily;

    Vector3 waterPlacementStartOffset = Vector3.up * 0.2f;

    protected override void PlacePlant()
    {
        if (TryPlaceInWater())
        {
            return;
        }
        if (TryPlaceOnWall())
        {
            return;
        }
        Debug.LogError("failed to place plant");
    }

    private bool TryPlaceInWater()
    {
        if (!TryPlaceOn(transform.position + waterPlacementStartOffset, Vector3.down, placementDistance, waterLayer, this.transform, Quaternion.identity))
        {
            return false;
        }

        // diable the unneeded gameobject
        WallLily.gameObject.SetActive(false);
        return true;
    }

    private bool TryPlaceOnWall()
    {
        if (!(TryPlaceOn(transform.position, Vector3.right, placementDistance, wallLayer, this.transform, Quaternion.identity)
        || TryPlaceOn(transform.position, Vector3.left, placementDistance, wallLayer, this.transform, Quaternion.Euler(0, 180, 0))))
        {
            return false;
        }

        // diable the unneeded gameobject
        WaterLily.SetActive(false);
        return true;
    }

    public override bool CanPlaceAtPosition(Vector3 startpoint)
    {
        audioManager.PlaySFX("Splash");
        return CanPlaceAtPositionInternal(startpoint + waterPlacementStartOffset, Vector3.down, placementDistance, waterLayer) ||
           CanPlaceAtPositionInternal(startpoint, Vector3.left, placementDistance, wallLayer) ||
           CanPlaceAtPositionInternal(startpoint, Vector3.right, placementDistance, wallLayer);
    }
}
