using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "New Move")]
public class Moves : ScriptableObject
{
     public string movename;
     public string description;
     public PokemonType type;
     public MoveType category;
     public int power;
     public int accuracy;
     public int powerpoints;



    public enum MoveType
    {
        None,
        Status,
        Special,
        Physical
    }

   
}


