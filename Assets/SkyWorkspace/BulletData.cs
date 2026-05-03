using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Game/BulletData", order = 1)]
public class BulletSettingsSO : ScriptableObject
{
    public BulletSettings bulletSettings;
}

[System.Serializable]
public struct BulletSettings
{
    public GameObject bulletPrefab;

    public float xSpeed;
    public float ySpeed;
    public float lifetime;
    public int damage;
    public AnimationCurve forwardSpeedCurve;
    public AnimationCurve sidewaysSpeedCurve;



    public static BulletSettings Default => new BulletSettings
    {
        xSpeed = 10f,
        ySpeed = 2f,
        lifetime = 5f,
        damage = 10,
        forwardSpeedCurve = new AnimationCurve(new Keyframe(0, 10), new Keyframe(1, 10)),
        sidewaysSpeedCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0))
    };
}