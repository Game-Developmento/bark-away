using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public event EventHandler<ObjectEventArgs> OnTaskSpawned;
    public event EventHandler<ObjectEventArgs> OnTaskCompleted;
    public class ObjectEventArgs : EventArgs
    {
        public InteractableBase interactable;
        public TasksObjectSO task;
    }
    public event EventHandler<ObjectEventArgs> OnTaskIncomplete;
    public static TaskManager Instance { get; private set; }
    [SerializeField] private TasksListSO tasksListSO;

    private List<TasksObjectSO> waitingTasksList;

    [SerializeField] private int waitingTasksMax = 4;
    [SerializeField] private float spawnerTaskTimerMax = 5f;
    private float spawnerTaskTimer;

    private void Awake()
    {
        Instance = this;
        waitingTasksList = new List<TasksObjectSO>();
        spawnerTaskTimer = spawnerTaskTimerMax;
    }
    private void Update()
    {
        spawnerTaskTimer -= Time.deltaTime;
        if (spawnerTaskTimer <= 0f)
        {
            if (waitingTasksList.Count < waitingTasksMax)
            {
                TasksObjectSO waitingTask = tasksListSO.tasksList[UnityEngine.Random.Range(0, tasksListSO.tasksList.Count)];
                if (!waitingTasksList.Contains(waitingTask))
                {
                    waitingTasksList.Add(waitingTask);
                    OnTaskSpawned?.Invoke(this, new ObjectEventArgs { task = waitingTask });
                    spawnerTaskTimer = spawnerTaskTimerMax;
                }
            }
        }
    }

    public void RemoveIncompleteTask(TasksObjectSO task)
    {
        if (task != null && waitingTasksList.Contains(task))
        {
            InteractableBase interactable = task.GetCurrentObject().GetComponent<InteractableBase>();
            waitingTasksList.Remove(task);
            OnTaskIncomplete?.Invoke(this, new ObjectEventArgs { interactable = interactable });
        }
    }

    public List<TasksObjectSO> GetTasksObjectSOList()
    {
        return waitingTasksList;
    }

    public bool IsTaskCompleted(InteractableBase interactable)
    {
        if (interactable != null)
        {
            foreach (TasksObjectSO task in waitingTasksList)
            {
                if (task.GetTaskInstanceID() == interactable.GetGameObjectID())
                {
                    waitingTasksList.Remove(task);
                    spawnerTaskTimer = spawnerTaskTimerMax;
                    OnTaskCompleted?.Invoke(this, new ObjectEventArgs { interactable = interactable, task = task });
                    return true;
                }
            }
        }
        return false;
    }
}
