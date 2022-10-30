using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialoguetext;
    public GameObject attacks;
    public GameObject menu;
    public GameObject pokemonList;
    public GameObject selectionBox;
    public List<TextMeshProUGUI> playerPokemon;
    public List<TextMeshProUGUI> pokeMoves;
    public List<TextMeshProUGUI> menuActions;

    public IEnumerator SetDialogue(string dialogue)
    {
        attacks.SetActive(false);
        dialoguetext.enabled = true;
        dialoguetext.text = dialogue;
        yield return new WaitForSeconds(2f);
    }
    
    public void SetMoves(List<Move> Moves)
    {
        for (int i = 0; i < pokeMoves.Count; i++)
        {
            if (i < Moves.Count)
                pokeMoves[i].text = Moves[i].Base.movename;
            else
                pokeMoves[i].text = "-";
        }

    }

    public void UpdateMenuSelection(int selection, List<TextMeshProUGUI> menu)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (selection == i)
            {
                menu[i].color = Color.blue;
            }
            else menu[i].color = Color.black;
        } 
    }
    
    public void SetPokemonNames(List<Pokemon> pokemons)
    {
        for (int i = 0; i < playerPokemon.Count; i++)
        {
            if (i < pokemons.Count)
                playerPokemon[i].text = pokemons[i].pokemonBase.pokeName;
            else
                playerPokemon[i].text = "-";
        }

    }
}




public enum BattleState
{
    Start,
    PlayerMenu,
    PokemonSelection,
    PlayerTurn,
    PlayerAttack,
    EnemyAttack,
    Busy,
    PlayerWin,
    EnemyWin
}
