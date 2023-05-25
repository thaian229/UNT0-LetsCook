using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float spawnRecipeInterval = 4f;
    [SerializeField] private float spawnRecipeCapacity = 4;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;

    private int successfulRecipesAmount = 0;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        spawnRecipeTimer = spawnRecipeInterval / 2f;
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeInterval;
            if (waitingRecipeSOList.Count < spawnRecipeCapacity)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plate)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipe = waitingRecipeSOList[i];

            if (!(waitingRecipe.kitchenObjectSOList.Count == plate.GetKitchenObjectSOList().Count)) continue;

            bool plateContentMatchesRecipe = true;
            foreach (KitchenObjectSO recipeKitchenObjSO in waitingRecipe.kitchenObjectSOList)
            {
                bool ingredientFound = false;
                foreach (KitchenObjectSO plateKitchenObjSO in plate.GetKitchenObjectSOList())
                {
                    if (plateKitchenObjSO == recipeKitchenObjSO)
                    {
                        ingredientFound = true;
                        break;
                    }
                }
                if (!ingredientFound)
                {
                    plateContentMatchesRecipe = false;
                    break;
                }
            }

            if (plateContentMatchesRecipe)
            {
                // player deliver correct order
                waitingRecipeSOList.RemoveAt(i);

                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                successfulRecipesAmount++;

                return;
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
