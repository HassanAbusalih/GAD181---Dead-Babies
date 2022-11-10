using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public PokemonParties pokemonParties;
    Pokemon randomPokemon;
    int pokemonNumber;
    int enemyPokemonNumber;
    int enemyLevel;
    int beforeLoad;
    int afterLoad;
    public string trainerName;
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
                    Pokemon pokemon = new Pokemon(pokemonParties.allPokemon[j].pokemonBase, PlayerPrefs.GetInt($"pokemonLevel{i}"));
                    pokemonParties.playerParty.Add(pokemon);
                    PlayerPrefs.DeleteKey($"pokemon{i}");
                    PlayerPrefs.DeleteKey($"pokemonLevel{i}");
                }
            }
        }
        PlayerPrefs.DeleteKey("party");
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
                    Pokemon pokemon = new Pokemon(pokemonParties.allPokemon[i].pokemonBase, PlayerPrefs.GetInt($"EncounterLevel"));
                    pokemonParties.enemyParty.Add(pokemon);
                }
            }
            PlayerPrefs.DeleteKey("Encounter");
            PlayerPrefs.DeleteKey("EncounterLevel");
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
                        Pokemon pokemon = new Pokemon(pokemonParties.allPokemon[j].pokemonBase, PlayerPrefs.GetInt($"enemyPokemonLevel{i}"));
                        pokemonParties.enemyParty.Add(pokemon);
                        PlayerPrefs.DeleteKey($"enemyPokemon{i}");
                        PlayerPrefs.DeleteKey($"enemyPokemonLevel{i}");
                    }
                }
            }
            PlayerPrefs.DeleteKey("enemyParty");
            PlayerPrefs.DeleteKey("trainerName");
        }
        PlayerPrefs.DeleteKey("Trainer");
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
