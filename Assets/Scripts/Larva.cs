using System.Collections;
using UnityEngine;

public class Larva : MonoBehaviour, IBicho
{
    public Transform target;
    public Transform firstPoint;
    public Transform secondPoint;
    Vector3 targetPos;
    [SerializeField] float speed;
    bool falling = false;
    public BichosSpawner spawner;
    [SerializeField] float fallingSpeed;
    [SerializeField] float timeBeforeDespawn;
    bool bornToBeAlive = false;
    [SerializeField] float timeToBeBorn;
    float bornTimer = 0.0f;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer gfx;
    public enum PosInPath // not my cleanest
    {
        StartAndFirstPoint,
        FirstPointAndSecond,
        SecondPointAndLeaf
    }
    PosInPath currentPosInPath = PosInPath.StartAndFirstPoint;

    private void Start()
    {
        targetPos = firstPoint.position;
    }
    void Update()
    {
        if (falling)
        {
            transform.position += Vector3.down * fallingSpeed *  Time.deltaTime;
            return;
        }
        if (!bornToBeAlive)
        {
            if (bornTimer > timeToBeBorn)
            {
                bornToBeAlive = true;
                animator.SetTrigger("wasBorn");
                CheckTargetDirAndFlip();
            }
            else
            {
                bornTimer += Time.deltaTime;
            }
            return;
        }
        float movement = speed * Time.deltaTime;
        Vector3 pathLeft = (targetPos - transform.position);
        transform.up = pathLeft.normalized;
        if (movement <= pathLeft.magnitude)
        {
            transform.position += transform.up * movement;
        }
        else
        {
            transform.position = targetPos;

            switch (currentPosInPath)
            {
                case PosInPath.StartAndFirstPoint:
                    targetPos = secondPoint.position;
                    currentPosInPath = PosInPath.FirstPointAndSecond;
                    CheckTargetDirAndFlip();
                    break;
                case PosInPath.FirstPointAndSecond:
                    targetPos = target.position;
                    currentPosInPath = PosInPath.SecondPointAndLeaf;
                    CheckTargetDirAndFlip();
                    break;
                case PosInPath.SecondPointAndLeaf:
                    targetPos = secondPoint.position;
                    LeaveScreen(falling);
                    break;
            }
        }
    }

    public void ScareAway()
    {
        if (falling || !bornToBeAlive)
        {
            return;
        }
        falling = true;
        animator.SetTrigger("isFalling");
        StartCoroutine(DespawnAfterTime());
    }

    public void ReceiveObjective(Transform appleFirstPoint, Transform appleSecondPoint, Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, Transform eggLeafTarget, BichosSpawner bichoSpawner)
    {
        firstPoint = appleFirstPoint;
        secondPoint = appleSecondPoint;
        target = appleTarget;
        spawner = bichoSpawner;
    }

    public void LeaveScreen(bool scaredAway)
    {
        spawner.OnBichoLeftScreen(scaredAway, true);
        Destroy(gameObject);
    }

    void CheckTargetDirAndFlip()
    {
        gfx.flipY = targetPos.x > transform.position.x;
    }

    IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(timeBeforeDespawn);
        LeaveScreen(falling);
    }
}
