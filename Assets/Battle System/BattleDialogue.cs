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
    public GameObject info;
    public float timer;
    bool allow;
    public List<TextMeshProUGUI> playerPokemon;
    public List<TextMeshProUGUI> pokeMoves;
    public List<TextMeshProUGUI> menuActions;
    public List<TextMeshProUGUI> moveInfo;

    private void Update()
    {
        if (timer < 0.1)
        {
            allow = false;
        }
        else
        {
            allow = true;
        }
        if (dialoguetext.enabled)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && allow)
        {
            timer += 2;
        }
    }
    public IEnumerator SetDialogue(string dialogue)
    {
        timer = 0;
        attacks.SetActive(false);
        dialoguetext.enabled = true;
        dialoguetext.text = dialogue;
        yield return new WaitUntil(() => timer >= 2);
        timer = 0;
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

    public void UpdateMoveSelection(int selection, List<TextMeshProUGUI> menu, Move move)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (selection == i)
            {
                menu[i].color = Color.blue;
                if ((float)move.powerpoints/ (float)move.Base.powerpoints > 0.7f)
                {
                    moveInfo[0].text = $"PP:  {move.powerpoints} / {move.maxPP}";
                }
                else if ((float)move.powerpoints / (float)move.Base.powerpoints > 0.4f)
                {
                    moveInfo[0].text = $"PP:  <color=yellow>{move.powerpoints}</color> / {move.maxPP}";
                }
                else if ((float)move.powerpoints / (float)move.Base.powerpoints <= 0.4f)
                {
                    moveInfo[0].text = $"PP:  <color=red>{move.powerpoints}</color> / {move.maxPP}";
                }
                moveInfo[1].text = $"Type:  <color=#{ColorUtility.ToHtmlStringRGB(move.Base.movescolor)}>{move.Base.type}</color>";
                moveInfo[2].text = $"Power:  {move.power}";
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
    MoveSelection,
    PlayerAttack,
    EnemyAttack,
    Busy,
    PlayerWin,
    EnemyWin
}
