using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Parâmetros do Animator
    private const string VELOCITY_X = "velocityX";
    private const string VELOCITY_Y = "velocityY";
    private const string JUMP = "isJumping";
    private const string ATTACK = "isAttacking"; // pode virar Trigger se preferir
    private const string ATTACKB = "isAttacking_O";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Atualiza a velocidade horizontal (walk)
    /// </summary>
    public void SetWalkSpeed(float speedX)
    {
        animator.SetFloat(VELOCITY_X, speedX);
    }

    /// <summary>
    /// Atualiza a velocidade vertical (jump/fall)
    /// </summary>
    public void SetJumpVelocity(float speedY)
    {
        animator.SetFloat(VELOCITY_Y, speedY);
    }

    public void SetJump(bool boolean)
    {
        animator.SetBool(JUMP, boolean);
    }

    /// <summary>
    /// Ativa ataque (usando bool)
    /// </summary>
    public void TriggerAttack()
    {
        //animator.SetTrigger(ATTACK);
        animator.SetBool(ATTACKB, true);
    }

    /// <summary>
    /// Trigger de dano
    /// </summary>
    public void TriggerHit()
    {

    }

    /// <summary>
    /// Trigger da morte
    /// </summary>
    public void TriggerDeath()
    {

    }

    /// <summary>
    /// Finaliza ataque (AnimationEvent no fim da animação)
    /// </summary>
    public void EndAttack()
    {
        animator.SetBool(ATTACKB, false);
    }
}
