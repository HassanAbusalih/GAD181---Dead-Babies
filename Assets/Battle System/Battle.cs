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
    public Animator captureanimation;
    public Animator capturefailanimation;
    public SpriteRenderer enemypokemon;
    public XpBar xpBar;
    Pokemon switchIn;
    BattleState state;
    int selection;
    int selectionB;
    int selectionC;
    bool deadPokemon;
    int xpGain;

    // Start is called before the first frame update

    void Start()
    {
        saveLoad.Load();
        for (int i = 0; i < pokemonParties.playerParty.Count; i++)
        {
            pokemonParties.playerParty[i].MakePokemon();
        }
        for (int i = 0; i < pokemonParties.enemyParty.Count; i++)
        {
            pokemonParties.enemyParty[i].MakePokemon();
        }
        StartCoroutine(StartBattle());
    }

    // Update is called once per frame
    void Update()
    {
        xpBar.SetXpBar(playerMon.pokemon.currentXpPoints, playerMon.pokemon.xpThreshhold);
        if (state == BattleState.PokemonSelection)
        {
            PokemonSelection();
            dialogue.UpdateMenuSelection(selectionC, dialogue.playerPokemon);
        }
        if (state == BattleState.PlayerMenu)
        {
            MenuSelection();
            dialogue.UpdateMenuSelection(selectionB, dialogue.menuActions);
        }
        else if (state == BattleState.PlayerTurn)
        {
            MoveSelection();
            dialogue.UpdateMenuSelection(selection, dialogue.pokeMoves);
        }
        else if (state == BattleState.PlayerAttack)
        {
            StartCoroutine(Attack());
        }
    }

    public IEnumerator StartBattle()
    {
        state = BattleState.Start;
        dialogue.SetPokemonNames(pokemonParties.playerParty);
        InitializePokemon();
        yield return new WaitForSeconds(0.1f);
        yield return dialogue.SetDialogue("A wild " + enemyMon.pokemon.pokemonBase.pokeName + " appears!");
        state = BattleState.PlayerMenu;
        yield return dialogue.SetDialogue("Select an action.");
    }

    IEnumerator PlayerTurn()
    {
        state = BattleState.Busy;
        yield return dialogue.SetDialogue("Choose a move.");
        state = BattleState.PlayerTurn;
        dialogue.dialoguetext.enabled = false;
        dialogue.attacks.SetActive(true);
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
            selection = 0;
            if (fainted)
            {
                int expYield = enemyMon.pokemon.pokemonBase.xpYield;
                int enemyLevel = enemyMon.pokemon.level;
                xpGain = Mathf.FloorToInt((expYield * enemyLevel) / 7);
                playerMon.pokemon.currentXpPoints += xpGain;
                yield return dialogue.SetDialogue(enemyMon.pokemon.pokemonBase.pokeName + " fainted!");
                yield return dialogue.SetDialogue(playerMon.pokemon.pokemonBase.pokeName + " Recieved " + xpGain + " XP");
                while (playerMon.pokemon.currentXpPoints >= playerMon.pokemon.xpThreshhold)
                {
                    playerMon.pokemon.level++;
                    playerMon.pokemon.currentXpPoints -= playerMon.pokemon.xpThreshhold;
                    playerMon.pokemon.StatsIncrease();
                    playerMon.pokemon.xpThreshhold = playerMon.pokemon.XpToNextLevel(playerMon.pokemon.level);
                    yield return StartCoroutine(dialogue.SetDialogue("You Leveld up to lvl   " + playerMon.pokemon.level));
                }
                pokemonParties.enemyParty.Remove(pokemonParties.enemyParty[0]);
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
                    InitializePokemon();
                    yield return dialogue.SetDialogue($"{saveLoad.trainerName} sends out {enemyMon.pokemon.pokemonBase.pokeName}!");
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
                    dialogue.SetPokemonNames(pokemonParties.playerParty);
                    dialogue.pokemonList.SetActive(true);
                    dialogue.selectionBox.SetActive(true);
                    deadPokemon = true;
                    state = BattleState.PokemonSelection;
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
    IEnumerator CatchPokemon()
    {
        state = BattleState.Busy;
        if (saveLoad.isTrainer == true)
        {
            StartCoroutine(dialogue.SetDialogue("You cannot capture a trainer's Pokemon."));
        }
        else if (pokemonParties.playerParty.Count < 6)
        {
            captureanimation.SetBool("Capture", true);
            yield return new WaitForSeconds(1);
            enemypokemon.GetComponent<SpriteRenderer>().enabled = false;
            if (Random.Range(0, 10) <= 8)
            {
                state = BattleState.PlayerWin;
                pokemonParties.playerParty.Add(pokemonParties.enemyParty[0]);
                Victory();
                StartCoroutine(dialogue.SetDialogue("You have captured a " + pokemonParties.enemyParty[0].pokemonBase.pokeName + "!"));
                PlayerPrefs.DeleteAll();
                saveLoad.PlayerSave();
                StartCoroutine(EndBattle());
            }
            else
            {
                captureanimation.SetBool("Capture", false);
                capturefailanimation.SetBool("capturefail", true);
                yield return new WaitForSeconds(2.5f);
                enemypokemon.GetComponent<SpriteRenderer>().enabled = true;
                StartCoroutine(dialogue.SetDialogue("The Force is strong with him"));
                yield return new WaitForSeconds(0.1f);
                capturefailanimation.SetBool("capturefail", false);
                yield return new WaitForSeconds(1);
                state = BattleState.EnemyAttack;
                yield return dialogue.SetDialogue("You fail to capture");
                StartCoroutine(Attack());
            }
        }
        else
        {
            StartCoroutine(dialogue.SetDialogue("Your party is full."));
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
                StartCoroutine(CatchPokemon());
            }
            else if(selectionB == 2)
            {
                if (pokemonParties.playerParty.Count >= 2)
                {
                    dialogue.pokemonList.SetActive(true);
                    dialogue.selectionBox.SetActive(true);
                    state = BattleState.PokemonSelection;
                }
                else
                {
                    StartCoroutine(dialogue.SetDialogue("You only have one Pokemon."));
                }
            }
            else if(selectionB == 3)
            {
                StartCoroutine(Escape());
            }
        }
    }

    void PokemonSelection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (selectionC > 0)
            {
                selectionC -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectionC < pokemonParties.playerParty.Count - 1)
            {
                selectionC += 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!deadPokemon && selectionC == 0)
            {
                StartCoroutine(dialogue.SetDialogue("This Pokemon is already on the field!"));
            }
            else
            {
                StartCoroutine(SwitchPokemon());
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

    IEnumerator SwitchPokemon()
    {
        state = BattleState.EnemyAttack;
        switchIn = pokemonParties.playerParty[selectionC];
        pokemonParties.playerParty[selectionC] = pokemonParties.playerParty[0];
        pokemonParties.playerParty[0] = switchIn;
        yield return dialogue.SetDialogue($"You send out {switchIn.pokemonBase.pokeName}!");
        dialogue.pokemonList.SetActive(false);
        dialogue.selectionBox.SetActive(false);
        InitializePokemon();
        selectionC = 0;
        dialogue.SetPokemonNames(pokemonParties.playerParty);
        if (deadPokemon)
        {
            deadPokemon = false;
            state = BattleState.PlayerMenu;
            dialogue.menu.SetActive(true);
            StartCoroutine(dialogue.SetDialogue("Select an action."));
        }
        else
        {
            StartCoroutine(Attack());
        }
    }
    void InitializePokemon()
    {
        playerMon.Setup(pokemonParties.playerParty[0]);
        enemyMon.Setup(pokemonParties.enemyParty[0]);
        playerInfo.Setup(playerMon.pokemon);
        enemyInfo.Setup(enemyMon.pokemon);
    }

    IEnumerator Escape()
    {
        if (saveLoad.isTrainer)
        {
            StartCoroutine(dialogue.SetDialogue("You cannot escape a trainer battle!"));
        }
        else if (Random.Range(0, 10) <= 8)
        {
            state = BattleState.Busy;
            yield return dialogue.SetDialogue("You run away!");
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.EnemyAttack;
            yield return dialogue.SetDialogue("You fail to escape!");
            StartCoroutine(Attack());
        }
    }
}
