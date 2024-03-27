using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    public LayerMask CardLayer;
    [SerializeField] bool blockPicking;
    public GameObject firstSelected;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 300, CardLayer))
            {
                if (hit.collider.TryGetComponent(out CardController card))
                {
                    if (blockPicking) return;
                    if (card.IsPicked) return;

                    firstSelected = card.gameObject;
                    card.GetPicked();

                    blockPicking = true;
                }
            }
        }
    }




}
