using UnityEngine;

[CreateAssetMenu(fileName = "NewTask", menuName = "Task")]
public class TasksObjectSO : ScriptableObject
{
    public string taskDescription;
    public Sprite taskSprite;
    [SerializeField] private GameObject prefab;
    private int instanceId;

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
        return prefab;
    }

}
