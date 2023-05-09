using UnityEngine;
using TMPro;
public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    private void Update()
    {
        if (player.GetInteractableObject() != null)
        {
            Show(player.GetInteractableObject());
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
