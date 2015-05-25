using UnityEngine;
using System.Collections;

public class Test_Bullet : Bolt.EntityBehaviour<IBulletState> {
    public float LifeTime;
    public float timer;

    public override void Attached()
    {
        state.BulletTransform.SetTransforms(transform);
    }

    public override void SimulateOwner()
    {
        timer += BoltNetwork.frameDeltaTime;
        if (timer >= LifeTime)
        {
            BoltNetwork.Destroy(gameObject);
        }
    }
}
