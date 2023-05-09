using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public event EventHandler<TasksObjectEventArgs> OnTaskSpawned;
    public class TasksObjectEventArgs : EventArgs
    {
        public TasksObjectSO task;
    }

    public event EventHandler OnTaskCompleted;
    public static TaskManager Instance { get; private set; }
    [SerializeField] private TasksListSO tasksListSO;

    private List<TasksObjectSO> waitingTasksList;

    private int waitingTasksMax = 4;
    private float spawnerTaskTimer;
    private float spawnerTaskTimerMax = 5f;

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

                if (!waitingTasksList.Contains(waitingTask))
                {
                    waitingTasksList.Add(waitingTask);
                    OnTaskSpawned?.Invoke(this, new TasksObjectEventArgs { task = waitingTask });
                }

            }

        }
    }

    public List<TasksObjectSO> GetTasksObjectSOList()
    {
        return waitingTasksList;
    }

    public bool IsTaskCompleted(IInteractable interactable)
    {
        if (interactable != null)
        {
            foreach (TasksObjectSO task in waitingTasksList)
            {
                if (interactable.GetTransform().position == task.prefab.transform.position)
                {
                    waitingTasksList.Remove(task);
                    OnTaskCompleted?.Invoke(this, EventArgs.Empty);
                    return true;
                }
            }
        }
        return false;
    }
}
