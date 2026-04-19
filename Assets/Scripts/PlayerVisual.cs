using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private const string IS_RUNNING = "IsRunning";

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        Flip();
    }
    
    private void Flip() {
        float moveX =  GameInput.Instance.GetMovementVector().x;
        
        if (moveX > 0) {
            spriteRenderer.flipX = false; 
        } else if (moveX < 0) {
            spriteRenderer.flipX = true;  
        }
    }
}
