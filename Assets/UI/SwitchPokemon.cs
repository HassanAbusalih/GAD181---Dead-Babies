using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SwitchPokemon : MonoBehaviour
{
    public GameObject pokemonUI;
    public GameObject pokemons;
    public List<TextMeshProUGUI> pokemonNames;
    public PokemonParties pokemonParties;
    int selectionA = 0;
    int selectionB = 0;
    int flag;
    Pokemon pokemonA;
    Pokemon pokemonB;
    public bool isActive;
    bool switching;

    // Start is called before the first frame update
    void Start()
    {
        SetPokemonNames(pokemonParties.playerParty);
    }

    // Update is called once per frame
    void Update()
    {
        Activate();
        if (isActive)
        {
            pokemonUI.SetActive(true);
            pokemons.SetActive(true);
            PokemonSelection();
            if (flag == 1)
            {
                UpdateMenuSelection(selectionB, pokemonNames);
            }
            else
            {
                UpdateMenuSelection(selectionA, pokemonNames);
            }
        }
        else
        {
            pokemonUI.SetActive(false);
            pokemons.SetActive(false);
            selectionA = 0;
            selectionB = 0;
            switching = false;
            flag = 0;
        }
    }

    public void UpdateMenuSelection(int selection, List<TextMeshProUGUI> menu)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (selection == i)
            {
                menu[i].color = Color.blue;
            }
            else menu[i].color = Color.black;
        }
    }

    public void SetPokemonNames(List<Pokemon> pokemons)
    {
        for (int i = 0; i < pokemonNames.Count; i++)
        {
            if (i < pokemons.Count)
                pokemonNames[i].text = pokemons[i].pokemonBase.pokeName;
            else
                pokemonNames[i].text = "-";
        }

    }

    void Activate()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isActive)
        {
            isActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isActive)
        {
            isActive = false;
        }
    }

    void PokemonSelection()
    { if (!switching)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (selectionA > 0)
                {
                    selectionA -= 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (selectionA < pokemonParties.playerParty.Count - 1)
                {
                    selectionA += 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                switching = true;
            }
        }
    else if (switching)
        {
            SetFlag();
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (selectionB > 0)
                {
                    selectionB -= 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (selectionB < pokemonParties.playerParty.Count - 1)
                {
                    selectionB += 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Switch();
            }
        }
    }

    void Switch()
    {
        pokemonA = pokemonParties.playerParty[selectionA];
        if (Input.GetKeyDown(KeyCode.Space) && selectionB != selectionA)
        {
            pokemonB = pokemonParties.playerParty[selectionB];
            pokemonParties.playerParty[selectionA] = pokemonB;
            pokemonParties.playerParty[selectionB] = pokemonA;
            SetPokemonNames(pokemonParties.playerParty);
            switching = false;
            SetFlag();
        }
    }

    void SetFlag()
    {
        if (switching && flag == 0)
        {
            flag++;
            selectionB = selectionA;
        }
        else if (!switching && flag == 1)
        {
            flag = 0;
            selectionA = selectionB;
        }
    }
}
