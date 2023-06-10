using UnityEngine;

[CreateAssetMenu(fileName = "NewTask", menuName = "Task")]
public class TasksObjectSO : ScriptableObject
{
    public string taskDescription;
    public Sprite taskSprite;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float taskDuration;
    private int instanceId;
    private ProgressBar taskTimer; // This is set when the task is instantiated 
    private GameObject currSpawnedTask; // The current task that was spawned

    public void SetTaskInstanceID(int id)
    {
        instanceId = id;
    }

    public int GetTaskInstanceID()
    {
        return instanceId;
    }

    public GameObject GetPrefab()
    {
        // This prefab changes! It is instantiated every time the task is spawned!
        return prefab;
    }
    public GameObject GetCurrentObject()
    {
        return currSpawnedTask;
    }

    public void SetCurrentTask(GameObject spawnedTask)
    {
        currSpawnedTask = spawnedTask;
    }
    public ProgressBar GetProgressBar()
    {
        return taskTimer;
    }
    public void SetProgressBar(ProgressBar newProgressBar)
    {
        taskTimer = newProgressBar;
        // Handle callback for tasks that their timer is over
        taskTimer.OnTimerOverEvent += ProgressBar_OnTaskIncomplete;
    }

    private void ProgressBar_OnTaskIncomplete(object sender, System.EventArgs E)
    {
        TaskManager taskManager = TaskManager.Instance;
        taskManager.RemoveIncompleteTask(this);
    }

    public float GetTaskDuration()
    {
        return taskDuration;
    }

    public void SetTaskDuration(float newTaskDuration)
    {
        taskDuration = newTaskDuration;
    }

}
