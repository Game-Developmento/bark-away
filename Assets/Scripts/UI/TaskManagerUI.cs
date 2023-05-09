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
        UpdateVisual(null);
    }

    private void TaskManager_OnTaskSpawned(object sender, TaskManager.TasksObjectEventArgs E)
    {
        UpdateVisual(E.task);
    }
    private void Instance_OnTaskCompleted(object sender, System.EventArgs E)
    {
        UpdateVisual(null);
    }

    private void UpdateVisual(TasksObjectSO task)
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
            if (task != null && task == currTask)
            {
                Instantiate(task.prefab, TaskManager.Instance.transform, true); // CHECK
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
