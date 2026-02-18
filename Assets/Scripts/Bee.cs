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

    private void Start()
    {
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

    public void ReceiveObjective(Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, BichosSpawner bichoSpawner)
    {
        target = flowerTarget;
        spawner = bichoSpawner;
    }

    void CheckTargetDirAndFlip()
    {
        gfx.flipY = targetPos.x > transform.position.x;
    }

    public void LeaveScreen(bool scaredAway)
    {
        spawner.OnBichoLeftScreen(scaredAway, false);
        Destroy(gameObject);
    }
}
