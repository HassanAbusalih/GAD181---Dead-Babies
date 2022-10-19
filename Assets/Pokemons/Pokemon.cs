using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon 
{
    public PokemonBase pokemon;
    public int level;
    public List<Move> pMoves;

    public Pokemon(PokemonBase Base, int pokemonLevel)
    {
        pokemon = Base;
        level = pokemonLevel;
        pMoves = new List<Move>();
        foreach (var move in Base.learnableMoves)
        {
            if (move.level <= level)
                pMoves.Add(new Move(move.moves));

            if (pMoves.Count == 4)
                break;
                
        }
    }
}
