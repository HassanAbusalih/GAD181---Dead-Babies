using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string pokeName;
    [SerializeField] string description;

    [SerializeField] Sprite pokeSprite;

    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefence;
    [SerializeField] int speed;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

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
