using UnityEngine;

public class Ant : MonoBehaviour
{
    public Transform target;
    public Transform midPoint;
    Vector3 targetPos;
    Vector3 initialPos;
    [SerializeField] float speed;
    bool going = true;
    [SerializeField] GameObject leafPiece;
    bool passedMidpoint = false;

    private void Start()
    {
        targetPos = midPoint.position;
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
            if (passedMidpoint)
            {
                if (going)
                {
                    passedMidpoint = false;
                    going = false;
                    speed *= 3;
                    targetPos = midPoint.position;
                    leafPiece.SetActive(true);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                passedMidpoint = true;
                if (going)
                {
                    targetPos = target.position;
                }
                else
                {
                    targetPos = initialPos;
                }
            }
        }
    }
}
