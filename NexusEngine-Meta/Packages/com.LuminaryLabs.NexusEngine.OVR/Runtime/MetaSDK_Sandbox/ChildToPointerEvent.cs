using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class ChildToPointerEvent : PointableUnityEventWrapper
{


    public void DebugPointerData(PointerEvent evt)
    {

        // Hand activeHand = OVRInput.GetActiveHand();
        if (evt.Data != null)
        {
            Debug.Log($"Pointer Data Type: {evt.Data.GetType()}");
        }
        else
        {
            Debug.Log("Pointer Data is null.");
        }
    }

}
