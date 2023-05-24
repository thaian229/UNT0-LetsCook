using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float burningTimerMax;
}
