using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform taskTemplate;

    [SerializeField] private TextMeshProUGUI points;
    [SerializeField] private PointsPopup pointsPopup;

    private int currentScore = 0;
    private int defaultPoints = 15;

    private Dictionary<TasksObjectSO, Transform> taskObjectMap = new Dictionary<TasksObjectSO, Transform>();

    private void Awake()
    {
        taskTemplate.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        TaskManager.Instance.OnTaskSpawned += TaskManager_OnTaskSpawned;
        TaskManager.Instance.OnTaskCompleted += TaskManager_OnTaskCompleted;
        TaskManager.Instance.OnTaskIncomplete += TaskManager_OnTaskIncomplete;
        UpdateVisual();
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
            InteractableBase interactable = spawnedObject.GetComponent<InteractableBase>();
            interactable.Initialize();
        }
    }
    private void TaskManager_OnTaskCompleted(object sender, TaskManager.ObjectEventArgs E)
    {
        UpdateVisual();
        InteractableBase interactable = E.interactable;
        TasksObjectSO taskCompleted = E.task;
        int timeLeftForTask = taskCompleted.GetProgressBar().GetCurrentTime();
        int scoreToAdd = timeLeftForTask * defaultPoints;
        UpdatePoints(scoreToAdd);

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
            if (progressBar != null && !progressBar.IsCurrentlyInProgress())
            {
                progressBar.BeginProgress();
            }
        }
    }

    private void UpdatePoints(int scoreToAdd)
    {
        pointsPopup.ShowScorePopup(scoreToAdd);
        currentScore += scoreToAdd;
        points.text = "Points: " + currentScore.ToString();

    }
    private void UpdateVisual()
    {
        List<TasksObjectSO> currTaskList = TaskManager.Instance.GetTasksObjectSOList();
        RemoveTasks(currTaskList);
        DisplayTasks(currTaskList);
    }
}
