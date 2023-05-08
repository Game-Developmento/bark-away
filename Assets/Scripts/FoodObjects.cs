using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObjects : MonoBehaviour
{
    [SerializeField] private FoodListSO foodList;

    public FoodListSO GetFoodListSO() {
        return foodList;
    }
}
