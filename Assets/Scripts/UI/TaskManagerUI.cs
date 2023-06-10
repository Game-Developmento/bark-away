using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TaskManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform taskTemplate;

    [SerializeField] private TextMeshProUGUI points;
    private PointsPopup pointsPopup;
    [SerializeField] private GameOverManagement gameOvermanagement;

    private int currentScore = 0;
    private int defaultPoints = 300;
    private int numOfTasksCompleted = 0;
    private int fastestTaskCompleted = 100;

    private Dictionary<TasksObjectSO, Transform> taskObjectMap = new Dictionary<TasksObjectSO, Transform>();

    private void Awake()
    {
        taskTemplate.gameObject.SetActive(false);
        pointsPopup = GetComponentInChildren<PointsPopup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        TaskManager.Instance.OnTaskSpawned += TaskManager_OnTaskSpawned;
        TaskManager.Instance.OnTaskCompleted += TaskManager_OnTaskCompleted;
        TaskManager.Instance.OnTaskIncomplete += TaskManager_OnTaskIncomplete;
        TaskManager.Instance.OnTimeFinished += TaskManager_OnTimeFinished;

        UpdateVisual();
    }
    private void TaskManager_OnTimeFinished(object sender, EventArgs e)
    {
        string sceneToLoad = "Time's up";
        gameOvermanagement.GameOver(currentScore, numOfTasksCompleted, fastestTaskCompleted, sceneToLoad);
    }
    private void TaskManager_OnTaskSpawned(object sender, TaskManager.ObjectEventArgs E)
    {
        UpdateVisual();
        TasksObjectSO newTaskToSpawn = E.task;
        GameObject prefabToInitialize = newTaskToSpawn.GetPrefab();
        if (prefabToInitialize != null)
        {
            GameObject spawnedObject = Instantiate(prefabToInitialize);
            newTaskToSpawn.SetTaskInstanceID(spawnedObject.GetInstanceID());
            newTaskToSpawn.SetCurrentTask(spawnedObject);
            if (spawnedObject.TryGetComponent(out InteractableBase interactable))
            {
                interactable.Initialize();
            }
        }
    }
    private void TaskManager_OnTaskCompleted(object sender, TaskManager.ObjectEventArgs E)
    {
        UpdateVisual();
        InteractableBase interactable = E.interactable;
        TasksObjectSO taskCompleted = E.task;
        if (taskCompleted != null)
        {
            ProgressBar progressbar = taskCompleted.GetProgressBar();
            int timeLeftForTask = progressbar.GetTimeLeft();
            float totalTimeforTask = progressbar.GetTotalTime();
            float timeRatio = timeLeftForTask / totalTimeforTask;
            int scoreToAdd = (int)(timeRatio * defaultPoints);
            UpdatePoints(scoreToAdd);
            numOfTasksCompleted += 1;
            // updating the fastest task completed so far
            int currTaskTimeCompleted = (int)(totalTimeforTask - timeLeftForTask);
            UpdateFastestTaskCompleted(currTaskTimeCompleted);
        }
        if (interactable != null)
        {
            interactable.Cleanup();
        }
    }

    private void TaskManager_OnTaskIncomplete(object sender, TaskManager.ObjectEventArgs E)
    {
        UpdateVisual();
        InteractableBase interactable = E.interactable;
        if (interactable != null)
        {
            interactable.Cleanup();
        }
    }

    private void RemoveTasks(List<TasksObjectSO> currTaskList)
    {
        List<TasksObjectSO> tasksToRemove = new List<TasksObjectSO>();

        // saves Dict keys to clean up unnecessary tasks
        foreach (TasksObjectSO taskObject in taskObjectMap.Keys)
        {
            if (!currTaskList.Contains(taskObject))
            {

                tasksToRemove.Add(taskObject);
            }
        }
        // removing tasks
        foreach (var taskObject in tasksToRemove)
        {
            Transform taskTransform = taskObjectMap[taskObject];
            taskObjectMap.Remove(taskObject);
            Destroy(taskTransform.gameObject);
        }
    }

    private void DisplayTasks(List<TasksObjectSO> currTaskList)
    {
        // Display tasks
        foreach (TasksObjectSO currTask in currTaskList)
        {
            Transform taskTransform;
            ProgressBar progressBar;
            if (taskObjectMap.ContainsKey(currTask))
            {
                taskTransform = taskObjectMap[currTask];
            }
            else
            {
                taskTransform = Instantiate(taskTemplate, container);
                taskObjectMap.Add(currTask, taskTransform);
                progressBar = taskTransform.GetComponentInChildren<ProgressBar>();
                currTask.SetProgressBar(progressBar);
            }
            taskTransform.gameObject.SetActive(true);
            taskTransform.GetComponent<TaskManagerSingleUI>().SetTasksObjectSO(currTask);
            progressBar = currTask.GetProgressBar();
            progressBar.SetDuration(currTask.GetTaskDuration());
            if (progressBar != null && !progressBar.IsCurrentlyInProgress())
            {
                progressBar.BeginProgress();
            }
        }
    }

    private void UpdatePoints(int scoreToAdd)
    {
        currentScore += scoreToAdd;

        if (pointsPopup != null)
        {
            pointsPopup.ShowScorePopup(scoreToAdd);
        }
        if (points != null)
        {
            points.text = "Points: " + currentScore.ToString();
        }

    }
    private void UpdateVisual()
    {
        List<TasksObjectSO> currTaskList = TaskManager.Instance.GetTasksObjectSOList();
        RemoveTasks(currTaskList);
        DisplayTasks(currTaskList);
    }

    private void UpdateFastestTaskCompleted(int currTaskTimeCompleted)
    {
        if (currTaskTimeCompleted < fastestTaskCompleted)
        {
            fastestTaskCompleted = currTaskTimeCompleted;
        }
    }

}
