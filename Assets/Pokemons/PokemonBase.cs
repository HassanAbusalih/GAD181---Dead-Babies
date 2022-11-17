using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class PokemonBase : ScriptableObject
{
    [SerializeField] public string pokeName;
    [SerializeField] public string description;
    [SerializeField] public Sprite pokeSprite;
    [SerializeField] public Sprite backSprite;
    [SerializeField] public int maxHp;
    [SerializeField] public int attack;
    [SerializeField] public int defense;
    [SerializeField] public int spAttack;
    [SerializeField] public int spDefence;
    [SerializeField] public int speed;
    [SerializeField] public PokemonType type1;
    [SerializeField] public PokemonType type2;
    public int pokeNumber;
    public List<LearnableMoves> learnableMoves;
    public Evolution evolutions;
    public string fusionName;
}

[System.Serializable]

public class LearnableMoves
{
    public Moves moves;
    public int level;
}

[System.Serializable]

public class Evolution
{
    public PokemonBase evolveTo;
    public int levelForEvolve;
}
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Fighting,
    Bug,
    Rock,
    Ice,
    Poison,
    Ground,
    Flying,
    Psychic,    
    Steel,
    Ghost,
    Dragon
}

public class PokemonTypeChart
{
    static float[][] TypeChart =                                 
                                                                        // Defense(column)
    {                    
                        // Attack(Row)       Normal      Fire      Water       Electric       Grass      FLying      Bug       Rock     Steel    Ice          Poison      Ground       Fighting      Psychic      Ghost      Dragon
                       /*Normal*/new float[]{1.0f,       1.0f,     1.0f,       1.0f,          1.0f,       1.0f,      1.0f,     0.5f,    0.5f,    1.0f,        1.0f,        1.0f,          1.0f,        1.0f,       0.0f,      1.0f},
                      /*Fire*/   new float[]{1.0f,       0.5f,     0.5f,       1.0f,          2.0f,       1.0f,      2.0f,     0.5f,    2.0f,    2.0f,        1.0f,        1.0f,          1.0f,        1.0f,       1.0f,      0.5f},
                     /*Water*/   new float[]{1.0f,       2.0f,     0.5f,       1.0f,          0.5f,       1.0f,      1.0f,     2.0f,    1.0f,    1.0f,        1.0f,        2.0f,          1.0f,        1.0f,       1.0f,      0.5f},
                    /*Electric*/ new float[]{1.0f,       1.0f,     2.0f,       0.5f,          0.5f,       2.0f,      1.0f,     1.0f,    1.0f,    1.0f,        1.0f,        0.0f,          1.0f,        1.0f,       1.0f,      0.5f},
                   /*Grass*/     new float[]{1.0f,       0.5f,     2.0f,       1.0f,          0.5f,       0.5f,      0.5f,     2.0f,    0.5f,    1.0f,        0.5f,        2.0f,          1.0f,        1.0f,       1.0f,      0.5f},
           /*Fighting*/          new float[]{2.0f,       1.0f,     1.0f,       1.0f,          1.0f,       0.5f,      0.5f,     2.0f,    0.5f,    2.0f,        0.5f,        1.0f,          1.0f,        0.5f,       0.0f,      1.0f},
                 /*Bug*/         new float[]{1.0f,       0.5f,     1.0f,       1.0f,          2.0f,       0.5f,      1.0f,     1.0f,    0.5f,    1.0f,        0.5f,        1.0f,          0.5f,        2.0f,       0.5f,      1.0f},
                /*Rock*/         new float[]{1.0f,       2.0f,     1.0f,       1.0f,          1.0f,       2.0f,      2.0f,     2.0f,    0.5f,    2.0f,        1.0f,        0.5f,          0.5f,        1.0f,       1.0f,      1.0f},
              /*Ice*/            new float[]{1.0f,       0.5f,     0.5f,       1.0f,          2.0f,       2.0f,      1.0f,     1.0f,    0.5f,    0.5f,        1.0f,        2.0f,          1.0f,        1.0f,       1.0f,      2.0f},
             /*Poison*/          new float[]{1.0f,       1.0f,     1.0f,       1.0f,          2.0f,       1.0f,      1.0f,     0.5f,    0.5f,    1.0f,        0.5f,        0.5f,          1.0f,        1.0f,       0.5f,      1.0f},
            /*Ground*/           new float[]{1.0f,       2.0f,     1.0f,       2.0f,          0.5f,       0.0f,      0.5f,     2.0f,    0.5f,    1.0f,        2.0f,        1.0f,          1.0f,        1.0f,       1.0f,      1.0f},
                  /*Flying*/     new float[]{1.0f,       1.0f,     1.0f,       0.5f,          2.0f,       1.0f,      2.0f,     0.5f,    0.5f,    1.0f,        1.0f,        1.0f,          2.0f,        1.0f,       1.0f,      1.0f},
          /*Psychic*/            new float[]{1.0f,       1.0f,     1.0f,       1.0f,          1.0f,       1.0f,      1.0f,     1.0f,    0.5f,    1.0f,        2.0f,        1.0f,          2.0f,        0.5f,       1.0f,      1.0f},
               /*Steel*/         new float[]{1.0f,       0.5f,     0.5f,       0.5f,          1.0f,       1.0f,      1.0f,     2.0f,    0.5f,    2.0f,        1.0f,        1.0f,          1.0f,        1.0f,       1.0f,      1.0f},
         /*Ghost*/               new float[]{0.0f,       1.0f,     1.0f,       1.0f,          1.0f,       1.0f,      1.0f,     1.0f,    0.5f,    1.0f,        1.0f,        1.0f,          1.0f,        2.0f,       2.0f,      1.0f},
        /*Dragon*/               new float[]{1.0f,       1.0f,     1.0f,       1.0f,          1.0f,       1.0f,      1.0f,     1.0f,    1.0f,    1.0f,        1.0f,        1.0f,          1.0f,        1.0f,       1.0f,      2.0f},
    };
    
    public static float GetDamageEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if(attackType == PokemonType.None || defenseType == PokemonType.None)
        {
            return 1;
        }
        int row = (int)attackType - 1; 
        int column = (int)defenseType - 1;
        {
            return TypeChart[row][column];
        }
    }
}


