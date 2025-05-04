using UnityEngine;
using UnityEngine.InputSystem;

public class InputSource : CharacterInputs.ICharacterMovementActions
{
    private CharacterInputs _inputActions;
    private PlantCharacterController _characterController;

    public bool IsValid => _inputActions != null;
    public bool IsBound => _characterController != null;
    public bool IsEnabled => IsValid && _inputActions.CharacterMovement.enabled;

    public void BindCharacterController(PlantCharacterController characterController)
    {
        _characterController = characterController;
    }

    public void UnbindCharacterController()
    {
        _characterController = null;
    }

    public void Enable()
    {
        // lazy initialization. Setting the callbacks in the constructor is a bit ugly, so there is that.
        if (!this.IsValid)
        {
            _inputActions = new CharacterInputs();
            _inputActions.CharacterMovement.SetCallbacks(this);
        }
        _inputActions.Enable();
        _inputActions.CharacterMovement.Enable();
    }

    public void Disable()
    {
        if (IsValid && !IsEnabled) return;
        _inputActions.CharacterMovement.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!IsBound) return;
        _characterController.SetMovementInput(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsBound) return;
        if (context.ReadValueAsButton())
        {
            _characterController.PressJump();
        }
    }

    public void OnRoot(InputAction.CallbackContext context)
    {
        if (!IsBound) return;
        if (context.ReadValueAsButton())
        {
            _characterController.PressRoot();
        }
    }
}
