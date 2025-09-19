using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Parâmetros do Animator
    private const string VELOCITY_X = "velocityX";
    private const string VELOCITY_Y = "velocityY";
    private const string ATTACK = "isAttacking"; // pode virar Trigger se preferir

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
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

    /// <summary>
    /// Ativa ataque (usando bool)
    /// </summary>
    public void TriggerAttack()
    {
        animator.SetTrigger(ATTACK);
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
    //    public void EndAttack()
    //    {
    //        animator.SetBool(ATTACK, false);
    //    }
}
