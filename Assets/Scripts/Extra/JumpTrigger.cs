using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] private TriggerArea area;
    public float Force = 10;
    private void OnEnable()
    {
        area.onTriggerEnter += ForceJump;
    }

    private void OnDisable()
    {
        area.onTriggerEnter -= ForceJump;
    }

    void ForceJump(GameObject target)
    {
        //Todo fix for correct player when finished
        if(target.TryGetComponent<Rigidbody>(out Rigidbody rig))
        {
            var velocity = rig.linearVelocity;
            velocity.y = Force;
            rig.linearVelocity = velocity;
        }
    }

    private void Reset()
    {
        area = GetComponent<TriggerArea>();
    }
}
