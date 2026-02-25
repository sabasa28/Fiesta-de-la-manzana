using System.Net;
using UnityEngine;

public class Bee : MonoBehaviour, IBicho
{
    public Transform target;
    Vector3 targetPos;
    Vector3 initialPos;
    [SerializeField] float speed;
    bool going = true;
    [SerializeField] GameObject polenPiece;
    bool scared = false;
    public BichosSpawner spawner;
    [SerializeField] SpriteRenderer gfx;
    Vector3 polenOrigScale;
    Vector3 polenInverseScale;

    private void Start()
    {
        polenOrigScale = polenPiece.transform.localScale;
        polenInverseScale = polenOrigScale;
        polenInverseScale.y = -polenInverseScale.y;
        targetPos = target.transform.position;
        initialPos = transform.position;
        transform.up = (targetPos - transform.position);
        CheckTargetDirAndFlip();
    }
    void Update()
    {
        float movement = speed * Time.deltaTime;
        Vector3 pathLeft = (targetPos - transform.position);
        transform.up = pathLeft.normalized;

        if (movement <= pathLeft.magnitude)
        {
            transform.position += transform.up * movement;
        }
        else
        {
            if (!going)
            {
                LeaveScreen(scared);
            }
            transform.position = targetPos;
            targetPos = initialPos;
            CheckTargetDirAndFlip();
            polenPiece.SetActive(true);
            going = false;
            speed *= 3;
        }
    }
    public void ScareAway()
    {
        if (!going)
        {
            return;
        }
        targetPos = initialPos;
        CheckTargetDirAndFlip();
        going = false;
        speed *= 3;
        scared = true;
    }

    public void ReceiveObjective(Transform appleFirstPoint, Transform appleSecondPoint, Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, Transform eggLeafTarget, BichosSpawner bichoSpawner)
    {
        target = flowerTarget;
        spawner = bichoSpawner;
    }

    void CheckTargetDirAndFlip()
    {
        gfx.flipY = targetPos.x > transform.position.x;
        polenPiece.transform.localScale = targetPos.x > transform.position.x ? polenInverseScale : polenOrigScale;
    }

    public void LeaveScreen(bool scaredAway)
    {
        spawner.OnBichoLeftScreen(scaredAway, false);
        Destroy(gameObject);
    }
}
