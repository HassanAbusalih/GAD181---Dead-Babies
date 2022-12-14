using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Textbox : MonoBehaviour
{
    [SerializeField] List<string> textList = new List<string>();
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject textBox;
    public SaveLoad saveLoad;
    Collider2D textCollider;
    Collider2D playerCollider;
    bool isActive;
    int counter;
    Trainer trainer;

    [SerializeField] AudioSource SelectionSFX;
    [SerializeField] private AudioClip SelectSFX;

    private void Awake()
    {
        trainer = GetComponent<Trainer>();
        textCollider = GetComponent<Collider2D>();
        playerCollider = FindObjectOfType<PlayerMovement>().GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (textCollider.IsTouching(playerCollider))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            { 
                if (!isActive)
                {
                    isActive = true;
                    textBox.SetActive(true);
                    text.gameObject.SetActive(true);
                    //FindObjectOfType<PlayerMovement>().encounter = true;  //adding this line in pervents movement during dialogue
                }
                NextLine();
            }
            else if (trainer != null)
            {
                if (!saveLoad.isTrainer && PlayerPrefs.GetInt($"{trainer.trainerBase.trainerName}") == 1 && PlayerPrefs.GetInt($"{trainer.trainerBase.trainerName}1") != 1)
                {
                    PlayerPrefs.SetInt($"{trainer.trainerBase.trainerName}1", 1);
                    isActive = true;
                    textBox.SetActive(true);
                    text.gameObject.SetActive(true);
                    NextLine();
                }
                else if (PlayerPrefs.GetInt("Red1") == 1)
                {
                    SceneManager.LoadScene(2);
                }
            }
        }
    }

    void NextLine()
    {
        if (counter < textList.Count)
        {
            SelectionSFX.PlayOneShot(SelectSFX);
            text.text = textList[counter];
            counter++;
        }
        else
        {
            textBox.SetActive(false);
            text.gameObject.SetActive(false);
            isActive = false;
            counter = 0;
            //FindObjectOfType<PlayerMovement>().encounter = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            textBox.SetActive(false);
            text.gameObject.SetActive(false);
            isActive = false;
            counter = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            textBox.SetActive(false);
            text.gameObject.SetActive(false);
            isActive = false;
            counter = 0;
        }
    }
}
