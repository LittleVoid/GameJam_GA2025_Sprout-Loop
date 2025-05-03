using UnityEngine;

public class CanPlaceTester : MonoBehaviour
{
    public Plant_Base_SO plant;
    void Update()
    {
        Debug.Log("Plant:"+ plant.displayName + " could be placed:"+plant.CanPlace(transform.position));
    }
}
