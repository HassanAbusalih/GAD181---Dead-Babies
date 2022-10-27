using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public Pokemon pokemon;

    public void Setup(Pokemon pokemonBase)
    {
        pokemon = pokemonBase;
        GetComponent<SpriteRenderer>().sprite = pokemon.pokemonBase.pokeSprite;
    }


}
