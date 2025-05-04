using System;
using UnityEngine;

public class PlantAnimator : MonoBehaviour
{
    [SerializeField] private string _running = "IsRunning";
    [SerializeField] private string _airborne = "IsAirborne";
    [SerializeField] private string _climbing = "IsClimbing";
    [SerializeField] private PlantCharacterController _plantCharacterController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _amature;

    void Update()
    {
        if (_rb.linearVelocityX > 0f)
        {
            _amature.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (_rb.linearVelocityX < 0f)
        {
            _amature.rotation = Quaternion.Euler(0, -90, 0);
        }

        switch (_plantCharacterController.State)
        {
            case PlantCharacterController.Characterstates.GroundedLocomotion:
                bool value = Mathf.Abs(_rb.linearVelocityX) > 0f;
                _animator.SetBool(_running, value);
                _animator.SetBool(_airborne, false);
                _animator.SetBool(_climbing, false);
                break;
            case PlantCharacterController.Characterstates.Airborne:
                _animator.SetBool(_running, false);
                _animator.SetBool(_airborne, true);
                _animator.SetBool(_climbing, false);
                break;
            case PlantCharacterController.Characterstates.Climbing:
                _animator.SetBool(_running, false);
                _animator.SetBool(_airborne, false);
                _animator.SetBool(_climbing, true);
                break;
            case PlantCharacterController.Characterstates.Rooted:
            case PlantCharacterController.Characterstates.Dead:
                break;
        }
    }
}
