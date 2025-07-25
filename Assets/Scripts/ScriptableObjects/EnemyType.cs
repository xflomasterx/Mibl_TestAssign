using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy")]
public class EnemyType : ScriptableObject
{
    [Header("General")]
    public GameObject pref;
    public string speciesName = "Enemy";
    public float height = 1f;

    [Header("Behaviour")]
    public float speed = 1f;
    public float patrolRadius = 2f;
    public float maxFollowDistance = 2f;
    public float playerSeekRadius = 2f;
    public bool isAbleToLeaveSpawn = false;
}
