using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonLevel;

    public void Setup(Pokemon pokemon)
    {
        pokemonName.text = pokemon.pokemon.pokeName;
        pokemonLevel.text = "Level" + pokemon.level;
    }
}
