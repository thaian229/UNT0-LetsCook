using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footstepTimer = 0f;
    private float footstepInterval = .1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        footstepTimer = footstepInterval;
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            footstepTimer = footstepInterval;

            if (player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootstepSound(player.transform.position, volume);
            }
        }
    }
}
