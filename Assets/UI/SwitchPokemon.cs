using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SwitchPokemon : MonoBehaviour
{
    public GameObject pokemonUI;
    public GameObject pokemons;
    public GameObject exit;
    public GameObject spriteBox;
    public Image sprite;
    public List<TextMeshProUGUI> yesNo;
    public List<TextMeshProUGUI> pokemonNames;
    public TextMeshProUGUI uiText;
    public PokemonParties pokemonParties;
    public int selectionA = 0;
    public int selectionB = 0;
    int flag1;
    int flag2;
    Pokemon pokemonA;
    Pokemon pokemonB;
    public bool isActive;
    public bool quitting;
    bool switching;
    bool fusing;


    // Update is called once per frame
    void Update()
    {
        Activate();
        if (quitting)
        {
            exit.SetActive(true);
            UpdateMenuSelection(selectionB, yesNo);
            QuitSelection(yesNo);
        }
        else if (isActive)
        {
            SetPokemonNames(pokemonParties.playerParty);
            pokemonUI.SetActive(true);
            pokemons.SetActive(true);
            sprite.enabled = true;
            spriteBox.SetActive(true);
            PokemonSelection();
            if (flag1 == 1)
            {
                UpdateMenuSelection(selectionB, pokemonNames);
            }
            else if (flag2 == 1)
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
            sprite.enabled = false;
            spriteBox.SetActive(false);
            exit.SetActive(false);
            selectionA = 0;
            selectionB = 0;
            switching = false;
            fusing = false;
            flag1 = 0;
            flag2 = 0;
        }
    }

    public void UpdateMenuSelection(int selection, List<TextMeshProUGUI> menu)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (selection == i)
            {
                menu[i].color = Color.blue;
                if (isActive)
                {
                    sprite.sprite = pokemonParties.playerParty[i].pokemonBase.pokeSprite;
                }
            }
            else menu[i].color = Color.black;
        }
        if (switching || fusing)
        {
            menu[selectionA].color = new Color (0, 0.5f, 0);
        }
    }

    public void QuitSelection(List<TextMeshProUGUI> yesNo)
    {
        if (Input.GetKeyDown(KeyCode.A) && selectionB == 1)
        {
            selectionB--;
        }
        else if (Input.GetKeyDown(KeyCode.D) && selectionB == 0)
        {
            selectionB++;
        }
        if (Input.GetKeyDown(KeyCode.Space) && selectionB == 0)
        {
            quitting = false;
            FindObjectOfType<MainMenu>(true).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Space) && selectionB == 1)
        {
            quitting = false;
        }
    }

    public void SetPokemonNames(List<Pokemon> pokemons)
    {
        for (int i = 0; i < pokemonNames.Count; i++)
        {
            if (i < pokemons.Count)
            {
                pokemonNames[i].text = $"{pokemons[i].pokemonBase.pokeName} -  <color=#{ColorUtility.ToHtmlStringRGB(pokemons[i].pokemonBase.type1Color)}>{pokemons[i].pokemonBase.type1}</color>";
                if (pokemons[i].pokemonBase.type2 != PokemonType.None)
                {
                    pokemonNames[i].text += $" / <color=#{ColorUtility.ToHtmlStringRGB(pokemons[i].pokemonBase.type2Color)}>{pokemons[i].pokemonBase.type2}</color>";
                }
            }
            else
                pokemonNames[i].text = "-";
        }

    }

    void Activate()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isActive && !quitting)
        {
            isActive = true;
            quitting = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !quitting && !isActive)
        {
            quitting = true;
            isActive = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && quitting)
        {
            quitting = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isActive)
        {
            isActive = false;
            uiText.text = "Pokemon";
        }
    }

    void PokemonSelection()
    { if (!switching && !fusing)
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
            else if (Input.GetKeyDown(KeyCode.Space) && pokemonParties.playerParty.Count >= 2)
            {
                switching = true;
                uiText.text = "Switching...";
            }
            else if (Input.GetKeyDown(KeyCode.E) && pokemonParties.playerParty.Count >= 2)
            {
                fusing = true;
                uiText.text = "Fusing...";
            }
        }
    else if (switching || fusing)
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
            else if (Input.GetKeyDown(KeyCode.Space) && switching)
            {
                Switch();
            }
            else if (Input.GetKeyDown(KeyCode.E) && fusing)
            {
                if (selectionA != selectionB)
                {
                    Fuse();
                }
            }
        }
    }

    void Switch()
    {
        pokemonA = pokemonParties.playerParty[selectionA];
        if (selectionB != selectionA)
        {
            pokemonB = pokemonParties.playerParty[selectionB];
            pokemonParties.playerParty[selectionA] = pokemonB;
            pokemonParties.playerParty[selectionB] = pokemonA;
            SetPokemonNames(pokemonParties.playerParty);
            switching = false;
            SetFlag();
        }
    }

    void Fuse()
    {
        pokemonA = pokemonParties.playerParty[selectionA];
        pokemonB = pokemonParties.playerParty[selectionB];
        int pokeLevel = (pokemonA.level + pokemonB.level) / 2;
        string fusionNameA = $"{pokemonA.pokemonBase.pokeName}{pokemonB.pokemonBase.pokeName}";
        string fusionNameB = $"{pokemonB.pokemonBase.pokeName}{pokemonA.pokemonBase.pokeName}";
        for (int i = pokemonParties.allPokemon.Count - 1; i > 0; i--)
        {
            if (fusionNameA == pokemonParties.allPokemon[i].pokemonBase.fusionName)
            {
                pokemonParties.playerParty.Remove(pokemonParties.playerParty[selectionA]);
                if (selectionA < selectionB)
                {
                    pokemonParties.playerParty.Remove(pokemonParties.playerParty[selectionB - 1]);
                }
                else
                {
                    pokemonParties.playerParty.Remove(pokemonParties.playerParty[selectionB]);
                }
                pokemonParties.playerParty.Add(pokemonParties.allPokemon[i]);
                pokemonParties.playerParty[pokemonParties.playerParty.Count - 1].level = pokeLevel;
                SetPokemonNames(pokemonParties.playerParty);
                fusing = false;
                break;
            }
            else if (fusionNameB == pokemonParties.allPokemon[i].pokemonBase.fusionName)
            {
                pokemonParties.playerParty.Remove(pokemonParties.playerParty[selectionA]);
                if (selectionA < selectionB)
                {
                    pokemonParties.playerParty.Remove(pokemonParties.playerParty[selectionB - 1]);
                }
                else
                {
                    pokemonParties.playerParty.Remove(pokemonParties.playerParty[selectionB]);
                }
                pokemonParties.playerParty.Add(pokemonParties.allPokemon[i]);
                pokemonParties.playerParty[pokemonParties.playerParty.Count - 1].level = pokeLevel;
                SetPokemonNames(pokemonParties.playerParty);
                fusing = false;
                break;
            }
        }
        if (!fusing)
        {
            selectionA = 0;
            selectionB = 0;
            SetFlag();
        }
        else
        {
            uiText.text = "Fusion failed.";
            selectionA = selectionB;
            fusing = false;
            Invoke("SetFlag", 1f);
        }
    }

    void SetFlag()
    {
        if (switching && flag1 == 0)
        {
            flag1++;
            selectionB = selectionA;
        }
        else if (fusing && flag2 == 0)
        {
            flag2++;
            selectionB = selectionA;
        }
        else if (!switching && flag1 == 1)
        {
            flag1 = 0;
            selectionA = selectionB;
            uiText.text = "Pokemon";
        }
        else if (!fusing && flag2 == 1)
        {
            flag2 = 0;
            selectionA = selectionB;
            uiText.text = "Pokemon";
        }
    }
}
