using UnityEngine;

public interface IBicho
{
    public void ReceiveObjective(Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, BichosSpawner bichoSpawner);
    public void ScareAway();

    public void LeaveScreen(bool scaredAway);
}
