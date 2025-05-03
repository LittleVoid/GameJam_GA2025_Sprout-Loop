using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlantCollection_SO", menuName = "PlantCollection_SO", order = 0)]
public class PlantCollection_SO : ScriptableObject
{
    public List<Plant_Base_SO> List = new List<Plant_Base_SO>();
}
