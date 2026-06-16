using System.Collections.Generic;
using UnityEngine;

public class IRelayReceiver : MonoBehaviour, IReceiver
{
    [TextArea]
    [Header("Developer message:")]
    public string Notes = "This script can be used to pass along a signal from a transmitter to multiple receivers. If a transmitter targets this receiver, each receiver in the 'TargetReceivers list will be activated simultaneously. \n Good for resetting puzzles and traps.";

    [Tooltip("The objects which have Receivers attached which this relay will pass the signal to.")]
    [SerializeField] List<GameObject> receiverObjects = new List<GameObject>();

    [SerializeField] List<IReceiver> targetReceivers = new List<IReceiver> ();

    private void Start()
    {
        foreach (GameObject rObject in receiverObjects) 
        {
            IReceiver r = rObject.GetComponent<IReceiver>();
            targetReceivers.Add(r);
        }
    }

    public void OnSignal(bool polarity)
    {
        for (int i = 0; i < targetReceivers.Count; i++)
        {
            targetReceivers[i].OnSignal(polarity);
        }
    }
}
