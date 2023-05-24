using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private float spawnPlateTimerInterval = 4f;
    [SerializeField] private int platesSpawnAmountMax = 3;

    private float spawnPlateTimer;
    private int platesSpawnedAmount;

    private void Start()
    {
        spawnPlateTimer = 0f;
    }

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer >= spawnPlateTimerInterval)
        {
            spawnPlateTimer = 0f;

            // Spawn plate
            if (platesSpawnedAmount < platesSpawnAmountMax)
            {
                platesSpawnedAmount++;
                
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
