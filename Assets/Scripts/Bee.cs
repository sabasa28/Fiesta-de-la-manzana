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
        if (transform.right.y < 0)
        {
            transform.Rotate(Vector3.up, 180, Space.Self);
        }
        if (movement <= pathLeft.magnitude)
        {
            transform.position += transform.up * movement;
        }
        else
        {
            if (!going)
            {
                Destroy(gameObject);
            }
            transform.position = targetPos;
            targetPos = initialPos;
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
        going = false;
        speed *= 3;
    }

    public void ReceiveObjective(Transform appleTarget, Transform flowerTarget, Transform[] leafMidpoint, Transform[] leafTarget)
    {
        target = flowerTarget;
    }
}
