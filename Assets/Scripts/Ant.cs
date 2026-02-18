using UnityEngine;

public class Ant : MonoBehaviour, IBicho
{
    public Transform target;
    public Transform firstPoint;
    public Transform secondPoint;
    Vector3 targetPos;
    Vector3 initialPos;
    [SerializeField] float speed;
    bool going = true;
    [SerializeField] GameObject leafPiece;
    bool scared = false;
    public BichosSpawner spawner;

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
        initialPos = transform.position;
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
            transform.position = targetPos;


            if (going)
            {
                switch (currentPosInPath)
                {
                    case PosInPath.StartAndFirstPoint:
                        targetPos = secondPoint.position;
                        currentPosInPath = PosInPath.FirstPointAndSecond;
                        break;
                    case PosInPath.FirstPointAndSecond:
                        targetPos = target.position;
                        currentPosInPath = PosInPath.SecondPointAndLeaf;
                        break;
                    case PosInPath.SecondPointAndLeaf:
                        targetPos = secondPoint.position;
                        going = false;
                        speed *= 3;
                        targetPos = secondPoint.position;
                        leafPiece.SetActive(true);
                        break;
                }
            }
            else
            {
                switch (currentPosInPath)
                {
                    case PosInPath.StartAndFirstPoint:
                        LeaveScreen(scared);
                        break;
                    case PosInPath.FirstPointAndSecond:
                        currentPosInPath = PosInPath.StartAndFirstPoint;
                        targetPos = initialPos;
                        break;
                    case PosInPath.SecondPointAndLeaf:
                        currentPosInPath = PosInPath.FirstPointAndSecond;
                        targetPos = firstPoint.position;
                        break;
                }
            }
        }
    }

    public void ScareAway()
    {
        if (!going)
        {
            return;
        }
        going = false;
        speed *= 3;
        scared = true;
        switch (currentPosInPath)
        {
            case PosInPath.StartAndFirstPoint:
                targetPos = initialPos;
                break;
            case PosInPath.FirstPointAndSecond:
                targetPos = firstPoint.position;
                break;
            case PosInPath.SecondPointAndLeaf:
                targetPos = secondPoint.position;
                break;
        }
    }

    public void ReceiveObjective(Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, BichosSpawner bichoSpawner)
    {
        firstPoint = leafFirstPoint;
        secondPoint = leafSecondpoint;
        target = leafTarget;
        spawner = bichoSpawner;
    }

    public void LeaveScreen(bool scaredAway)
    {
        spawner.OnBichoLeftScreen(scaredAway, true);
        Destroy(gameObject);
    }
}
