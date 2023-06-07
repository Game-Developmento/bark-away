using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform taskTemplate;

    private Dictionary<TasksObjectSO, Transform> taskObjectMap = new Dictionary<TasksObjectSO, Transform>();

    private void Awake()
    {
        taskTemplate.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        TaskManager.Instance.OnTaskSpawned += TaskManager_OnTaskSpawned;
        TaskManager.Instance.OnTaskCompleted += Instance_OnTaskCompleted;
        UpdateVisual();
    }

    private void TaskManager_OnTaskSpawned(object sender, TaskManager.TasksObjectEventArgs E)
    {
        UpdateVisual();
        TasksObjectSO newTaskToSpawn = E.task;
        GameObject prefabToInitialize = newTaskToSpawn.GetPrefab();
        if (prefabToInitialize != null)
        {
            GameObject spawnedObject = Instantiate(prefabToInitialize);
            newTaskToSpawn.SetTaskInstanceID(spawnedObject.GetInstanceID());
            InteractableBase interactable = spawnedObject.GetComponent<InteractableBase>();
            interactable.Initialize();
        }
    }
    private void Instance_OnTaskCompleted(object sender, TaskManager.InteractableObjectEventArgs E)
    {
        UpdateVisual();
        InteractableBase interactable = E.interactable;
        if (interactable != null)
        {
            interactable.Cleanup();
        }
    }

    private void UpdateVisual()
    {
        List<TasksObjectSO> currTaskList = TaskManager.Instance.GetTasksObjectSOList();
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

        // Display tasks
        foreach (TasksObjectSO currTask in currTaskList)
        {
            Transform taskTransform;
            if (taskObjectMap.ContainsKey(currTask))
            {
                taskTransform = taskObjectMap[currTask];
            }
            else
            {
                taskTransform = Instantiate(taskTemplate, container);
                taskObjectMap.Add(currTask, taskTransform);
            }
            taskTransform.gameObject.SetActive(true);
            taskTransform.GetComponent<TaskManagerSingleUI>().SetTasksObjectSO(currTask);
            ProgressBar progressBar = taskTransform.GetComponentInChildren<ProgressBar>();
            if (progressBar != null && !progressBar.IsCurrentlyInProgress())
            {
                progressBar.BeginProgress();
            }
        }
    }
}
