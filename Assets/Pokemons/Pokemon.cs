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
        //currentXpPoints = 0;
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

    public bool TakeDamage(Move move, Pokemon Playerattacker)
    {
        float criticalHit = 1f;
        if(Random.Range(0,100) *100.0f <= 10.25f)
        {
            criticalHit = 2.0f;
        }
        float type = PokemonTypeChart.GetDamageEffectiveness(move.Base.type, this.pokemonBase.type1) * PokemonTypeChart.GetDamageEffectiveness(move.Base.type, this.pokemonBase.type2);

        int attack = 1;
        int defense = 1;

       if ( move.Base.category == Moves.MoveType.Special)
        {
            attack = Playerattacker.pokemonBase.spAttack;
            defense = pokemonBase.spDefence;
        }
        else
        {
            attack = Playerattacker.pokemonBase.attack;
            defense = pokemonBase.defense;

        }

        float mod = Random.Range(0.85f, 0.9f) * criticalHit;
        float calculationDamage1 = ((2 * Playerattacker.level) + 10) / 250.0f;
        float calculationDamage2 = ((move.Base.power * attack) / defense) + 2;
        int damage = (int)(calculationDamage1 * calculationDamage2 * mod);
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

