using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonLevel;
    public TextMeshProUGUI pokemonHP;
    public Image hpBar;
    Pokemon newPokemon;
    public HP health;
    public XpBar xp;

    public void Setup(Pokemon pokemon)
    {
        newPokemon = pokemon;
        pokemonName.text = pokemon.pokemonBase.pokeName;
        pokemonLevel.text = "Lvl   " + pokemon.level;
        pokemonHP.text = pokemon.currentHP + " / " + pokemon.totalHP;
        health.SetHealth(pokemon.currentHP, pokemon.totalHP);
        //xp.SetXpBar(pokemon.currentXpPoints, pokemon.xpThreshhold);
    }

    public void DamageTaken()
    {
        health.SetHealth(newPokemon.currentHP, newPokemon.totalHP);
        pokemonHP.text = ((int)newPokemon.currentHP) + " / " + ((int)newPokemon.totalHP);
    }
}
