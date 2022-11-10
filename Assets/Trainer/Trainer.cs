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
        if (collision.gameObject.CompareTag("Player") && !trainerBase.battled)
        {
            Debug.Log("I can see you");
            pokemonParties.enemyParty = trainerBase.trainerPokemon;
            PlayerPrefs.SetString("trainerName", trainerBase.name);
            saveLoad.isTrainer = true;
            trainerBase.battled = true;
        }
    }

}
