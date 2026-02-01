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
                Destroy(gameObject);
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
        targetPos = initialPos;
        turning = false;
        didFullRotation = true;
        transform.up = (targetPos - transform.position).normalized;
        going = false;
        speed *= 3;
    }
    public void ReceiveObjective(Transform appleTarget, Transform flowerTarget, Transform[] leafMidpoint, Transform[] leafTarget)
    {
        target = appleTarget;
    }
}
