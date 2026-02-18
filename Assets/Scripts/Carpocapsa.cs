using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Carpocapsa : MonoBehaviour, IBicho
{
    public Transform target;
    Vector3 targetPos;
    Vector3 initialPos;
    [SerializeField] float speed;
    bool going = true;
    [SerializeField] GameObject applePiece;
    bool scared = false;
    public BichosSpawner spawner;

    private void Start()
    {
        targetPos = target.transform.position;
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
            if (!going)
            {
                LeaveScreen(scared);
            }
            transform.position = targetPos;
            targetPos = initialPos;
            applePiece.SetActive(true);
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
        going = false;
        speed *= 3;
        scared = true;
    }

    public void ReceiveObjective(Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, BichosSpawner bichoSpawner)
    {
        target = appleTarget;
        spawner = bichoSpawner;
    }

    public void LeaveScreen(bool scaredAway)
    {
        spawner.OnBichoLeftScreen(scaredAway, true);
        Destroy(gameObject);
    }


}
