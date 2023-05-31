using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskManagerSingleUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI taskNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetTasksObjectSO(TasksObjectSO tasksObjectSO)
    {
        taskNameText.text = tasksObjectSO.taskDescription;
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        Transform iconTransform = Instantiate(iconTemplate, iconContainer);
        iconTransform.gameObject.SetActive(true);
        iconTransform.GetComponent<Image>().sprite = tasksObjectSO.taskSprite;
    }
}
