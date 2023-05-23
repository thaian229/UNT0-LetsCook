using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator != null && player != null)
        {
            animator.SetBool(IS_WALKING, player.IsWalking());
        }
    }

    private void Update()
    {
        if (animator != null && player != null)
        {
            animator.SetBool(IS_WALKING, player.IsWalking());
        }
    }
}
