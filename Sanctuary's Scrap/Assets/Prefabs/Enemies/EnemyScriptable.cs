using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 1)]
public class EnemyScriptable : ScriptableObject
{
    public int range;
    public float attackSpeed;
    public int stoppingDistance;
    public int retreatDistance;
    public int health;
    public int damage;
    public int movementSpeed;
}
