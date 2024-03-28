using System.Collections;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LayerController nextLayerController;

    public void CheckActiveStacks()
    {
        IEnumerator Routine()
        {
            yield return null;
            bool noActiveStacks = true;

            for (int i = 0; i < transform.childCount; i++)
            {
                StackController stack = transform.GetChild(i).GetComponent<StackController>();

                if (stack.HasCards())
                    noActiveStacks = false;
            }

            if (noActiveStacks)
                ActivateNextLayer();
        }
        StartCoroutine(Routine());
    }

    void ActivateNextLayer()
    {
        if (nextLayerController != null)
            nextLayerController.GetActivated();
    }

    public void GetActivated()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            StackController stack = transform.GetChild(i).GetComponent<StackController>();

            stack.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
