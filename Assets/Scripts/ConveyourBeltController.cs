using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConveyourBeltController : MonoBehaviour
{
    [SerializeField] float minNextSpawn;
    [SerializeField] float maxNextSpawn;
    [SerializeField] float xToSpawn;
    float minYToSpawn;
    float maxYToSpawn;
    [SerializeField] Transform minYToSpawnTrans;
    [SerializeField] Transform maxYToSpawnTrans;
    [SerializeField] ConveyourApple prefabToSpawn;
    List<ConveyourApple> spawnedApples = new();
    [SerializeField] float minDistanceBetweenApples;
    [SerializeField] Animator animController;
    [SerializeField] float speed;
    float spawnSpeed;
    float initialSpeed;
    float spawnTimer = 0.0f;
    float speedRaiseTimer = 0.0f;
    float nextSpawnTime = 0.0f;
    bool raisingSpeed = false;
    [SerializeField] float speedRaiseMultiplier;
    [SerializeField] float timeToStartRaisingSpeed;
    [SerializeField] float timeBetweenSpeedRaising;
    [SerializeField] int lives = 3;
    [SerializeField] Image[] LivesRepresentation;
    [SerializeField] Mascot mascot;
    int applesSaved = 0;
    bool gameStarted = false;
    [SerializeField] Transform mascotEndscreenPos;
    [SerializeField] GameObject startEndPanel;
    [SerializeField] GameObject startSpecificUI;
    [SerializeField] GameObject endSpecificUI;
    [SerializeField] TextMeshProUGUI mascotText;
    [SerializeField] string conveyourBeltMinigameIntroText;
    [SerializeField] string conveyourBeltLoseText;
    Vector3 mascotInitialScale;
    Vector3 mascotInitialPos;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] InputAction touchPosAction;
    [SerializeField] InputAction touchPosAction1;
    [SerializeField] InputAction touchPosAction2;
    [SerializeField] InputAction touchPosAction3;
    [SerializeField] InputAction touchPosAction4;
    [SerializeField] InputAction touchPosAction5;
    [SerializeField] InputAction touchPosAction6;
    [SerializeField] InputAction touchPosAction7;
    [SerializeField] InputAction touchPosAction8;
    [SerializeField] InputAction touchPosAction9;
    [SerializeField] InputAction touchPressedAction;
    Touch touchDebug;
    [SerializeField] GameObject menuButton;
    bool buttonsEnabled = true;
    [SerializeField] int timesSpedUp;
    [SerializeField] int maxTimesToSpeedUp;

    private void Awake()
    {
        touchPressedAction = playerInput.actions["TouchPressed"];
        touchPosAction = playerInput.actions["TouchPosition"];
        touchPosAction1 = playerInput.actions["TouchPosition1"];
        touchPosAction2 = playerInput.actions["TouchPosition2"];
        touchPosAction3 = playerInput.actions["TouchPosition3"];
        touchPosAction4 = playerInput.actions["TouchPosition4"];
        touchPosAction5 = playerInput.actions["TouchPosition5"];
        touchPosAction6 = playerInput.actions["TouchPosition6"];
        touchPosAction7 = playerInput.actions["TouchPosition7"];
        touchPosAction8 = playerInput.actions["TouchPosition8"];
        touchPosAction9 = playerInput.actions["TouchPosition9"];


    }
    private void OnEnable()
    {
        touchPressedAction.performed += TouchPressedEvent;
    }
    private void OnDisable()
    {
        touchPressedAction.performed -= TouchPressedEvent;
    }
    void TouchPressedEvent(InputAction.CallbackContext context) //esta al pedo pero me dio miedo sacarlo a ultimo momento
    {
        float value = context.ReadValue<float>();
    }
    private void Start()
    {
        spawnSpeed = speed;
        ChangeMenuButtonState(true);
        mascotInitialScale = mascot.transform.localScale;
        mascotInitialPos = mascot.transform.position;
        initialSpeed = speed;
        minYToSpawn = minYToSpawnTrans.position.y;
        maxYToSpawn = maxYToSpawnTrans.position.y;
        animController.SetFloat("ConveyourSpeed", speed);
        SetAndEnableStartScreen();
    }

    void Update()
    {
        
        if (!gameStarted)
        {
            return;
        }
        if (spawnTimer > nextSpawnTime)
        {
            SpawnApple();
            nextSpawnTime = Random.Range(minNextSpawn, maxNextSpawn) / spawnSpeed;
            spawnTimer = 0.0f;
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }
        speedRaiseTimer += Time.deltaTime;
        if (!raisingSpeed)
        {
            if (speedRaiseTimer > timeToStartRaisingSpeed)
            {
                raisingSpeed = true;
            }
        }
        else
        {
            if (speedRaiseTimer > timeBetweenSpeedRaising)
            {
                speedRaiseTimer = 0.0f;
                if (timesSpedUp < maxTimesToSpeedUp)
                {
                    ChangeSpeed(speed * speedRaiseMultiplier);
                    timesSpedUp++;
                }
                else
                {
                    spawnSpeed *= speedRaiseMultiplier;
                }
            }
        }
    }

    void SpawnApple()
    {
        float YToSpawn = Random.Range(minYToSpawn, maxYToSpawn);
        Vector3 posToSpawn = new Vector3(xToSpawn, YToSpawn, 0.0f);
        if ((spawnedApples.Count > 0) && (Vector2.Distance(posToSpawn, spawnedApples[spawnedApples.Count - 1].transform.position) < minDistanceBetweenApples))
        {
            //spawnearia demasiado cerca de la anterior, podria colisionar con la penultima pero creo que no da el tiempo
            return;
        }
        ConveyourApple spawnedApple = (Instantiate(prefabToSpawn, posToSpawn, Quaternion.identity));
        if (spawnedApple)
        {
            spawnedApple.movementSpeed = speed;
            spawnedApple.conveyourBeltController = this;
            spawnedApples.Add(spawnedApple);
        }
    }

    void ChangeSpeed(float newSpeed)
    { 
        speed = newSpeed;
        spawnSpeed = newSpeed;
        animController.SetFloat("ConveyourSpeed", speed);
        foreach (ConveyourApple apple in spawnedApples)
        {
            apple.movementSpeed = speed;
        }
    }

    public void RemoveAppleFromList(ConveyourApple appleToRemove, bool remainedInConveyournBelt)
    {
        spawnedApples.Remove(appleToRemove);
        if (appleToRemove.isBadApple == remainedInConveyournBelt)
        {
            LooseLife();
            if (lives > 0)
            {
                if (remainedInConveyournBelt)
                {
                    mascot.SayConveyourBeltWrongApplePassedText();
                }
                else
                {
                    mascot.SayConveyourBeltGoodAppleDiscardedText();
                }
            }
        }
        else if (!appleToRemove.isBadApple)
        {
            applesSaved++;
        }
        Destroy(appleToRemove.gameObject);
    }

    public void LooseLife()
    {
        if (!gameStarted)
        {
            return;
        }
        if (lives <= 0)
        {
            return;
        }
        lives--;
        if (lives == 0)
        {
            EndGame();
        }
        for (int i = lives; i < LivesRepresentation.Length; i++)
        {
            LivesRepresentation[i].color = Color.gray;
        }
    }
    public void ResetLives()
    {
        lives = 3;
        for (int i = 0; i < LivesRepresentation.Length; i++)
        {
            LivesRepresentation[i].color = Color.white;
        }
    }

    void SetAndEnableStartScreen()
    {
        StartCoroutine(DisableButtonsShortly());
        mascot.ShutUp();
        mascotText.text = conveyourBeltMinigameIntroText;
        startSpecificUI.SetActive(true);
        endSpecificUI.SetActive(false);
        startEndPanel.SetActive(true);
        mascot.transform.position = mascotEndscreenPos.position;
        mascot.transform.localScale = mascotInitialScale * 1.5f;
    }

    void EndGame()
    {
        Mascot.ReactionToScore reactionToScore = Mascot.ReactionToScore.Happy;
        StartCoroutine(DisableButtonsShortly());
        mascotText.text = conveyourBeltLoseText.Replace("X", applesSaved.ToString());
        if (applesSaved < 5)
        {
            reactionToScore = Mascot.ReactionToScore.Sad;
        }
        else
        { 
            mascotText.text += "\n¡Buen trabajo!";
        }
        if (applesSaved > PlayerPrefs.GetInt("ConveyourHighscore", 0))
        {
            PlayerPrefs.SetInt("ConveyourHighscore", applesSaved);
            mascotText.text += "\n¡Conseguiste un nuevo récord!";
            reactionToScore = Mascot.ReactionToScore.VeryHappy;
        }
        else
        {
            mascotText.text += "\nEl récord actual es de " + PlayerPrefs.GetInt("ConveyourHighscore", 0) + ".";
        }
        startEndPanel.SetActive(true);
        endSpecificUI.SetActive(true);
        startSpecificUI.SetActive(false);
        mascot.ShutUp();
        mascot.SetEndGameFace(reactionToScore);
        gameStarted = false;
        mascot.transform.position = mascotEndscreenPos.position;
        mascot.transform.localScale = mascotInitialScale * 1.5f;
        foreach (ConveyourApple apple in spawnedApples)
        {
            Destroy(apple.gameObject);
        }
        spawnedApples.Clear();
    }

    public void StartGame()
    {
        if (!buttonsEnabled)
        {
            return;
        }
        mascot.SayConveyourBeltMinigameLivesText();
        startEndPanel.SetActive(false);
        ChangeMenuButtonState(false);
        gameStarted = true;
        mascot.transform.position = mascotInitialPos;
        mascot.transform.localScale = mascotInitialScale;
    }

    public void PlayAgain()
    {
        if (!buttonsEnabled)
        {
            return;
        }
        ResetLives();
        applesSaved = 0;
        spawnTimer = 0.0f;
        speedRaiseTimer = 0.0f;
        timesSpedUp = 0;
        raisingSpeed = false;
        ChangeSpeed(initialSpeed);
        StartGame();
    }

    public void BackToMenu()
    {
        if (!buttonsEnabled)
        {
            return;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public Vector2 ReturnTouchPos(int touchId = -1) //not my cleanest, not my dirtiest
    {
        if (touchId == -1)
        {
            return Mouse.current.position.value;
        }
        if (touchPosAction.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction.ReadValue<TouchState>().position;
        }
        if (touchPosAction1.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction1.ReadValue<TouchState>().position;
        }
        if (touchPosAction2.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction2.ReadValue<TouchState>().position;
        }
        if (touchPosAction3.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction3.ReadValue<TouchState>().position;
        }
        if (touchPosAction4.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction4.ReadValue<TouchState>().position;
        }
        if (touchPosAction5.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction5.ReadValue<TouchState>().position;
        }
        if (touchPosAction6.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction6.ReadValue<TouchState>().position;
        }
        if (touchPosAction7.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction7.ReadValue<TouchState>().position;
        }
        if (touchPosAction8.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction8.ReadValue<TouchState>().position;
        }
        if (touchPosAction9.ReadValue<TouchState>().touchId == touchId)
        {
            return touchPosAction9.ReadValue<TouchState>().position;
        }
        return Vector2.zero;
    }

    void ChangeMenuButtonState(bool active)
    {
        menuButton.SetActive(active);
    }

    IEnumerator DisableButtonsShortly()
    {
        buttonsEnabled = false;
        yield return new WaitForSeconds(0.4f);
        buttonsEnabled = true;
    }
}
