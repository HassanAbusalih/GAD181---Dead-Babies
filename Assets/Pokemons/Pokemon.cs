using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon 
{
    [SerializeField] public PokemonBase pokemonBase;
    [SerializeField] public int level;
    public float totalHP;
    public float currentHP;
    public float currentXpPoints;
    public float xpThreshhold;
    public List<Move> pMoves;
    [HideInInspector] public int attackIncr;
    [HideInInspector] public int defenceIncr;
    [HideInInspector] public int maxHpIncr;
    [HideInInspector] public bool isLevelUp;

    public Pokemon (PokemonBase pBase, int pLevel)
    {
         level = pLevel;
         pokemonBase = pBase;
         totalHP = PokemonHealth(pokemonBase.maxHp);
         currentHP = PokemonHealth(pokemonBase.maxHp);
         pMoves = new List<Move>();
         foreach (var move in pokemonBase.learnableMoves)
         {
            if (move.level <= level)
            {
                pMoves.Add(new Move(move.moves));
                if (pMoves.Count == 4)
                {
                    break;
                }
            }
         }
    }

    public void MakePokemon()
    {
        totalHP = PokemonHealth(pokemonBase.maxHp);
        currentHP = PokemonHealth(pokemonBase.maxHp);
        xpThreshhold = 30;
        pMoves = new List<Move>();
        foreach (var move in pokemonBase.learnableMoves)
        {
            if (move.level <= level)
            {
                pMoves.Add(new Move(move.moves));
                if (pMoves.Count == 4)
                {
                    break;
                }
            }
        }
    }

    float PokemonHealth(int maxHP)
    {
        float hp = (maxHP * level);
        return hp;
    }

    public bool TakeDamage(Move move)
    {
        float mod = Random.Range(0.85f, 0.9f);
        float damage = move.Base.power * mod;
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        return false;
    }

    public Move RandomMove() 
    {
        int i = Random.Range(0, pMoves.Count);
        return pMoves[i];
    }
    public IEnumerator LevelUp(int xpGained)
    {
        attackIncr = Random.Range(1, 4);
        defenceIncr = Random.Range(1, 4);
        currentXpPoints += xpGained;
        if (currentXpPoints >= xpThreshhold)
        {
            level++;
            maxHpIncr = level + 10;
            pokemonBase.maxHp += maxHpIncr;
            pokemonBase.attack += attackIncr;
            pokemonBase.defense += defenceIncr;
            currentXpPoints -= xpThreshhold;
            xpThreshhold += 30;
            isLevelUp = true;
            yield return null;
        }
    }
}

