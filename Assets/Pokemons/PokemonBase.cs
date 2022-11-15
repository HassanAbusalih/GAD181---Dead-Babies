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
    public int xpYield;
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
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Steel,
    Ghost,
    Dragon
}
