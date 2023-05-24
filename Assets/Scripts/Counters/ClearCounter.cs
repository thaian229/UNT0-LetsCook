using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter is clear
            if (player.HasKitchenObject())
            {
                // And player is holding kitchen obj ==> Put down
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // Counter has obj
            if (!player.HasKitchenObject())
            {
                // And player hold nothing ==> let player pick up obj
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                PlateKitchenObject plateKitchenObject;

                if (player.GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    // player hold a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }
}
