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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupBattle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SetupBattle()
    {
        playerMon.Setup();
        enemyMon.Setup();
        playerInfo.Setup(playerMon.pokemon);
        enemyInfo.Setup(enemyMon.pokemon);
        yield return dialogue.SetDialogue("Battle Test");
        yield return new WaitForSeconds(1f);
        PlayerTurn();
    }

    void PlayerTurn()
    {
        BattleState state = BattleState.PlayerTurn;
        dialogue.dialoguetext.enabled = false;
        dialogue.Attacks.SetActive(true);
        dialogue.SetMoves(playerMon.pokemon.pMoves);

    }
}
