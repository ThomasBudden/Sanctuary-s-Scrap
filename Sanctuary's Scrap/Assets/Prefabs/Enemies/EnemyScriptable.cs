using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 1)]
public class EnemyScriptable : ScriptableObject
{
    public float attackSpeed;
    public int stoppingDistance;
    public int retreatDistance;
    public int attackDistance;
    public int health;
    public int damage;
    public int movementSpeed;
    public GameObject Projectile;
    public float projSize;
    public float projSpeed;
}
