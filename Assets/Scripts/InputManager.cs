using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    public event System.Action CheckUpperCardEvent;

    [Header("Config")]
    public LayerMask stackLayer;

    [Header("Debug")]
    public StackController firstSelectedStack;
    public StackController secondSelectedStack;
    public bool BlockPicking;

    private void Update()
    {
        if (BlockPicking) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerCheckEvent();
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, 300, stackLayer))
            {
                if (hit.collider.TryGetComponent(out StackController stack))
                {
                    if (stack.IsPicked) return;
                    if (!stack.IsActive) return;

                    if (firstSelectedStack == null)
                    {
                        firstSelectedStack = stack;
                        stack.GetPicked();
                    }
                    else
                    {
                        secondSelectedStack = stack;
                        if (CheckIfStackColorsMatch())
                        {
                            BlockPicking = true;
                            firstSelectedStack.MoveStack(secondSelectedStack);
                        }
                        else
                        {
                            firstSelectedStack.GetReleased();
                            ResetPickingParams();
                        }
                    }
                }
            }
        }

    }

    public void ResetPickingParams()
    {
        BlockPicking = false;
        firstSelectedStack = null;
        secondSelectedStack = null;
    }

    bool CheckIfStackColorsMatch()
    {
        bool isMatch = firstSelectedStack.GetStackColor() == secondSelectedStack.GetStackColor();

        return isMatch;
    }

    public void TriggerCheckEvent()
    {
        CheckUpperCardEvent?.Invoke();
    }

}
