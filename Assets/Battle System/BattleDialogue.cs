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
    public List<TextMeshProUGUI> PokeMoves;
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
        for (int i = 0; i < PokeMoves.Count; i++)
        {
            if (i < Moves.Count)
                PokeMoves[i].text = Moves[i].Base.movename;
            else
                PokeMoves[i].text = "-";
        }

    }

    public void UpdateMoveSelection(int selection)
    {
        for (int i = 0; i < PokeMoves.Count; i++)
        {
            if (selection == i)
            {
                PokeMoves[i].color = Color.blue;
            }
            else PokeMoves[i].color = Color.black;
        } 
    }
    public void UpdateMenuSelection(int selection)
    {
        for (int i = 0; i < menuActions.Count; i++)
        {
            if (selection == i)
            {
                menuActions[i].color = Color.blue;
            }
            else menuActions[i].color = Color.black;
        } 
    }

    public void UpdatePokemonSelection(int selection)
    {
        for (int i = 0; i < playerPokemon.Count; i++)
        {
            if (selection == i)
            {
                playerPokemon[i].color = Color.blue;
            }
            else playerPokemon[i].color = Color.black;
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
