using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drink", menuName = "Drink")]
public class ScriptableDrinks : ScriptableObject
{

    public string drinkName;
    public int life;
    public float ammos;
    public Sprite artwork;

   
}
