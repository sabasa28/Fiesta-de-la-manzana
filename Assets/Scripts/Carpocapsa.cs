using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Carpocapsa : MonoBehaviour
{
    public Transform target;
    Vector3 targetPos;
    Vector3 initialPos;
    [SerializeField] float speed;
    bool going = true;
    [SerializeField] GameObject applePiece;

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
                Destroy(gameObject);
            }
            transform.position = targetPos;
            targetPos = initialPos;
            applePiece.SetActive(true);
            going = false;
            speed *= 3;
        }
    }
}
