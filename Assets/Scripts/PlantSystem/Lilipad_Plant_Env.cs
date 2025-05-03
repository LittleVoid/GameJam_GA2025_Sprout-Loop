using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Lilipad_Plant_Env : Plant_Base_Env
{
    public GameObject wallGO, waterGO;

    public LayerMask waterLayer, wallLayer;


    protected override void OnPlacement()
    {
        bool placed = false;
        if (!placed)
            placed = TryPlaceInWater();
        if(!placed)
            placed = TryPlaceOnWall();

        Debug.Log("Placement lilipad:" + placed);
    }

    
    public bool TryPlaceInWater()
    {
        Vector3 startpoint = transform.position + Vector3.up * 0.2f;
        var result = ExtensionMethods.GetNextHitpointOnTerrain(startpoint, Vector3.down,1,waterLayer);
        if (result != startpoint)
        {
            waterGO.SetActive(true);
            wallGO.SetActive(false);

            waterGO.transform.position = result;
            return true;
        }
        return false;
    }

    public bool TryPlaceOnWall()
    {
        Vector3 startpoint = transform.position;
        //right check
        var result = ExtensionMethods.GetNextHitpointOnTerrain(startpoint, Vector3.right, 2, wallLayer);
        if (result != startpoint)
        {
            waterGO.SetActive(false);
            wallGO.SetActive(true);

            wallGO.transform.position = result;
            return true;
        }

        //left check
        result = ExtensionMethods.GetNextHitpointOnTerrain(startpoint, Vector3.left, 2, wallLayer);
        if (result != startpoint)
        {
            waterGO.SetActive(false);
            wallGO.SetActive(true);

            wallGO.transform.position = result;
            wallGO.transform.rotation = Quaternion.Euler(0,180,0);
            return true;
        }


        return false;
    }
}
