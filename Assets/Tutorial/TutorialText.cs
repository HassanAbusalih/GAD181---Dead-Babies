using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] List<string> beforeCombat = new List<string>();
    [SerializeField] List<string> afterCombat = new List<string>();
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject textBox;
    public SaveLoad saveLoad;
    Collider2D textCollider;
    Collider2D playerCollider;
    bool isActive;
    int counter;
    Trainer trainer;
    public bool tutorialBattle;
    bool intro;

    private void Awake()
    {
        trainer = GetComponent<Trainer>();
        textCollider = GetComponent<Collider2D>();
        playerCollider = FindObjectOfType<PlayerMovement>().GetComponent<Collider2D>();
        saveLoad = FindObjectOfType<SaveLoad>();
    }

    private void Update()
    {
        if (textCollider.IsTouching(playerCollider))
        {
            if (PlayerPrefs.GetInt("TutorialBattle") != 1 && !intro)
            {
                if (!isActive)
                {
                    intro = true;
                    isActive = true;
                    textBox.SetActive(true);
                    text.gameObject.SetActive(true);
                    FindObjectOfType<PlayerMovement>().encounter = true;  //adding this line in pervents movement during dialogue
                }
                DialogueBeforeCombat();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && PlayerPrefs.GetInt("TutorialBattle") != 1)
            {
                PlayerPrefs.SetInt("Tutorial", 1);
                DialogueBeforeCombat();
            }
            else if (!saveLoad.isTrainer && PlayerPrefs.GetInt($"{trainer.trainerBase.trainerName}") == 1 && PlayerPrefs.GetInt($"{trainer.trainerBase.trainerName}1") != 1)
            {
                PlayerPrefs.SetInt($"{trainer.trainerBase.trainerName}1", 1);
                isActive = true;
                textBox.SetActive(true);
                text.gameObject.SetActive(true);
                FindObjectOfType<PlayerMovement>().encounter = true;
                DialogueAfterCombat();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && PlayerPrefs.GetInt($"{trainer.trainerBase.trainerName}1") == 1)
            {
                if (!isActive)
                {
                    isActive = true;
                    textBox.SetActive(true);
                    text.gameObject.SetActive(true);
                    FindObjectOfType<PlayerMovement>().encounter = true;  //adding this line in pervents movement during dialogue
                }
                DialogueAfterCombat();
            }
        }
    }

    void DialogueBeforeCombat()
    {
        if (counter < beforeCombat.Count)
        {
            text.text = beforeCombat[counter];
            counter++;
        }
        else
        {
            trainer.StartTutorial();
            tutorialBattle = true;
            textBox.SetActive(false);
            text.gameObject.SetActive(false);
            isActive = false;
            counter = 0;
            FindObjectOfType<PlayerMovement>().encounter = false;
        }
    }

    void DialogueAfterCombat()
    {
        if (counter < afterCombat.Count)
        {
            text.text = afterCombat[counter];
            counter++;
        }
        else
        {
            textBox.SetActive(false);
            text.gameObject.SetActive(false);
            isActive = false;
            counter = 0;
            FindObjectOfType<PlayerMovement>().encounter = false;
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
