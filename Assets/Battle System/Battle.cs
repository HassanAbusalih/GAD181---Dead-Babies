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
    int pokemonNumber;
    BattleState state;
    int selection;

    // Start is called before the first frame update
    void Start()
    {
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

    public IEnumerator StartBattle()
    {
        state = BattleState.Start;
        yield return StartCoroutine(SetupPokemon());
        yield return dialogue.SetDialogue("A wild " + enemyMon.pokemon.pokemonBase.pokeName + " appears!");
        StartCoroutine(PlayerTurn());
    }

    public IEnumerator SetupPokemon()
    { 
        //yield return StartCoroutine(Load());
        yield return new WaitForSeconds(0.1f);
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
                    Save();
                    yield return EndBattle();
                }
                else
                {
                    yield return new WaitForSeconds(1f);
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
                Save();
                if (pokemonParties.playerParty.Count == 0)
                {
                    state = BattleState.EnemyWin;
                    yield return dialogue.SetDialogue("You lose!");
                    yield return EndBattle();
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                    yield return SetupPokemon();
                    yield return dialogue.SetDialogue("You send out " + playerMon.pokemon.pokemonBase.pokeName + "!");
                    StartCoroutine(PlayerTurn());
                }
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

    IEnumerator Load()
    {
        yield return null;
        for (int i = 0; i < PlayerPrefs.GetInt("Party"); i++)
        {
            pokemonNumber = PlayerPrefs.GetInt($"pokemon{i}");
            for (int j = 0; j < pokemonParties.allPokemon.Count; j++)
            {
                if (pokemonNumber == pokemonParties.allPokemon[j].pokemonBase.pokeNumber)
                {
                    pokemonParties.playerParty.Add(pokemonParties.allPokemon[j]);
                    pokemonParties.playerParty[pokemonParties.playerParty.Count].level = PlayerPrefs.GetInt($"pokemonLevel{i}");
                }
            }
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Party", pokemonParties.playerParty.Count);
        for (int i = 0; i < pokemonParties.playerParty.Count; i++)
        {
            PlayerPrefs.SetInt($"pokemon{i}", pokemonParties.playerParty[i].pokemonBase.pokeNumber);
            PlayerPrefs.SetInt($"pokemonLevel{i}", pokemonParties.playerParty[i].level);
        }
    }

}
