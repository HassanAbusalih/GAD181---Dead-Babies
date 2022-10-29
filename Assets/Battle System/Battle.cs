using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    public BattleUnit playerMon;
    public BattleUI playerInfo;
    public BattleUnit enemyMon;
    public BattleUI enemyInfo;
    public BattleDialogue dialogue;
    public PokemonParties pokemonParties;
    public Animator animator;
    public AudioSource audiosource1;
    public AudioSource audiosource2;
    public SaveLoad saveLoad;
    BattleState state;
    int selection;
    int selectionB;
    bool isTrainer;

    // Start is called before the first frame update



    void Start()
    {
        saveLoad.Load();
        StartCoroutine(StartBattle());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleState.PlayerMenu)
        {
            MenuSelection();
            dialogue.UpdateMenuSelection(selectionB);
        }
        else if (state == BattleState.PlayerTurn)
        {
            MoveSelection();
            dialogue.UpdateMoveSelection(selection);
        }
        else if (state == BattleState.PlayerAttack)
        {
            StartCoroutine(Attack());
        }
    }

    public IEnumerator StartBattle()
    {
        state = BattleState.Start;
        InitializePokemon();
        yield return StartCoroutine(SetupPokemon());
        yield return dialogue.SetDialogue("A wild " + enemyMon.pokemon.pokemonBase.pokeName + " appears!");
        state = BattleState.PlayerMenu;
        yield return dialogue.SetDialogue("Select an action.");
    }

    public IEnumerator SetupPokemon()
    {
        yield return null;
        InitializePokemon();
        playerMon.Setup(pokemonParties.playerParty[0]);
        enemyMon.Setup(pokemonParties.enemyParty[0]);
        playerInfo.Setup(playerMon.pokemon);
        enemyInfo.Setup(enemyMon.pokemon);
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
            yield return dialogue.SetDialogue(playerMon.pokemon.pokemonBase.pokeName + " uses " + move.Base.name + "!");
            bool fainted = enemyMon.pokemon.TakeDamage(move);
            enemyInfo.DamageTaken();
            if (fainted)
            {
                pokemonParties.enemyParty.Remove(pokemonParties.enemyParty[0]);
                yield return dialogue.SetDialogue(enemyMon.pokemon.pokemonBase.pokeName + " fainted!");
                if (pokemonParties.enemyParty.Count == 0)
                {
                    state = BattleState.PlayerWin;
                    yield return dialogue.SetDialogue(enemyMon.pokemon.pokemonBase.pokeName + " fainted!");
                    Victory();
                    yield return dialogue.SetDialogue("You win!");
                    saveLoad.PlayerSave();
                    yield return EndBattle();
                }
                else
                {
                    yield return SetupPokemon();
                    yield return dialogue.SetDialogue("Enemy sends out " + enemyMon.pokemon.pokemonBase.pokeName + "!");
                    state = BattleState.EnemyAttack;
                    StartCoroutine(Attack());
                }
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
            yield return dialogue.SetDialogue(enemyMon.pokemon.pokemonBase.pokeName + " uses " + move.Base.name + "!");
            bool fainted = playerMon.pokemon.TakeDamage(move);
            playerInfo.DamageTaken();
            if (fainted)
            {
                yield return dialogue.SetDialogue(playerMon.pokemon.pokemonBase.pokeName + " fainted!");
                pokemonParties.playerParty.Remove(pokemonParties.playerParty[0]);
                PlayerPrefs.DeleteAll();
                saveLoad.PlayerSave();
                if (pokemonParties.playerParty.Count == 0)
                {
                    state = BattleState.EnemyWin;
                    yield return dialogue.SetDialogue("You lose!");
                    yield return EndBattle();
                }
                else
                {
                    yield return SetupPokemon();
                    yield return dialogue.SetDialogue("You send out " + playerMon.pokemon.pokemonBase.pokeName + "!");
                    StartCoroutine(PlayerTurn());
                }
            }
            else
            {
                state = BattleState.PlayerMenu;
                dialogue.menu.SetActive(true);
                StartCoroutine(dialogue.SetDialogue("Select an action."));
            }
        }
    }
    void MenuSelection()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (selectionB < dialogue.menuActions.Count - 1)
            {
                selectionB++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (selectionB > 0)
            {
                selectionB--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (selectionB > 1)
            {
                selectionB -= 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectionB < dialogue.menuActions.Count - 2)
            {
                selectionB += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectionB == 0)
            {
                dialogue.menu.SetActive(false);
                StartCoroutine(PlayerTurn());
            }
            else if(selectionB == 1)
            {
                CatchPokemon();
            }
            else if(selectionB == 2)
            {

            }
            else if(selectionB == 3)
            {

            }
        }
    }
    void CatchPokemon()
    {
        if (isTrainer == true)
        {
            StartCoroutine(dialogue.SetDialogue("You cannot capture a trainer's Pokemon."));
          
        }

        else if (pokemonParties.playerParty.Count < 5)
        {
            
            pokemonParties.playerParty.Add(pokemonParties.enemyParty[0]);
            StartCoroutine(dialogue.SetDialogue("You have captured a " + pokemonParties.playerParty[pokemonParties.playerParty.Count - 1].pokemonBase.name + "!"));
            PlayerPrefs.DeleteAll();
            saveLoad.PlayerSave();
            Victory();
            StartCoroutine(EndBattle());
        }
        else
        {
            StartCoroutine(dialogue.SetDialogue("Your party is full."));
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

    void Victory()
    {
        if (state == BattleState.PlayerWin)
        {
            audiosource2.Stop();
            audiosource1.Play();
        }
    }

    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(2);
        animator.SetBool("End", true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(1.5f);
        asyncOperation.allowSceneActivation = true;

    }

    void InitializePokemon()
    {
        for (int i = 0; i < pokemonParties.playerParty.Count; i++)
        {
            pokemonParties.playerParty[i].MakePokemon();
        }
        for (int i = 0; i < pokemonParties.enemyParty.Count; i++)
        {
            pokemonParties.enemyParty[i].MakePokemon();
        }
    }

}
