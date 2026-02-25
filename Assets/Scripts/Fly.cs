using UnityEngine;

public class Fly : MonoBehaviour, IBicho
{
    public Transform target;
    Vector3 targetPos;
    Vector3 initialPos;
    [SerializeField] float speed;
    bool going = true;
    bool turning = false;
    bool turningRight = false;
    float timer = 0.0f;
    [SerializeField] float timeBetweenTurns;
    float rotationSpeed;
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float rotationSpeedIncrementor;
    bool didFullRotation = true;
    [SerializeField] float minAngleToDetectFullRotation;
    [SerializeField] GameObject applePiece;
    bool goingRight = true;
    [SerializeField] SpriteRenderer gfx;
    bool scared = false;
    public BichosSpawner spawner;
    private void Start()
    {
        targetPos = target.transform.position;
        initialPos = transform.position;
        transform.up = (targetPos - initialPos).normalized;
    }
    void Update()
    {
        float movement = speed * Time.deltaTime;
        Vector3 pathLeft = (targetPos - transform.position);
        if (turning)
        {
            transform.position += transform.up * movement;
            rotationSpeed += Time.deltaTime * rotationSpeedIncrementor;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + (turningRight ? 1 : -1) * rotationSpeed * Time.deltaTime);
            if (didFullRotation)
            {
                if (Vector3.Angle(transform.up, (targetPos - transform.position).normalized) > minAngleToDetectFullRotation)
                {
                    didFullRotation = false;
                }
            }
            else
            {
                if (Vector3.Angle(transform.up, (targetPos - transform.position).normalized) < minAngleToDetectFullRotation)
                {
                    transform.up = (targetPos - transform.position).normalized;
                    didFullRotation = true;
                    turning = false;
                }
            }
        }
        else
        {

            timer += Time.deltaTime;
            transform.position += transform.up * movement;
            if (timer > timeBetweenTurns && going)
            {
                turningRight = Random.Range(0, 2) == 0;
                turning = true;
                didFullRotation = true;
                rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
                timer = 0.0f;
            }

        }
        if (movement > pathLeft.magnitude)
        {
            if (!going)
            {
                LeaveScreen(scared);
            }
            transform.position = targetPos;
            targetPos = initialPos;
            applePiece.SetActive(true);
            turning = false;
            transform.up = (targetPos - transform.position).normalized;
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
        scared = true;
        targetPos = initialPos;
        turning = false;
        didFullRotation = true;
        transform.up = (targetPos - transform.position).normalized;
        going = false;
        speed *= 3;
    }
    public void ReceiveObjective(Transform appleFirstPoint, Transform appleSecondPoint, Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, Transform eggLeafTarget, BichosSpawner bichoSpawner)
    {
        target = appleTarget;
        spawner = bichoSpawner;
    }

    void CheckTargetDirAndFlip()
    {
        goingRight = targetPos.x > transform.position.x;
        gfx.flipY = !goingRight;
    }

    public void LeaveScreen(bool scaredAway)
    {
        spawner.OnBichoLeftScreen(scaredAway, true);
        Destroy(gameObject);
    }
}
