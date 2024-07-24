using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PlayerActionState { Place, Interact }
public class PlayerActionController : MonoBehaviour
{
    //PlayerActionState state = PlayerActionState.Interact;
    [SerializeField] MouseInteractor interactor;
    [SerializeField] ObjectPlacer placer;
    //public void SetState(PlayerActionState state)
    //{
    //    this.state = state;
    //    placer.gameObject.SetActive(state == PlayerActionState.Place);
    //    interactor.gameObject.SetActive(state == PlayerActionState.Interact);
    //}

    private void Update()
    {
        GameObject placement = InventoryManager.itemSelected.Placement();
        if (placement != null)
        {
            //SetState(PlayerActionState.Place);
            placer.gameObject.SetActive(true);
            placer.CreateGhost(placement);
        }
        else
        {
            placer.gameObject.SetActive(false);
            //SetState(PlayerActionState.Interact);
        }
    }
}
