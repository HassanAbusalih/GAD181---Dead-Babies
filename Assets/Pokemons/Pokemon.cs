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

    public Pokemon (PokemonBase pBase, int pLevel)
    {
         level = pLevel;
         pokemonBase = pBase;
    }

    public void MakePokemon()
    {
        totalHP = PokemonHealth();
        currentHP = PokemonHealth();
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

    float PokemonHealth()
    {
        float hp =  ((2 * maxHp * level) / 100) + level + 10;
        return hp;
    }

    public (bool, bool, float) TakeDamage(Move move, Pokemon Playerattacker)
    {
        bool crit = false;
        float criticalHit = 1f;
        if(Random.Range(0, 200) <= Playerattacker.pokemonBase.speed)
        {
            criticalHit = 2.0f;
            crit = true;
        }
        float type = PokemonTypeChart.GetDamageEffectiveness(move.Base.type, this.pokemonBase.type1) * PokemonTypeChart.GetDamageEffectiveness(move.Base.type, this.pokemonBase.type2);
        
        int pAttack;
        int pDefense;
        float stab = 1f;

        if(Playerattacker.pokemonBase.type1 == move.Base.type || Playerattacker.pokemonBase.type2 == move.Base.type)
        {
            stab = 1.5f;
        }

       if ( move.Base.category == Moves.MoveType.Special)
        {
            pAttack = Playerattacker.spAttack;
            pDefense = spDefense;
        }
        else
        {
            pAttack = Playerattacker.attack;
            pDefense = defense;

        }

        float mod = Random.Range(0.85f, 0.9f) * criticalHit;
        float calculationDamage1 = ((2 * Playerattacker.level) + 10) / 250.0f;
        float calculationDamage2 = (move.Base.power * pAttack / pDefense) + 2;
        int damage = (int)(calculationDamage1 * calculationDamage2 * mod * stab * type);
        currentHP -= damage;
        bool fainted = false;
        if (currentHP <= 0)
        {
            currentHP = 0;
            fainted = true;
            return (fainted, crit, type);
        }
        return (fainted, crit, type);
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

    public int  attack
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 5; }
    }

    public int defense
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 5; }
    }

    public int spAttack
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 5; }
    }

    public int spDefense
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 5; }
    }

    public int maxHp
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 10; }
    }




}

