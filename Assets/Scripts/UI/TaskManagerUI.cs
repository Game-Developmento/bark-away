using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform taskTemplate;


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
            IInteractable interactable = spawnedObject.GetComponent<IInteractable>();
            interactable.Initialize();
        }
    }
    private void Instance_OnTaskCompleted(object sender, TaskManager.InteractableObjectEventArgs E)
    {
        UpdateVisual();
        IInteractable interactable = E.interactable;
        if (interactable != null)
        {
            interactable.Cleanup();
        }
    }

    private void UpdateVisual()
    {
        // Clean-up
        foreach (Transform child in container)
        {
            if (child == taskTemplate) continue;
            Destroy(child.gameObject);
        }

        // Display tasks
        foreach (TasksObjectSO currTask in TaskManager.Instance.GetTasksObjectSOList())
        {
            Transform taskTransform = Instantiate(taskTemplate, container);
            taskTransform.gameObject.SetActive(true);
            taskTransform.GetComponent<TaskManagerSingleUI>().SetTasksObjectSO(currTask);
        }
    }
}
