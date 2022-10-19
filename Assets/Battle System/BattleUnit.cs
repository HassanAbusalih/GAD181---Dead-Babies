using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public PokemonBase Base;
    public int level;
    public Pokemon pokemon;

    public void Setup()
    {
        pokemon = new Pokemon(Base, level);
        GetComponent<SpriteRenderer>().sprite = pokemon.pokemon.pokeSprite;
    }

}
