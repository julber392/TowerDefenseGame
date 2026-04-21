using System;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;
    public event EventHandler OnPlayerAttack;

    private void Awake() {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Combat.Attack.started += PlayerAttack;
        
    }
    void PlayerAttack(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this,EventArgs.Empty);
    }
    public Vector2 GetMovementVector() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
}