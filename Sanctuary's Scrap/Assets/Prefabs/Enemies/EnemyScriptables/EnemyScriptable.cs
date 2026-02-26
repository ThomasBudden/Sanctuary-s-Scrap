using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 1)]
public class EnemyScriptable : ScriptableObject
{
    public GameObject enemyBody;
    public int enemyTypeId;
    public float size;
    public float attackSpeed;
    public float stoppingDistance;
    public float retreatDistance;
    public float attackDistance;
    public float health;
    public float damage;
    public float movementSpeed;
    public float turnSpeed;
    public GameObject Projectile;
    public float projSize;
    public float projSpeed;
}
