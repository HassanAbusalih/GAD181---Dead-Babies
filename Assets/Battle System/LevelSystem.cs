using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public PokemonParties pokemonParties;
    public Pokemon pokemon;
    public BattleDialogue dialogue;
    public Image xpBar;
    public int xpPoints = 0;
    public int levelThreshhold = 30;

    public IEnumerator LevelUp()
    {
        if(xpPoints >= levelThreshhold)
        {
            yield return null;
            pokemonParties.playerParty[0].level++;
            xpPoints = xpPoints - levelThreshhold;
            levelThreshhold = levelThreshhold + 30;
            StartCoroutine( dialogue.SetDialogue("You Leveld up to lvl " + pokemonParties.playerParty[0].level));
        }
    }
    public void SetXp(float xp, float maxXp)
    {
        xpBar.transform.localScale = new Vector3(xp / maxXp, 1, 1);
    }
}
