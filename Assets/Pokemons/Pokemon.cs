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

    public int attack
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 5; }
    }
    public int spAttack
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 5; }
    }
    public int defense
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 5; }
    }
    public int spDefense
    {
        get { return (int)((pokemonBase.attack * level) / 100f) + 5; }
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

    public bool TakeDamage(Move move, Pokemon Playerattacker)
    {
        float criticalHit = 1f;
        if(Random.Range(0,100) *100.0f <= 10.25f)
        {
            criticalHit = 2.0f;
        }
        float type = PokemonTypeChart.GetDamageEffectiveness(move.Base.type, this.pokemonBase.type1) * PokemonTypeChart.GetDamageEffectiveness(move.Base.type, this.pokemonBase.type2);

        int attack;
        int defense;

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

        float mod = Random.Range(0.85f, 0.9f)* type *criticalHit;
        float calculationDamage1 = ((2 * Playerattacker.level) + 10) / 250.0f;
        float calculationDamage2 = ((move.Base.power) * (attack / defense) + 2);
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

