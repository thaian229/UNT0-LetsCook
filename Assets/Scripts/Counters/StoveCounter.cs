using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State currentState;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        currentState = State.Idle;
        fryingTimer = 0f;
        burningTimer = 0f;
    }

    private void Update()
    {
        if (!HasKitchenObject()) return;

        switch (currentState)
        {
            case State.Idle:
                break;

            case State.Frying:
                fryingTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                });

                if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                {
                    // fried
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                    currentState = State.Fried;
                    burningTimer = 0f;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = currentState,
                    });
                }
                break;

            case State.Fried:
                burningTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                });

                if (burningTimer >= burningRecipeSO.burningTimerMax)
                {
                    // fried
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                    currentState = State.Burned;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = currentState,
                    });
                }
                break;

            case State.Burned:
                break;

            default:
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter is clear
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // And player is holding kitchen obj ==> Put down
                player.GetKitchenObject().SetKitchenObjectParent(this);

                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                currentState = State.Frying;
                fryingTimer = 0f;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = currentState,
                });
            }
            else
            {
                // Counter has obj
                if (!player.HasKitchenObject())
                {
                    // And player hold nothing ==> let player pick up obj
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }
        else
        {
            // Counter has obj
            if (!player.HasKitchenObject())
            {
                // And player hold nothing ==> let player pick up obj
                GetKitchenObject().SetKitchenObjectParent(player);

                currentState = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = currentState,
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO recipeInput)
    {
        FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOWithInput(recipeInput);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputFromInputRecipe(KitchenObjectSO recipeInput)
    {
        FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOWithInput(recipeInput);

        if (cuttingRecipeSO == null) return null;
        return cuttingRecipeSO.output;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO recipeInput)
    {
        foreach (FryingRecipeSO recipe in fryingRecipeSOArray)
        {
            if (recipe.input == recipeInput)
            {
                return recipe;
            }
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO recipeInput)
    {
        foreach (BurningRecipeSO recipe in burningRecipeSOArray)
        {
            if (recipe.input == recipeInput)
            {
                return recipe;
            }
        }

        return null;
    }
}
