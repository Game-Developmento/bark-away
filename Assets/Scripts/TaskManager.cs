using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    public event EventHandler OnTaskSpawned;
    public event EventHandler OnTaskCompleted;
    public static TaskManager Instance { get; private set; }
    [SerializeField] private TasksListSO tasksListSO;

    private List<TasksObjectSO> waitingTasksList;

    private int waitingTasksMax = 4;
    private float spawnerTaskTimer;
    private float spawnerTaskTimerMax = 4f;

    private void Awake()
    {
        Instance = this;
        waitingTasksList = new List<TasksObjectSO>();

    }
    private void Update()
    {
        spawnerTaskTimer -= Time.deltaTime;
        if (spawnerTaskTimer <= 0f)
        {
            spawnerTaskTimer = spawnerTaskTimerMax;
            if (waitingTasksList.Count < waitingTasksMax)
            {
                TasksObjectSO waitingTask = tasksListSO.tasksList[UnityEngine.Random.Range(0, tasksListSO.tasksList.Count)];
                Debug.Log(waitingTask.name);
                waitingTasksList.Add(waitingTask);

                OnTaskSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public List<TasksObjectSO> GetTasksObjectSOList()
    {
        return waitingTasksList;
    }
}
