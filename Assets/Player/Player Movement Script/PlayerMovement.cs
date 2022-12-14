using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    BoxCollider2D myBoxCollider2D;
    public Animator anim;
    public Animator battleAnim;
    public SaveLoad saveLoad;
    public float walkSpeed;
    public float runSpeed;
    Vector2 movementOfPlayer;
    public SwitchPokemon switchPokemon;
    public bool encounter;
    float cooldown;
    float cd;
    bool touching;
    [SerializeField] public AudioSource WalkingSound;
    [SerializeField] public AudioSource RunningSound;
    [SerializeField] public AudioClip Walk;
    [SerializeField] public AudioClip Run;
    // Start is called before the first frame update
    private void Start()
    {
        LoadPos();
        saveLoad.Load();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown < 5)
        {
            cooldown += Time.deltaTime;
        }
        if (!switchPokemon.isActive && !encounter && !switchPokemon.quitting)
        {
            movementOfPlayer.x = Input.GetAxisRaw("Horizontal");
            movementOfPlayer.y = Input.GetAxisRaw("Vertical");
            movementOfPlayer = new Vector3(movementOfPlayer.x, movementOfPlayer.y).normalized;
            PlayerAnimation();
            BattleEncounter();
        }
        else
        {
            movementOfPlayer.x = 0;
            movementOfPlayer.y = 0;
            anim.StopPlayback();
        }
        if (cd < 1 && touching)
        {
            cd += Time.deltaTime;
        } 
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            rb2D.MovePosition(rb2D.position + movementOfPlayer * runSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb2D.MovePosition(rb2D.position + movementOfPlayer * walkSpeed * Time.fixedDeltaTime);
        }
    }
    void PlayerAnimation()
    {
        anim.SetFloat("ySpeed", movementOfPlayer.y);
        anim.SetFloat("xSpeed", Mathf.Abs(movementOfPlayer.x));
        if (movementOfPlayer.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        else if (movementOfPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    void BattleEncounter()
    {
        int battleEncounterRNG = Random.Range(0, 1500);
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("1")))
        {
            touching = true;
            if (battleEncounterRNG <= 1 && !encounter && cooldown > 4 && cd >= 1)
            {
                StartEncounter(Random.Range(1, 5));
            }
        }
        else if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("2")))
        {
            touching = true;
            if (battleEncounterRNG <= 1 && !encounter && cooldown > 4 && cd >= 1)
            {
                StartEncounter(Random.Range(5, 8));
            }
        }
        else if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("3")))
        {
            touching = true;
            if (battleEncounterRNG <= 1 && !encounter && cooldown > 4 && cd >= 1)
            {
                StartEncounter(Random.Range(7, 11));
            }
        }
        else if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("4")))
        {
            touching = true;
            if (battleEncounterRNG <= 1 && !encounter && cooldown > 4 && cd >= 1)
            {
                StartEncounter(Random.Range(10, 13));
            }
        }
        else if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("5")))
        {
            touching = true;
            if (battleEncounterRNG <= 1 && !encounter && cooldown > 4 && cd >= 1)
            {
                StartEncounter(Random.Range(12, 15));
            }
        }
        else if (saveLoad.isTrainer && !encounter)
        {
            encounter = true;
            battleAnim.SetBool("Encounter", true);
            saveLoad.PlayerSave();
            saveLoad.EnemySave(0);
            SavePos();
            StartCoroutine(LoadScene());
        }
        else
        {
            touching = false;
        }
    }

    void StartEncounter(int level)
    {
        encounter = true;
        saveLoad.isTrainer = false;
        battleAnim.SetBool("Encounter", true);
        saveLoad.PlayerSave();
        saveLoad.EnemySave(level);
        SavePos();
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(1.1f);
        asyncOperation.allowSceneActivation = true;
    }

    public void SavePos()
    {
        PlayerPrefs.SetFloat("X", transform.position.x);
        PlayerPrefs.SetFloat("Y", transform.position.y);
    }

    void LoadPos()
    {
        transform.position = new Vector2(PlayerPrefs.GetFloat("X"), PlayerPrefs.GetFloat("Y"));
        PlayerPrefs.DeleteKey("X");
        PlayerPrefs.DeleteKey("Y");
    }

    public void WalkingSFX()
    {
        WalkingSound.PlayOneShot(Walk);
    }
}
