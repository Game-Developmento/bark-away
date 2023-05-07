using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TaskManagerSingleUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI taskNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;


    public void SetTasksObjectSO(TasksObjectSO tasksObjectSO)
    {
        taskNameText.text = tasksObjectSO.taskDescription;
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // we need to set here the icon, need to add icons and check how to do it in the video.
    }
}
