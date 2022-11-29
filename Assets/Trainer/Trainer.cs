using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public Trainers trainerBase;
    public PokemonParties pokemonParties;
    public SaveLoad saveLoad;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerPrefs.GetInt($"{trainerBase.trainerName}") != 1)
        {
            PlayerPrefs.SetInt($"{trainerBase.trainerName}", 1);
            pokemonParties.enemyParty = trainerBase.trainerPokemon;
            PlayerPrefs.SetString("trainerName", trainerBase.trainerName);
            saveLoad.isTrainer = true;
        }
    }

}
