using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private const string IS_RUNNING = "IsRunning";
    private const string ATTACK = "Attack";
    [SerializeField] private Sword sword;
    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        sword.OnSwordAttacked += Sword_AttackAnimate;
        Flip();
    }

    private void Sword_AttackAnimate(object sender, EventArgs e)
    {
        animator.SetTrigger(ATTACK);
    }

    private void Flip() {
        float moveX =  GameInput.Instance.GetMovementVector().x;
        
        if (moveX > 0) {
            spriteRenderer.flipX = false; 
        } else if (moveX < 0) {
            spriteRenderer.flipX = true;  
        }
    }

    public void TriggerEndAttackAnimation()
    {
        sword.AttackColliderOff();
    }
}
