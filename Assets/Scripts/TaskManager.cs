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
    public event EventHandler OnTimeFinished;
    public event EventHandler OnPlayerCaught;
    public static TaskManager Instance { get; private set; }
    [SerializeField] private TasksListSO tasksListSO;

    [SerializeField] private List<TasksListSO> tutorialTaskGroup;
    private int currentTaskGroupIndex = 0;
    private bool isNextGroupReady = true;

    private TasksListSO currentTaskGroup;

    private List<TasksObjectSO> waitingTasksList;

    [SerializeField] private int waitingTasksMax = 4;

    [SerializeField] private float minTimeTospawn = 10f;
    [SerializeField] private float maxTimeToSpawn = 15f;
    [SerializeField] private float minTimeForTaskToComplete = 30f;
    [SerializeField] private float maxTimeForTaskToComplete = 40f;
    private float adjustedMinTimeToSpawn;
    private float adjustedMaxTimeToSpawn;
    private float adjustedMinTimeForTaskToComplete;
    private float adjustedMaxTimeForTaskToComplete;
    private float spawnerTaskTimer = 10f;
    [SerializeField] private bool isTutorial;

    [SerializeField] private Clock clock;

    [SerializeField] private FieldOfView fieldOfView;

    private void Awake()
    {
        Instance = this;
        waitingTasksList = new List<TasksObjectSO>();
    }

    private void Start()
    {
        clock.OnTimeOverEvent += clock_OnTimerOver;
        fieldOfView.OnPlayerInFieldOfView += fieldOfView_OnPlayerCaught;
        SetRandomTimeForTask();
    }

    private void SetRandomTimeForTask()
    {
        float randomTime = UnityEngine.Random.Range(adjustedMinTimeToSpawn, adjustedMaxTimeToSpawn);
        spawnerTaskTimer = randomTime;
    }

    private void clock_OnTimerOver(object sender, System.EventArgs E)
    {
        OnTimeFinished?.Invoke(this, EventArgs.Empty);
    }
    private void fieldOfView_OnPlayerCaught(object sender, System.EventArgs E)
    {
        OnPlayerCaught?.Invoke(this, EventArgs.Empty);
    }
    private void Update()
    {
        if (isTutorial && TutorialProgression.Instance.IsBeforeGameTutorialFinished())
        {
            HandleTutorial();

        }
        else
        {
            HandlePlayMode();
            UpdateTaskSpawnTime();
        }
    }


    private void HandleTutorial()
    {
        if (isNextGroupReady && currentTaskGroupIndex < tutorialTaskGroup.Count)
        {
            currentTaskGroup = tutorialTaskGroup[currentTaskGroupIndex];
            AddTasks(currentTaskGroup);
            isNextGroupReady = false;
        }
        // Check if all the tasks in the current group are completed
        if (waitingTasksList.Count == 0)
        {
            currentTaskGroupIndex++;
            isNextGroupReady = true;
        }
    }

    private void HandlePlayMode()
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
                    SetRandomTimeForTask();
                }
            }
        }
    }

    private void UpdateTaskSpawnTime()
    {
        float totalTimeForInGame = clock.GetTotalTime();
        float remainingTimeInGame = clock.GetSecondsRemaining();
        float factor = remainingTimeInGame / totalTimeForInGame;

        // Calculate adjusted values based on the factor
        adjustedMinTimeToSpawn = minTimeTospawn * factor;
        adjustedMaxTimeToSpawn = maxTimeToSpawn * factor;
        adjustedMinTimeForTaskToComplete = minTimeForTaskToComplete * factor;
        adjustedMaxTimeForTaskToComplete = maxTimeForTaskToComplete * factor;

        // Set minimum bounds for spawn times
        adjustedMinTimeToSpawn = Mathf.Max(adjustedMinTimeToSpawn, 4f);
        adjustedMaxTimeToSpawn = Mathf.Max(adjustedMaxTimeToSpawn, 7f);
        adjustedMinTimeForTaskToComplete = Mathf.Max(adjustedMinTimeForTaskToComplete, 12f);
        adjustedMaxTimeForTaskToComplete = Mathf.Max(adjustedMaxTimeForTaskToComplete, 18f);
    }

    public (float, float) GetTimeForTaskToCompleteRange()
    {
        return (adjustedMinTimeForTaskToComplete, adjustedMaxTimeForTaskToComplete);
    }
    private void AddTasks(TasksListSO currentTaskGroup)
    {
        foreach (TasksObjectSO currTask in currentTaskGroup.tasksList)
        {
            TutorialProgression.Instance.HandleNextPartInTutorial();
            waitingTasksList.Add(currTask);
            OnTaskSpawned?.Invoke(this, new ObjectEventArgs { task = currTask });
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

    public bool IsInteractableTaskCompleted(InteractableBase interactable)
    {
        if (interactable != null)
        {
            foreach (TasksObjectSO task in waitingTasksList)
            {
                if (task.GetTaskInstanceID() == interactable.GetGameObjectID())
                {
                    if (waitingTasksList.Count == waitingTasksMax)
                    {
                        SetRandomTimeForTask();
                    }
                    waitingTasksList.Remove(task);
                    OnTaskCompleted?.Invoke(this, new ObjectEventArgs { interactable = interactable, task = task });
                    return true;
                }
            }
        }
        return false;
    }

    public void RegularTaskCompleted(int objectID)
    {
        foreach (TasksObjectSO task in waitingTasksList)
        {
            if (task.GetTaskInstanceID() == objectID)
            {
                waitingTasksList.Remove(task);
                OnTaskCompleted?.Invoke(this, new ObjectEventArgs { task = task });
                return;
            }
        }
    }
}
