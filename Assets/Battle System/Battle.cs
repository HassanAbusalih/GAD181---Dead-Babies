using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattleUnit playerMon;
    public BattleUI playerInfo;
    public BattleUnit enemyMon;
    public BattleUI enemyInfo;
    public BattleDialogue dialogue;
    public Animator animator;
    public AudioSource audiosource1;
    public AudioSource audiosource2;
   
    BattleState state;
    int selection;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupBattle());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleState.PlayerTurn)
        {
            MoveSelection();
            dialogue.UpdateMoveSelection(selection);
        }
        else if (state == BattleState.PlayerAttack)
        {
            StartCoroutine(Attack());
        }
    }

    public IEnumerator SetupBattle()
    {
        state = BattleState.Start;
        playerMon.Setup();
        enemyMon.Setup();
        playerInfo.Setup(playerMon.pokemon);
        enemyInfo.Setup(enemyMon.pokemon);
        yield return dialogue.SetDialogue("A wild " + enemyMon.pokemon.pokemon.pokeName + " appears!");
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        yield return dialogue.SetDialogue("Choose a move.");
        state = BattleState.PlayerTurn;
        dialogue.dialoguetext.enabled = false;
        dialogue.Attacks.SetActive(true);
        dialogue.SetMoves(playerMon.pokemon.pMoves);

    }

    IEnumerator Attack()
    {
        if (state == BattleState.PlayerAttack)
        {
            state = BattleState.Busy;
            Move move = playerMon.pokemon.pMoves[selection];
            yield return dialogue.SetDialogue(playerMon.pokemon.pokemon.pokeName + " uses " + move.Base.name + " !");
            bool fainted = enemyMon.pokemon.TakeDamage(move);
            enemyInfo.DamageTaken();
            if (fainted)
            {
                yield return dialogue.SetDialogue(enemyMon.pokemon.pokemon.pokeName + " fainted!");
                yield return dialogue.SetDialogue("You win!");
                state = BattleState.PlayerWin;
                yield return EndAnimation();
                yield return new WaitForSeconds(2);

            }
            else
            {
                state = BattleState.EnemyAttack;
                StartCoroutine(Attack());
            }
        }
        else if (state == BattleState.EnemyAttack)
        {
            state = BattleState.Busy;
            Move move = enemyMon.pokemon.RandomMove();
            yield return dialogue.SetDialogue(enemyMon.pokemon.pokemon.pokeName + " uses " + move.Base.name + " !");
            bool fainted = playerMon.pokemon.TakeDamage(move);
            playerInfo.DamageTaken();
            if (fainted)
            {
                yield return dialogue.SetDialogue(playerMon.pokemon.pokemon.pokeName + " fainted!");
                yield return dialogue.SetDialogue("You Lose");
                state = BattleState.EnemyWin;
                yield return EndAnimation();
                yield return new WaitForSeconds(2);
            }
            else
            {
                StartCoroutine(PlayerTurn());
            }
        }
    }

    void MoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (selection < playerMon.pokemon.pMoves.Count - 1)
            {
                selection++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (selection > 0)
            {
                selection--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (selection > 1)
            {
                selection -= 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selection < playerMon.pokemon.pMoves.Count - 2)
            {
                selection += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            state = BattleState.PlayerAttack;
        }
    }

    IEnumerator EndAnimation()
    {
        
        if (state == BattleState.PlayerWin)
        {
            audiosource2.Stop();
            audiosource1.Play(); 
        }
        yield return new WaitForSeconds(5);
        animator.SetBool("End", true);

    }

    

}
