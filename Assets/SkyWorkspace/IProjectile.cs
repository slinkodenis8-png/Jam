using System;
using UnityEngine;

public interface IProjectile
{
    void Initialize(BulletSettings bulletSettings, Vector3 newDirection);
    void DestroySelf();
}
