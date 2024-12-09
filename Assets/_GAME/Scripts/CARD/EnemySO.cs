using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]

public class EnemySO : ScriptableObject
{
    public string enemyName; 
    public Sprite enemyIcon; 
    public int health;       
    public int damage;       
    public int level;       

}
