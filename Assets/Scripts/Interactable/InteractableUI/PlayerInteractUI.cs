using UnityEngine;
using TMPro;
public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    private void Update()
    {
        IInteractable currInteractableObject = player.GetInteractableObject();
        if (currInteractableObject != null)
        {
            Show(currInteractableObject);
        }
        else
        {
            Hide();
        }
    }
    private void Show(IInteractable interactable)
    {
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = interactable.GetInteractText();
    }

    private void Hide()
    {
        containerGameObject.SetActive(false);
    }
}
