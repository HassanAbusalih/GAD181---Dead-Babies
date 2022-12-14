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
    public AudioSource  audiosource1;
    public AudioSource audiosource2;
    public AudioSource levelUpSFX;
    public SaveLoad saveLoad;
    public Animator captureanimation;
    public Animator capturefailanimation;
    public Animator playerattack;
    public Animator enemyattack;
    public SpriteRenderer enemypokemon;
    public EvoloutionUI evoloutionUI;
    public bool battleEnd;
    GameObject mainMenu;
    public XpBar xpBar;
    Pokemon switchIn;
    BattleState state;
    int selection;
    int selectionB;
    int selectionC;
    bool deadPokemon;
    bool tutorial;
    int xpGain;
    [SerializeField] AudioSource playerattackSFX;
    [SerializeField] AudioSource loosingHPSFX;
    [SerializeField] AudioSource enemyattackSFX;
    [SerializeField] private AudioSource SelectionSoundEffect;
    [SerializeField] private AudioSource ClickSound;
    [SerializeField] private AudioClip Playerattack;
    [SerializeField] private AudioClip loosingHP;
    [SerializeField] private AudioClip enemmyattack;
    [SerializeField] private AudioClip SelectionSound;
    [SerializeField] private AudioClip Click;

    // Start is called before the first frame update

    void Start()
    {
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            tutorial = true;
            PlayerPrefs.SetInt("Tutorial", 2);
        }
        animator.SetBool("BattleStart", true);
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
        if (state == BattleState.MoveSelection)
        {
            MoveSelection();
            dialogue.UpdateMoveSelection(selection, dialogue.pokeMoves, playerMon.pokemon.pMoves[selection]);
        }
        if (state == BattleState.PlayerAttack)
        {
            StartCoroutine(Attack());
        }
    }

    public IEnumerator StartBattle()
    {
        state = BattleState.Start;
        dialogue.SetPokemonNames(pokemonParties.playerParty);
        InitializePokemon();
        xpBar.SetXpBar(playerMon.pokemon.currentXpPoints, playerMon.pokemon.xpThreshhold);
        if (saveLoad.isTrainer)
        {
            yield return dialogue.SetDialogue($"{saveLoad.trainerName} challenges you!");
        }
        else
        {
            yield return dialogue.SetDialogue("A wild " + enemyMon.pokemon.pokemonBase.pokeName + " appears!");
        }
        yield return new WaitForSeconds(2.1f);
        animator.SetBool("BattleStart", false);
        animator.enabled = false;
        if (tutorial)
        {
            dialogue.tutorialText.enabled = true;
            dialogue.tutorialBox.SetActive(true);
            yield return dialogue.SetTutorialDialogue(dialogue.tutorialTextList);
            dialogue.tutorialText.enabled = false;
            dialogue.tutorialBox.SetActive(false);
        }
        yield return dialogue.SetDialogue("Select an action.");
        state = BattleState.PlayerMenu;
    }

    IEnumerator PlayerTurn()
    {
        state = BattleState.Busy;
        yield return dialogue.SetDialogue("Choose a move.");
        state = BattleState.MoveSelection;
        dialogue.dialoguetext.enabled = false;
        dialogue.info.SetActive(true);
        dialogue.attacks.SetActive(true);
        dialogue.SetMoves(playerMon.pokemon.pMoves);
    }

    IEnumerator PlayerMenu()
    {
        playerattack.SetBool("attack", false);
        playerattack.SetBool("defense", false);
        enemyattack.SetBool("enemy defense", false);
        enemyattack.SetBool("enemy attack", false);
        yield return dialogue.SetDialogue("Select an action.");
        state = BattleState.PlayerMenu;
        dialogue.info.SetActive(false);
        dialogue.menu.SetActive(true);
    }

    IEnumerator Attack()
    {
        if (state == BattleState.PlayerAttack)
        {
            playerattackSFX.PlayOneShot(Playerattack);
            state = BattleState.Busy;
            Move move = playerMon.pokemon.pMoves[selection];
            playerMon.pokemon.pMoves[selection].powerpoints--;
            playerattack.SetBool("attack", true);
            yield return new WaitForSeconds(1.15f);
            enemyattack.SetBool("enemy defense", true);
            (bool fainted, bool crit, float type) battleResult = enemyMon.pokemon.TakeDamage(move, playerMon.pokemon);
            enemyInfo.DamageTaken();
            yield return dialogue.SetDialogue(playerMon.pokemon.pokemonBase.pokeName + " uses " + move.Base.name + "!");
            selection = 0;
            if (battleResult.crit && battleResult.type > 1)
            {
                yield return dialogue.SetDialogue("A super effective critical hit!");
            }
            else if (battleResult.crit && battleResult.type < 1)
            {
                yield return dialogue.SetDialogue("A critical hit! But it's not very effective...");
            }
            else if (battleResult.type > 1)
            {
                yield return dialogue.SetDialogue("It's super effective!");
            }
            else if (battleResult.type < 1)
            {
                yield return dialogue.SetDialogue("It's not very effective...");
            }
            if (battleResult.fainted)
            {
                int enemyLevel = enemyMon.pokemon.level;
                xpGain = Mathf.FloorToInt((340 * enemyLevel) / 4);
                playerMon.pokemon.currentXpPoints += xpGain;
                yield return dialogue.SetDialogue(enemyMon.pokemon.pokemonBase.pokeName + " fainted!");
                xpBar.SetXpBar(playerMon.pokemon.currentXpPoints, playerMon.pokemon.xpThreshhold);
                yield return dialogue.SetDialogue(playerMon.pokemon.pokemonBase.pokeName + " recieves " + xpGain + " XP.");
                while (playerMon.pokemon.currentXpPoints >= playerMon.pokemon.xpThreshhold)
                {
                    playerMon.pokemon.currentXpPoints -= playerMon.pokemon.xpThreshhold;
                    playerMon.pokemon.level++;
                    levelUpSFX.Play();
                    playerMon.pokemon.xpThreshhold = playerMon.pokemon.XpToNextLevel(playerMon.pokemon.level);
                    xpBar.SetXpBar(playerMon.pokemon.currentXpPoints, playerMon.pokemon.xpThreshhold);
                    playerInfo.Setup(playerMon.pokemon);
                    yield return StartCoroutine(dialogue.SetDialogue($"{playerMon.pokemon.pokemonBase.pokeName} leveled up to lvl  {playerMon.pokemon.level}."));                  
                }
                pokemonParties.enemyParty.Remove(pokemonParties.enemyParty[0]);
                if (pokemonParties.enemyParty.Count == 0)
                {
                    state = BattleState.PlayerWin;
                    yield return StartCoroutine(Evolution());
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
             playerattackSFX.PlayOneShot(Playerattack);
            enemyattackSFX.PlayOneShot(enemmyattack);
            state = BattleState.Busy;
            Move move = enemyMon.pokemon.RandomMove();
            enemyattack.SetBool("enemy defense", false);
            playerattack.SetBool("attack", false);
            enemyattack.SetBool("enemy attack", true);
            playerattack.SetBool("defense", true);
            (bool fainted, bool crit, float type) battleResult = playerMon.pokemon.TakeDamage(move, enemyMon.pokemon);
            playerInfo.DamageTaken();
            yield return dialogue.SetDialogue(enemyMon.pokemon.pokemonBase.pokeName + " uses " + move.Base.name + "!");
            if (battleResult.crit && battleResult.type > 1)
            {
                yield return dialogue.SetDialogue("A super effective critical hit!");
            }
            else if (battleResult.crit && battleResult.type < 1)
            {
                yield return dialogue.SetDialogue("A critical hit! But it's not very effective...");
            }
            else if (battleResult.type > 1)
            {
                yield return dialogue.SetDialogue("It's super effective!");
            }
            else if (battleResult.type < 1)
            {
                yield return dialogue.SetDialogue("It's not very effective...");
            }
            if (battleResult.fainted)
            {
                yield return dialogue.SetDialogue(playerMon.pokemon.pokemonBase.pokeName + " fainted!");
                pokemonParties.playerParty.Remove(pokemonParties.playerParty[0]);
                saveLoad.PlayerSave();
                if (pokemonParties.playerParty.Count == 0)
                {
                    state = BattleState.EnemyWin;
                    yield return dialogue.SetDialogue("You lose!");
                    PlayerPrefs.DeleteAll();
                    Destroy(FindObjectOfType<MainMenu>(true).gameObject);
                    StartCoroutine(EndBattle());
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
                StartCoroutine(PlayerMenu());
            }
        }
    }
    IEnumerator CatchPokemon()
    {
        state = BattleState.Busy;
        if (saveLoad.isTrainer == true)
        {
            StartCoroutine(dialogue.SetDialogue("You cannot capture a trainer's Fera."));
            state = BattleState.PlayerMenu;
        }
        else if (pokemonParties.playerParty.Count < 6)
        {
            captureanimation.SetBool("Capture", true);
            yield return new WaitForSeconds(1);
            enemypokemon.GetComponent<SpriteRenderer>().enabled = false;
            if (Random.Range(0, 10) <= 6)
            {
                state = BattleState.PlayerWin;
                pokemonParties.playerParty.Add(pokemonParties.enemyParty[0]);
                Victory();
                StartCoroutine(dialogue.SetDialogue("You have captured a " + pokemonParties.enemyParty[0].pokemonBase.pokeName + "!"));
                saveLoad.PlayerSave();
                StartCoroutine(EndBattle());
            }
            else
            {
                captureanimation.SetBool("Capture", false);
                capturefailanimation.SetBool("capturefail", true);
                yield return new WaitForSeconds(2.5f);
                enemypokemon.GetComponent<SpriteRenderer>().enabled = true;
                StartCoroutine(dialogue.SetDialogue("The Force is strong with him."));
                capturefailanimation.SetBool("capturefail", false);
                yield return new WaitForSeconds(1);
                state = BattleState.EnemyAttack;
                yield return dialogue.SetDialogue("You fail to capture the Fera!");
                StartCoroutine(Attack());
            }
        }
        else
        {
            StartCoroutine(dialogue.SetDialogue("Your party is full."));
            state = BattleState.PlayerMenu;
        }
    }
    void MenuSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (selectionB < dialogue.menuActions.Count - 1)
            {
                SelectionSoundEffect.PlayOneShot(SelectionSound);
                selectionB++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (selectionB > 0)
            {
                SelectionSoundEffect.PlayOneShot(SelectionSound);
                selectionB--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (selectionB > 1)
            {
            	SelectionSoundEffect.PlayOneShot(SelectionSound);
                selectionB -= 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectionB < dialogue.menuActions.Count - 2)
            {
            	SelectionSoundEffect.PlayOneShot(SelectionSound);
                selectionB += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
			ClickSound.PlayOneShot(Click);
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
                    StartCoroutine(dialogue.SetDialogue("You only have one Fera."));
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
            	SelectionSoundEffect.PlayOneShot(SelectionSound);
                selectionC -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectionC < pokemonParties.playerParty.Count - 1)
            {
            	SelectionSoundEffect.PlayOneShot(SelectionSound);
                selectionC += 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickSound.PlayOneShot(Click);
            dialogue.pokemonList.SetActive(false);
            dialogue.selectionBox.SetActive(false);
            state = BattleState.PlayerMenu;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ClickSound.PlayOneShot(Click);
            if (!deadPokemon && selectionC == 0)
            {
                StartCoroutine(dialogue.SetDialogue("This Fera is already on the field!"));
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
            	SelectionSoundEffect.PlayOneShot(SelectionSound);
                selection++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (selection > 0)
            {
            	SelectionSoundEffect.PlayOneShot(SelectionSound);
                selection--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (selection > 1)
            {
            	SelectionSoundEffect.PlayOneShot(SelectionSound);
                selection -= 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selection < playerMon.pokemon.pMoves.Count - 2)
            {
            	SelectionSoundEffect.PlayOneShot(SelectionSound);
                selection += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickSound.PlayOneShot(Click);
            StartCoroutine(PlayerMenu());

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ClickSound.PlayOneShot(Click);
            if (playerMon.pokemon.pMoves[selection].powerpoints == 0)
            {
                
            }
            else
            {
                state = BattleState.PlayerAttack;
                dialogue.info.SetActive(false);
            }
        }
    }

    void Victory()
    {
        if (state == BattleState.PlayerWin)
        {
            audiosource2.Stop();
            audiosource1.Play();
        }
        //playerMon.pokemon.Evolve();
    }
    IEnumerator Evolution()
    {
        if(playerMon.pokemon.level >= playerMon.pokemon.pokemonBase.evolutions.levelForEvolve && playerMon.pokemon.pokemonBase.evolutions.levelForEvolve != 0)
        {
            audiosource2.Stop();
            yield return evoloutionUI.Evolve(playerMon.pokemon);
            playerMon.Setup(playerMon.pokemon);
            playerInfo.Setup(playerMon.pokemon);
        }
    }

    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(2);
        animator.enabled = true;
        animator.SetBool("End", true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(1.5f);
        asyncOperation.allowSceneActivation = true;
        battleEnd = true;
    }

    IEnumerator SwitchPokemon()
    {
        state = BattleState.Busy;
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
            yield return dialogue.SetDialogue("Select an action.");
            state = BattleState.PlayerMenu;
            dialogue.menu.SetActive(true);
        }
        else
        {
            state = BattleState.EnemyAttack;
            StartCoroutine(Attack());
        }
    }
    void InitializePokemon()
    {
        playerMon.Setup(pokemonParties.playerParty[0]);
        enemyMon.Setup(pokemonParties.enemyParty[0]);
        playerInfo.Setup(playerMon.pokemon);
        enemyInfo.Setup(enemyMon.pokemon);
        xpBar.SetXpBar(playerMon.pokemon.currentXpPoints, playerMon.pokemon.xpThreshhold);
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
            saveLoad.PlayerSave();
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
