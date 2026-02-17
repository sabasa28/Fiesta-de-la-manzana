using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class BichosSpawner : MonoBehaviour
{
    [SerializeField] int minTimeForSpawn;
    [SerializeField] int maxTimeForSpawn;
    float sideAx;
    float sideBx;
    float topsideY;
    float bottomSideY;
    [SerializeField] Transform topLeftCorner;
    [SerializeField] Transform bottomRightCorner;
    [SerializeField] Transform climbingSpawnPos;
    float timer = 0.0f;
    float nextSpawnTime = 0;
    [SerializeField] float firstSpawnTime;
    [SerializeField] GameObject[] posibleFlyingSpawns;
    [SerializeField] GameObject[] posibleClimbingSpawns;
    [SerializeField] Transform appleTrans;
    [SerializeField] Transform flowerTrans;
    [SerializeField] Transform[] leafTrans;
    [SerializeField] Transform[] leafMidpointsTrans;
    [SerializeField] Transform timerRepresentation;
    float fullGameTimer = 0.0f;
    [SerializeField] float gameTime;
    bool gameStarted = false;
    [SerializeField] GameObject mascot;
    [SerializeField] Transform mascotEndscreenPos;
    [SerializeField] GameObject startEndPanel;
    [SerializeField] GameObject startSpecificUI;
    [SerializeField] GameObject endSpecificUI;
    [SerializeField] string endscreenMascotDialogue;
    [SerializeField] TextMeshProUGUI mascotText;
    Vector3 mascotInitialPos;
    [SerializeField] GameObject menuButton;
    bool buttonsEnabled = true;
    Vector3 mascotInitialScale;
    [SerializeField] MascotAnimationController mascotAnimationController;

    void Start()
    {
        ChangeMenuButtonState(true);
        StartCoroutine(DisableButtonsShortly());
        mascotInitialScale = mascot.transform.localScale;
        mascotInitialPos = mascot.transform.position;
        nextSpawnTime = firstSpawnTime;
        sideAx = topLeftCorner.position.x;
        sideBx = bottomRightCorner.position.x;
        topsideY = topLeftCorner.position.y;
        bottomSideY = bottomRightCorner.position.y;
        SetAndEnableStartScreen();
    }

    private void Update()
    {
        if (!gameStarted)
        {
            return;
        }
        timer += Time.deltaTime;
        if (fullGameTimer < gameTime)
        {
            fullGameTimer += Time.deltaTime;
            timerRepresentation.localScale = new Vector3(fullGameTimer / gameTime, 1.0f, 1.0f);
        }
        else
        {
            timerRepresentation.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            EndGame();
        }
        if (timer >= nextSpawnTime)
        {
            timer = 0.0f;
            nextSpawnTime = Random.Range(minTimeForSpawn, maxTimeForSpawn);
            int spawnOption = Random.Range(0, posibleFlyingSpawns.Length + posibleClimbingSpawns.Length);
            IBicho spawnedBicho;
            if (spawnOption < posibleFlyingSpawns.Length)
            {
                spawnedBicho = Instantiate(posibleFlyingSpawns[spawnOption], GetRandomFlyingSpawnPos(), Quaternion.identity).GetComponent<IBicho>();
            }
            else
            {
                spawnOption -= posibleFlyingSpawns.Length;
                spawnedBicho = Instantiate(posibleClimbingSpawns[spawnOption], climbingSpawnPos.position, Quaternion.identity).GetComponent<IBicho>();
            }
            spawnedBicho.ReceiveObjective(appleTrans, flowerTrans, leafMidpointsTrans, leafTrans);
        }
    }

    Vector3 GetRandomFlyingSpawnPos()
    {
        int randomSide = Random.Range(0,3);
        Vector3 spawnPos = Vector3.zero; 
        switch (randomSide)
        {
            case 0:
                spawnPos.x = sideAx;
                spawnPos.y = Random.Range(bottomSideY, topsideY);
                break;
            case 1:
                spawnPos.x = sideBx;
                spawnPos.y = Random.Range(bottomSideY, topsideY);
                break;
            case 2:
                spawnPos.x = Random.Range(sideAx, sideBx);
                spawnPos.y = topsideY;
                break;
        }
        return spawnPos;
    }

    void SetAndEnableStartScreen()
    { 
        startSpecificUI.SetActive(true);
        endSpecificUI.SetActive(false);
        startEndPanel.SetActive(true);
        mascot.transform.position = mascotEndscreenPos.position;
        mascot.transform.localScale = mascotInitialScale * 1.5f;
    }

    void EndGame()
    {
        StartCoroutine(DisableButtonsShortly());
        mascotText.text = endscreenMascotDialogue;
        startEndPanel.SetActive(true);
        endSpecificUI.SetActive(true);
        startSpecificUI.SetActive(false);
        gameStarted = false;
        mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.ClosedEyesHappyHand);
        mascot.transform.position = mascotEndscreenPos.position;
        mascot.transform.localScale = mascotInitialScale * 1.5f;
    }

    public void StartGame()
    {
        if (!buttonsEnabled)
        {
            return;
        }
        ChangeMenuButtonState(false);
        startEndPanel.SetActive(false);
        gameStarted = true;
        mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.Idle);
        mascot.transform.position = mascotInitialPos;
        mascot.transform.localScale = mascotInitialScale;
    }

    public void ReloadScene()
    {
        if (!buttonsEnabled)
        {
            return;
        }
        SceneManager.LoadScene("BichosAttack");
    }

    public void BackToMenu()
    {
        if (!buttonsEnabled)
        {
            return;
        }
        SceneManager.LoadScene("MainMenu");
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
