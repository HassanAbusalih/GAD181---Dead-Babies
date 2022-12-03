using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> menuSelect;
    int selectionA;
    int selectionB;
    PlayerMovement player;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        player = GameObject.Find("Player(Pink)").GetComponent<PlayerMovement>();
        player.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        player.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        player.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            Selection();
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
    public void Selection()
    {
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(selectionA < menuSelect.Count -1)
            {
                selectionA++;
                UpdateMenuSelection(selectionA, menuSelect);
            }          
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (selectionA > 0)
            {
                selectionA--;
                UpdateMenuSelection(selectionA, menuSelect);
            }
        }
        if (selectionA == 0)
        {          
            UpdateMenuSelection(selectionA, menuSelect);
        }
        if (selectionA == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            player = FindObjectOfType<PlayerMovement>(true);
            gameObject.SetActive(false);

        }
        if (selectionA == 3 && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("quit");
            Application.Quit();
        }
    }
}
