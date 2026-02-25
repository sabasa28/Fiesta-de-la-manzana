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
    [SerializeField] float maxDistToSides;
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
    [SerializeField] GameObject larvaSpawn;
    [SerializeField] Transform appleFirstPointTrans;
    [SerializeField] Transform appleSecondPointTrans;
    [SerializeField] Transform appleTrans;
    [SerializeField] Transform flowerTrans;
    [SerializeField] Transform leafTrans;
    [SerializeField] Transform eggLeafTrans;
    [SerializeField] Transform leafFirstPointTrans;
    [SerializeField] Transform leafSecondPointTrans;
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
    [SerializeField] int badBichosRepeled;
    [SerializeField] int badBichosWhoLeft;
    [SerializeField] int goodBichosRepeled;
    [SerializeField] int goodBichosWhoLeft;

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
            spawnedBicho.ReceiveObjective(appleFirstPointTrans, appleSecondPointTrans, appleTrans, flowerTrans, leafFirstPointTrans, leafSecondPointTrans, leafTrans, eggLeafTrans, this);
        }
    }

    Vector3 GetRandomFlyingSpawnPos()
    {
        int randomSide = Random.Range(0,3);
        Vector3 spawnPos = Vector3.zero; 
        switch (randomSide)
        {
            case 0: //Left
                spawnPos.x = sideAx;
                spawnPos.y = Random.Range(bottomSideY, topsideY);
                break;
            case 1: //right
                spawnPos.x = sideBx;
                spawnPos.y = Random.Range(bottomSideY, topsideY);
                break;
            case 2: //bottom
                if (Random.Range(0, 2) == 0)
                {
                    spawnPos.x = Random.Range(sideAx, sideAx + maxDistToSides);
                }
                else 
                {
                    spawnPos.x = Random.Range(sideBx - maxDistToSides, sideBx);
                }
                spawnPos.y = bottomSideY;
                break;
        }
        return spawnPos;
    }

    public void SpawnLarva(Vector3 posToSpawn)
    {
         IBicho spawnedBicho = Instantiate(larvaSpawn, posToSpawn, Quaternion.identity).GetComponent<IBicho>();
        spawnedBicho.ReceiveObjective(appleFirstPointTrans, appleSecondPointTrans, appleTrans, flowerTrans, leafFirstPointTrans, leafSecondPointTrans, leafTrans, eggLeafTrans, this);
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
        mascotText.text = "Ahuyentaste " + badBichosRepeled + " de " + badBichosWhoLeft + " insectos dañinos para la planta, y dejaste en paz a " + (goodBichosWhoLeft - goodBichosRepeled) + " de " 
            + goodBichosWhoLeft + " insectos no dañinos.";
        if (badBichosWhoLeft == badBichosRepeled && goodBichosRepeled == 0)
        {
            mascotText.text += "\n¡Puntaje perfecto! Felicitaciones.";
            mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.ClosedEyesHappyHand);
        }
        else if (badBichosRepeled > (badBichosWhoLeft / 2) && (goodBichosWhoLeft - goodBichosRepeled) > (goodBichosWhoLeft / 2))
        {
            mascotText.text += "\n¡Buen trabajo!";
            mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.IdleHand);
        }
        else
        {
            mascotText.text += "\n¡Podés hacerlo mejor! Volvé a intentar.";
            mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.Crying);
        }
        startEndPanel.SetActive(true);
        endSpecificUI.SetActive(true);
        startSpecificUI.SetActive(false);
        gameStarted = false;
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

    public void OnBichoLeftScreen(bool scaredAway, bool isBadBicho)
    {
        if (isBadBicho)
        {
            badBichosWhoLeft++;
            if (scaredAway)
            {
                badBichosRepeled++;
            }
        }
        else
        {
            goodBichosWhoLeft++;
            if (scaredAway)
            {
                goodBichosRepeled++;
            }
        }
    }
}
