using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject menuText;
    [SerializeField] GameObject newGame;
    [SerializeField] GameObject page1;
    [SerializeField] GameObject page2;
    [SerializeField] List<TextMeshProUGUI> menuSelect;
    [SerializeField] List<TextMeshProUGUI> starterNames;
    [SerializeField] List<Pokemon> starters;
    [SerializeField] List<Image> starterSprites;
    [SerializeField] AudioSource menuTheme;
    [SerializeField] AudioSource gameTheme;
    PlayerMovement player;
    int selection;
    bool ng;
    bool guide;

    private void Start()
    {
        MainMenu[] gameMenus = FindObjectsOfType<MainMenu>(true);
        Canvas canvas = GetComponent<Canvas>();
        if (!canvas.enabled && gameMenus.Length == 1)
        {
            canvas.enabled = true;
        }
        player = FindObjectOfType<PlayerMovement>();
        DontDestroyOnLoad(gameObject);
        player.gameObject.SetActive(false);
        menuTheme.Play();
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.gameObject.SetActive(true);
        }
        else
        {
            player = FindObjectOfType<PlayerMovement>();
            gameTheme = player.GetComponent<AudioSource>();
        }
        menuTheme.Stop();
        gameTheme.Play();
    }

    private void OnEnable()
    {
        if (player != null)
        {
            player.gameObject.SetActive(false);
        }
        else
        {
            player = FindObjectOfType<PlayerMovement>();
            gameTheme = player.GetComponent<AudioSource>();
        }
        menuTheme.Play();
        gameTheme.Stop();
    }

    private void Update()
    {
        if (gameObject.activeSelf && !ng && !guide)
        {
            Selection();
        }
        else if (ng)
        {
            StarterSelection();
        }
        else if (guide)
        {
            GuideSelection();
        }
        MainMenu[] gameMenus = FindObjectsOfType<MainMenu>(true);
        if (gameObject.activeSelf && gameMenus.Length > 1)
        {
            Destroy(gameObject);
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

    void StarterSelection()
    {
        UpdateMenuSelection(selection, starterNames);
        if (Input.GetKeyDown(KeyCode.A) && selection > 0)
        {
            selection--;
        }
        if (Input.GetKeyDown(KeyCode.D) && selection < starterNames.Count - 1)
        {
            selection++;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
            player.gameObject.transform.position = new Vector2(0, 0);
            FindObjectOfType<PokemonParties>().playerParty.Clear();
            FindObjectOfType<PokemonParties>().playerParty.Add(starters[selection]);
            FindObjectOfType<PokemonParties>().playerParty[0].level = 5;
            selection = 0;
            ng = false;
            player = FindObjectOfType<PlayerMovement>(true);
            newGame.SetActive(false);
            menuText.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void SetStarters()
    {
        for (int i = 0; i < starterNames.Count; i++)
        {
            starterSprites[i].sprite = starters[i].pokemonBase.pokeSprite;
            starterNames[i].text = starters[i].pokemonBase.pokeName;
        }
    }

    public void Selection()
    {
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(selection < menuSelect.Count - 1)
            {
                selection++;
                UpdateMenuSelection(selection, menuSelect);
            }          
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (selection > 0)
            {
                selection--;
                UpdateMenuSelection(selection, menuSelect);
            }
        }
        if (selection == 0)
        {          
            UpdateMenuSelection(selection, menuSelect);
        }
        if (selection == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            newGame.SetActive(true);
            menuText.SetActive(false);
            SetStarters();
            ng = true;
            selection = 0;
        }
        if (selection == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            if (FindObjectOfType<PokemonParties>().playerParty.Count >= 1)
            {
                player = FindObjectOfType<PlayerMovement>(true);
                gameObject.SetActive(false);
            }
        }
        else if (selection == 2 && Input.GetKeyDown(KeyCode.Space))
        {
            guide = true;
            page1.SetActive(true);
        }
        if (selection == 3 && Input.GetKeyDown(KeyCode.Space))
        {
            List<Pokemon> playerPokemon = FindObjectOfType<PokemonParties>().playerParty;
            if (playerPokemon.Count > 0)
            {
                player.saveLoad.PlayerSave();
                player.SavePos();
            }
            Debug.Log("quit");
            Application.Quit();
        }
    }

    void GuideSelection()
    {
        if (page1.activeSelf && Input.GetKeyDown(KeyCode.D))
        {
            page1.SetActive(false);
            page2.SetActive(true);
        }
        if (page2.activeSelf && Input.GetKeyDown(KeyCode.A))
        {
            page2.SetActive(false);
            page1.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            page1.SetActive(false);
            page2.SetActive(false);
            guide = false;
        }
    }
}
