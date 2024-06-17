using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public Resource[] inputs; // Входные ресурсы
    public Resource[] outputs; // Выходные ресурсы
}

[System.Serializable]
public class Resource
{
    public Res res;
    public int amount;
}

