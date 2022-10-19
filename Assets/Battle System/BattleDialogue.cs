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
        yield return new WaitForSeconds(1f);
    }
    
    public void SetMoves(List<Move> Moves)
    {
        for (int i = 0; i < PokeMoves.Count; i++)
        {
            if (i < Moves.Count)
                Moves[i].Base.movename = PokeMoves[i].text;
            else
                PokeMoves[i].text = "-";
        }

    }
}



public enum BattleState
{
    Start,
    PlayerTurn,
    PlayerAttack,
    EnemyAttack,
    Busy
}
