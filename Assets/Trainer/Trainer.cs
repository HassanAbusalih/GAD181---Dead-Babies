using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public Trainers trainerBase;
    public PokemonParties pokemonParties;
    public SaveLoad saveLoad;
    public bool tutorial;
    public bool flag = true;

    private void Start()
    {
        if (GetComponent<TutorialText>() != null)
        {
            tutorial = true;
        }
    }

    private void Update()
    {
        if (GetComponent<TutorialText>() != null)
        {
            flag = GetComponent<TutorialText>().tutorialBattle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TutorialBattle());
        }
    }

    IEnumerator TutorialBattle()
    {
        if (tutorial && PlayerPrefs.GetInt($"{trainerBase.trainerName}") != 1)
        {
            yield return null;
        }
        else if (!tutorial && PlayerPrefs.GetInt($"{trainerBase.trainerName}") != 1)
        {
            PlayerPrefs.SetInt($"{trainerBase.trainerName}", 1);
            pokemonParties.enemyParty = trainerBase.trainerPokemon;
            PlayerPrefs.SetString("trainerName", trainerBase.trainerName);
            saveLoad.isTrainer = true;
        }
    }

    public void StartTutorial()
    {
        PlayerPrefs.SetInt($"{trainerBase.trainerName}", 1);
        pokemonParties.enemyParty = trainerBase.trainerPokemon;
        PlayerPrefs.SetString("trainerName", trainerBase.trainerName);
        PlayerPrefs.SetInt("TutorialBattle", 1);
        saveLoad.isTrainer = true;
    }
}
