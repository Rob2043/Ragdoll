using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    private List<Rigidbody> _rigibodyies;
    
    public void Initializing()
    {
        _rigibodyies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        Disable();
    }

    public void Hit(Vector3 force, Vector3 hitposition)
    {
        Rigidbody injuredRigibody = _rigibodyies.OrderBy(rigibody => Vector3.Distance(rigibody.position, hitposition)).First(); 
        injuredRigibody.AddForceAtPosition(force,hitposition,ForceMode.Impulse);
    }
    public void Enable()
    {
        foreach(Rigidbody rigidbody in _rigibodyies)
            rigidbody.isKinematic = false;
    }
    public void Disable()
    {
        foreach(Rigidbody rigidbody in _rigibodyies)
            rigidbody.isKinematic = true;
    }
}
