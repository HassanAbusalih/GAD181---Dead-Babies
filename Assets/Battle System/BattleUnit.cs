using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public Pokemon pokemon;
    public bool bsprite;

    public void Setup(Pokemon pokemonBase)
    {
        pokemon = pokemonBase;
        

        if(bsprite)
        {
            GetComponent<SpriteRenderer>().sprite = pokemon.pokemonBase.backSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = pokemon.pokemonBase.pokeSprite;
        }
    }


}
