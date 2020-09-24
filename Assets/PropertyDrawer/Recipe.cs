using System;
using UnityEngine;

public enum IngredientUnit { Spoon, Cup, Bowl, Piece }

// 自定义 Serializable 类
[Serializable]
public class Ingredient
{
    public string name;
    public int amount = 1;
    public IngredientUnit unit;
}

public class Recipe : MonoBehaviour
{
    public Ingredient potionResult;
    public Ingredient[] potionIngredients;
}