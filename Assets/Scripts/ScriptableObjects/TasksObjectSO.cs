using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTask", menuName = "Task")]
public class TasksObjectSO : ScriptableObject
{
    public string taskDescription;
    public Sprite taskSprite;

    public GameObject prefab;

    public int instanceId;

}
