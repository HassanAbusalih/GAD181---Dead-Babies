using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialoguetext;
    public GameObject Attacks;
    public List<TextMeshProUGUI> PokeMoves;

    public IEnumerator SetDialogue(string dialogue)
    {
        Attacks.SetActive(false);
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
}




public enum BattleState
{
    Start,
    PlayerTurn,
    PlayerAttack,
    EnemyAttack,
    Busy,
    PlayerWin,
    EnemyWin
}
