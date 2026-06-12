using UnityEngine;

public class SwitchBoard : MonoBehaviour
{
    private bool RisOn = false;

    [SerializeField] private GameObject ObjectsToActivate;
    [SerializeField] private GameObject ObjectsToDeactivate;
    void Update()
    {
        if (RisOn)
        {
            ObjectsToActivate.SetActive(true);
            ObjectsToDeactivate.SetActive(false);
        }
        else if (!RisOn)
        {
            ObjectsToActivate.SetActive(false);
            ObjectsToDeactivate.SetActive(true);
        }
    }

    public void Switch()
    {
        if (RisOn)
        {
            RisOn = false;
        }
        else if (!RisOn)
        {
            RisOn = true;
        }
    }
}
