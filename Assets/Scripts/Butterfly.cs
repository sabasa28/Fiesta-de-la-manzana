using Unity.VisualScripting;
using UnityEngine;

public class Butterfly : MonoBehaviour, IBicho
{
    Vector2 maxBounds;
    Vector2 minBounds;
    Vector3 targetPos;
    Vector3 initialPos;
    Vector3 pathStart;
    [SerializeField] float speed;
    int flightsLeft = 5;
    bool scared = false;
    public BichosSpawner spawner;
    [SerializeField] SpriteRenderer gfxSprite;

    void Start()
    {
        minBounds = Camera.main.ScreenToWorldPoint(Vector3.zero);
        maxBounds = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
        initialPos = transform.position;
        SetNextTarget();
    }
    private void Update()
    {
        Vector3 pathLeft = (targetPos - transform.position);
        float movement = Time.deltaTime * speed;
        if (movement <= pathLeft.magnitude)
        {
            transform.position += pathLeft.normalized * movement;
        }
        else
        {
            transform.position = targetPos;
            flightsLeft--;
            if (flightsLeft > 0)
            {
                SetNextTarget();
            }
            else if (flightsLeft == 0)
            {
                targetPos = initialPos;
                gfxSprite.flipX = targetPos.x > transform.position.x;
            }
            else
            {
                LeaveScreen(scared);
            }

        }
    }
    void SetNextTarget()
    {
        pathStart = transform.position;
        targetPos = new Vector3(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y), 0.0f);
        gfxSprite.flipX = targetPos.x > transform.position.x;
    }
    public void ScareAway()
    {
        targetPos = initialPos;
        flightsLeft = 0;
        speed *= 3;
        scared = true;
        gfxSprite.flipX = targetPos.x > transform.position.x;
    }

    public void ReceiveObjective(Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, BichosSpawner bichoSpawner)
    {
        spawner = bichoSpawner;
    }

    public void LeaveScreen(bool scaredAway)
    {
        spawner.OnBichoLeftScreen(scaredAway, false);
        Destroy(gameObject);
    }
}
