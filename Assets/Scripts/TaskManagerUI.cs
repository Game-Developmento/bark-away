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

    private void TaskManager_OnTaskSpawned(object sender, System.EventArgs E)
    {
        UpdateVisual();
    }
    private void Instance_OnTaskCompleted(object sender, System.EventArgs E)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == taskTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (TasksObjectSO task in TaskManager.Instance.GetTasksObjectSOList())
        {
            Transform taskTransform = Instantiate(taskTemplate, container);
            taskTransform.gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
