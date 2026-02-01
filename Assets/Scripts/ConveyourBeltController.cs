using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConveyourBeltController : MonoBehaviour
{
    [SerializeField] float minNextSpawn;
    [SerializeField] float maxNextSpawn;
    [SerializeField] float xToSpawn;
    [SerializeField] float minYToSpawn;
    [SerializeField] float maxYToSpawn;
    [SerializeField] ConveyourApple prefabToSpawn;
    List<ConveyourApple> spawnedApples = new();
    [SerializeField] float minDistanceBetweenApples;
    [SerializeField] Animator animController;
    [SerializeField] float speed;
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

    private void Start()
    {
        mascot.SayConveyourBeltMinigameIntroText();
        animController.SetFloat("ConveyourSpeed", speed);
    }

    void Update()
    {
        if (spawnTimer > nextSpawnTime)
        {
            SpawnApple();
            nextSpawnTime = Random.Range(minNextSpawn, maxNextSpawn) / speed;
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
                ChangeSpeed(speed * speedRaiseMultiplier);
                speedRaiseTimer = 0.0f;
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
        animController.SetFloat("ConveyourSpeed", speed);
        foreach (ConveyourApple apple in spawnedApples)
        {
            apple.movementSpeed = speed;
        }
        Debug.Log("SPEEEEEEEEEEEEEEED");
    }

    public void RemoveAppleFromList(ConveyourApple appleToRemove, bool remainedInConveyournBelt)
    {
        spawnedApples.Remove(appleToRemove);
        if (appleToRemove.isBadApple == remainedInConveyournBelt)
        {
            if (remainedInConveyournBelt)
            {
                mascot.SayConveyourBeltWrongApplePassedText();
            }
            else
            {
                mascot.SayConveyourBeltGoodAppleDiscardedText();
            }
            LooseLife();
        }
        else if (!appleToRemove.isBadApple)
        {
            applesSaved++;
        }
        Destroy(appleToRemove.gameObject);
    }

    public void LooseLife()
    {
        if (lives <= 0)
        {
            return;
        }
        lives--;
        if (lives == 0)
        {
            mascot.SayConveyourBeltLoseText(applesSaved);
        }
        for (int i = lives; i < LivesRepresentation.Length; i++)
        {
            LivesRepresentation[i].color = Color.gray;
        }
    }
}
