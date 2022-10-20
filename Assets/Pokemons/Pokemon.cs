using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon 
{
    public PokemonBase pokemon;
    public int level;
    public float totalHP;
    public float currentHP;
    public List<Move> pMoves;

    public Pokemon(PokemonBase Base, int pokemonLevel)
    {
        pokemon = Base;
        level = pokemonLevel;
        totalHP = PokemonHealth(pokemon.maxHp);
        currentHP = PokemonHealth(pokemon.maxHp);
        pMoves = new List<Move>();
        foreach (var move in Base.learnableMoves)
        {
            if (move.level <= level)
                pMoves.Add(new Move(move.moves));

            if (pMoves.Count == 4)
                break;
                
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
}
