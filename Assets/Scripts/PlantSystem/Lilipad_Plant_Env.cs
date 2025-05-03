using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Lilipad_Plant_Env : Plant_Base_Env
{
    public GameObject wallGO, waterGO;
    public LayerMask waterLayer, wallLayer;

    Vector3 waterPlacementStartOffset = Vector3.up * 0.2f;
    protected override void OnPlacement()
    {
        bool placed = false;
        if (!placed)
            placed = TryPlaceInWater();
        if (!placed)
            placed = TryPlaceOnWall();
    }

    
    public bool TryPlaceInWater()
    {
        if(TryPlaceOn(transform.position + waterPlacementStartOffset, Vector3.down, placementDistance, waterLayer, waterGO.transform, Quaternion.identity))
        {
            waterGO.SetActive(true);
            wallGO.SetActive(false);
            return true;
        }
        return false;
    }

    public bool TryPlaceOnWall()
    {
        if (TryPlaceOn(transform.position, Vector3.right, placementDistance, wallLayer, wallGO.transform, Quaternion.identity))
        {
            waterGO.SetActive(false);
            wallGO.SetActive(true);
            return true;
        }
        if(TryPlaceOn(transform.position, Vector3.left, placementDistance, wallLayer, wallGO.transform, Quaternion.Euler(0, 180, 0)))
        {
            waterGO.SetActive(false);
            wallGO.SetActive(true);
            return true;
        }

        return false;
    }

    public override bool CanPlaceOn(Vector3 startpoint)
    {
        if(CanPlaceOn(startpoint + waterPlacementStartOffset, Vector3.down, placementDistance, waterLayer, waterGO.transform, Quaternion.identity) ||
           CanPlaceOn(startpoint, Vector3.left, placementDistance, wallLayer, wallGO.transform, Quaternion.Euler(0, 180, 0)) ||
           CanPlaceOn(startpoint, Vector3.right, placementDistance, wallLayer, wallGO.transform, Quaternion.identity))
        {
            return true;
        }
        return false;
    }
}
