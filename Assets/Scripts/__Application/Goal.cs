using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Goal : MonoBehaviour
{
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] Game _game;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer != _playerMask) { return; }

        _game.GameWon();
    }
}