using UnityEngine;

public class PlantContainerManager : MonoBehaviour
{
    public PlantCollection_SO collection;

    [Button]
    public void Trigger()
    {
        collection.List[0].Use(Vector3.zero);
    }
}
