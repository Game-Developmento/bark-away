using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TasksListSO tasksListSO;
    private float spawnerTaskTimer;
    private float spawnerTaskTimerMax = 4f;

    private void Update()
    {
        spawnerTaskTimer -= Time.deltaTime;
        if (spawnerTaskTimer <= 0f)
        {
            spawnerTaskTimer = spawnerTaskTimerMax;
        }
    }
}
