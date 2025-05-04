using UnityEngine;
using UnityEngine.Events;

public class Watersensor : MonoBehaviour
{
    [SerializeField] private LayerMask _waterLayermask;

    public UnityEvent OnHitWater;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // we're only looking for water. bitshift because bitmask.
        if (1 << collision.gameObject.layer != _waterLayermask) { return; }

        OnHitWater?.Invoke();
    }
}
