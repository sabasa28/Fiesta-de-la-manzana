using UnityEngine;

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
    [SerializeField] Transform gfxSprite;
    Vector3 originalGFXScale;
    Vector3 reversedGFXScale;

    private void Start()
    {
        originalGFXScale = gfxSprite.localScale;
        reversedGFXScale = new Vector3 (-originalGFXScale.x, originalGFXScale.y, originalGFXScale.z);
        targetPos = target.transform.position;
        gfxSprite.localScale = targetPos.x < transform.position.x? originalGFXScale : reversedGFXScale;
        initialPos = transform.position;
    }
    void Update()
    {
        float movement = speed * Time.deltaTime;
        Vector3 pathLeft = (targetPos - transform.position);
        if (movement <= pathLeft.magnitude)
        {
            transform.position += pathLeft.normalized * movement;
        }
        else
        {
            if (!going)
            {
                LeaveScreen(scared);
            }
            transform.position = targetPos;
            targetPos = initialPos;
            gfxSprite.localScale = targetPos.x < transform.position.x ? originalGFXScale : reversedGFXScale;
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
        gfxSprite.localScale = targetPos.x < transform.position.x ? originalGFXScale : reversedGFXScale;
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
