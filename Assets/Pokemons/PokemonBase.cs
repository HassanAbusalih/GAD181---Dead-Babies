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

    [SerializeField] public int maxHp;
    [SerializeField] public int attack;
    [SerializeField] public int defense;
    [SerializeField] public int spAttack;
    [SerializeField] public int spDefence;
    [SerializeField] public int speed;

    [SerializeField] public PokemonType type1;
    [SerializeField] public PokemonType type2;
    public List<LearnableMoves> learnableMoves;
}

[System.Serializable]

public class LearnableMoves
{
    public Moves moves;
    public int level;
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
