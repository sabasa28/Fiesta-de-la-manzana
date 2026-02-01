using UnityEngine;

public interface IBicho
{
    public void ScareAway();
    public void ReceiveObjective(Transform appleTarget, Transform flowerTarget, Transform[] leafMidpoint, Transform[] leafTarget);
}
