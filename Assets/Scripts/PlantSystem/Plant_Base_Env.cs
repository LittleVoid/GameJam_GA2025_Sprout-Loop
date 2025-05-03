using UnityEngine;

public class Plant_Base_Env : MonoBehaviour
{
    public Plant_Base_SO Data;

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
}


