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

    public Pokemon (PokemonBase pBase, int pLevel) // Added a new parameter for the xp.
    {
         level = pLevel;
         pokemonBase = pBase;
    }

    public void MakePokemon()
    {
        totalHP = PokemonHealth(pokemonBase.maxHp);
        currentHP = PokemonHealth(pokemonBase.maxHp);
        currentXpPoints += currentXpPoints;
        xpThreshhold = XpToNextLevel(level);
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
    public float XpToNextLevel(int level)
    {
        return Mathf.Floor(100 * Mathf.Pow(level, (float)1.2));
    }
    public void Evolve()
    {
        if(level >= pokemonBase.evolutions.levelForEvolve)
        {
            pokemonBase = pokemonBase.evolutions.evolveTo;
        }
    }
}

