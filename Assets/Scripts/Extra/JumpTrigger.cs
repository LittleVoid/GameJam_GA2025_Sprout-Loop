using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayerMask;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer != _playerLayerMask) { return; }

        collision.gameObject.GetComponent<PlantCharacterController>().SuperJump();
    }
}
