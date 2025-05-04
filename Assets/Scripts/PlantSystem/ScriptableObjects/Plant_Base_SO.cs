using UnityEngine;

[CreateAssetMenu(fileName = "Plant_Base_SO", menuName = "Plant_Base_SO", order = 0)]
public class Plant_Base_SO : ScriptableObject {
    public Sprite displaySprite;
    public string displayName;
    public GameObject Prefab;

    public virtual void Use(Vector3 pos)
    {
        GameObject go = Instantiate(Prefab,pos,Quaternion.identity,PlantSpawnPrefabAnchor.GetParent());
    }

    public virtual bool CanPlace(Vector3 pos)
    {
        return Prefab.GetComponent<Plant_Base_Env>().CanPlaceAtPosition(pos);
    }
}
