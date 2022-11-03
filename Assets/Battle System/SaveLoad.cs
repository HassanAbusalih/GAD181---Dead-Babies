using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public PokemonParties pokemonParties;
    int pokemonNumber;
    int beforeLoad;
    int afterLoad;
    int enemyPokemonNumber;
    public string trainerName;
    Pokemon randomPokemon;
    int enemyLevel;
    public bool isTrainer;
    public void Load()
    {
        if (PlayerPrefs.GetInt("Trainer") == 0)
        {
            isTrainer = false;
        }
        else
        {
            isTrainer = true;
        }
        // Loading player Pokemon.
        beforeLoad = pokemonParties.playerParty.Count;
        for (int i = 0; i < PlayerPrefs.GetInt("party"); i++)
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
        afterLoad = pokemonParties.playerParty.Count - beforeLoad;
        if (afterLoad != 0 && beforeLoad != 0)
        {
            for (int i = 0; i < beforeLoad; i++)
            {
                pokemonParties.playerParty.Remove(pokemonParties.playerParty[i]);
            }
        }
        // Loading enemy Pokemon or random encounter.
        if (PlayerPrefs.GetInt("Trainer") == 0)
        {
            enemyPokemonNumber = PlayerPrefs.GetInt("Encounter");
            for (int i = 0; i < pokemonParties.allPokemon.Count; i++)
            {
                if (enemyPokemonNumber == pokemonParties.allPokemon[i].pokemonBase.pokeNumber)
                {
                    pokemonParties.enemyParty.Add(pokemonParties.allPokemon[i]);
                    pokemonParties.enemyParty[pokemonParties.enemyParty.Count - 1].level = PlayerPrefs.GetInt("EncounterLevel");
                }
            }
        }
        else if (PlayerPrefs.GetInt("Trainer") == 1)
        {
            trainerName = PlayerPrefs.GetString("trainerName");
            for (int i = 0; i < PlayerPrefs.GetInt("enemyParty"); i++)
            {
                enemyPokemonNumber = PlayerPrefs.GetInt($"enemyPokemon{i}");
                for (int j = 0; j < pokemonParties.allPokemon.Count; j++)
                {
                    if (enemyPokemonNumber == pokemonParties.allPokemon[j].pokemonBase.pokeNumber)
                    {
                        pokemonParties.enemyParty.Add(pokemonParties.allPokemon[j]);
                        pokemonParties.enemyParty[pokemonParties.enemyParty.Count - 1].level = PlayerPrefs.GetInt($"enemyPokemonLevel{i}");
                    }
                }
            }
        }
        PlayerPrefs.DeleteAll();
    }

    public void PlayerSave()
    {
        PlayerPrefs.SetInt("party", pokemonParties.playerParty.Count);
        for (int i = 0; i < pokemonParties.playerParty.Count; i++)
        {
            PlayerPrefs.SetInt($"pokemon{i}", pokemonParties.playerParty[i].pokemonBase.pokeNumber);
            PlayerPrefs.SetInt($"pokemonLevel{i}", pokemonParties.playerParty[i].level);
        }
    }

    public void EnemySave()
    {
        if (isTrainer)
        {
            PlayerPrefs.SetInt("Trainer", 1);
            PlayerPrefs.SetInt("enemyParty", pokemonParties.enemyParty.Count);
            for (int i = 0; i < pokemonParties.enemyParty.Count; i++)
            {
                PlayerPrefs.SetInt($"enemyPokemon{i}", pokemonParties.enemyParty[i].pokemonBase.pokeNumber);
                PlayerPrefs.SetInt($"enemyPokemonLevel{i}", pokemonParties.enemyParty[i].level);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Trainer", 0);
            randomPokemon = pokemonParties.allPokemon[Random.Range(0, pokemonParties.allPokemon.Count - 1)];
            pokemonParties.enemyParty.Add(randomPokemon);
            PlayerPrefs.SetInt("Encounter", randomPokemon.pokemonBase.pokeNumber);
            enemyLevel = Random.Range(1, 8);
            PlayerPrefs.SetInt("EncounterLevel", enemyLevel);
        }
    }
}
