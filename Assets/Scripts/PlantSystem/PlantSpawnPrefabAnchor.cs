using UnityEngine;

public class PlantSpawnPrefabAnchor : MonoBehaviour
{
    public static PlantSpawnPrefabAnchor instance {  get; private set; }
    private void OnEnable()
    {
        instance = this;
    }

    public static Transform GetParent()
    {
        return instance.transform;
    } 
}
