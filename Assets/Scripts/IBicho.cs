using UnityEngine;

public interface IBicho
{
    public void ReceiveObjective(Transform appleFirstPoint, Transform appleSecondPoint, Transform appleTarget, Transform flowerTarget, Transform leafFirstPoint, Transform leafSecondpoint, Transform leafTarget, Transform eggLeafTarget, BichosSpawner bichoSpawner);
    public void ScareAway();

    public void LeaveScreen(bool scaredAway);
}
