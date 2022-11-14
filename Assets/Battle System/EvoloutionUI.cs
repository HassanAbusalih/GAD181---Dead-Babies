using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvoloutionUI : MonoBehaviour
{
    public GameObject evolutionUI;
    public Image pokemonImage;
    public BattleDialogue dialogue;

    public IEnumerator Evolve(Pokemon pokemon)
    {
        evolutionUI.SetActive(true);
        pokemonImage.sprite = pokemon.pokemonBase.pokeSprite;
        yield return dialogue.SetDialogue($"{pokemon.pokemonBase.name} is evolving");
        var oldPokemon = pokemon.pokemonBase.name;
        pokemon.Evolve();
        pokemonImage.sprite = pokemon.pokemonBase.pokeSprite;
        yield return dialogue.SetDialogue($"{oldPokemon} evolved in to {pokemon.pokemonBase.name}");
        evolutionUI.SetActive(false);
    }



}
