using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public PokemonParties pokemonParties;
    int pokemonNumber;
    public void Load()
    {
        pokemonParties.playerParty.Clear();
        for (int i = 0; i < PlayerPrefs.GetInt("Party"); i++)
        {
            pokemonNumber = PlayerPrefs.GetInt($"pokemon{i}");
            for (int j = 0; j < pokemonParties.allPokemon.Count; j++)
            {
                if (pokemonNumber == pokemonParties.allPokemon[j].pokemonBase.pokeNumber)
                {
                    pokemonParties.playerParty.Add(pokemonParties.allPokemon[j]);
                    pokemonParties.playerParty[pokemonParties.playerParty.Count - 1].level = PlayerPrefs.GetInt($"pokemonLevel{i}");
                }
            }
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Party", pokemonParties.playerParty.Count);
        for (int i = 0; i < pokemonParties.playerParty.Count; i++)
        {
            PlayerPrefs.SetInt($"pokemon{i}", pokemonParties.playerParty[i].pokemonBase.pokeNumber);
            PlayerPrefs.SetInt($"pokemonLevel{i}", pokemonParties.playerParty[i].level);
        }
    }
}
