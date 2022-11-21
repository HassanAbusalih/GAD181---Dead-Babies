using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Trainers : ScriptableObject
{
    public string trainerName;
    public Sprite trainerSprite;
    public List<Pokemon> trainerPokemon;
}
