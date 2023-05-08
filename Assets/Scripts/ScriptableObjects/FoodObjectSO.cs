using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewFood", menuName = "Food")]
public class FoodObjectSO : ScriptableObject
{
   public Transform prefab;
//    public Sprite sprite;
   public string objectName;
    
}
