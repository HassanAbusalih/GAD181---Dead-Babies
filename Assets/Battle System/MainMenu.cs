using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> menuSelect;
    int selectionA;
    int selectionB;
    GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player(Pink)");
        player.SetActive(false);
    }
    private void Update()
    {
        Selection();
        ActivateSelection();
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

    }
    public void ActivateSelection()
    {
        if(selectionA == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false); // Game start should take you to the starter pokemon screen select instead of just starting the game.
            player.SetActive(true);
        }
        if (selectionA == 3 && Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log("quit");
            Application.Quit(); // will work in build only
        }
    }
}
