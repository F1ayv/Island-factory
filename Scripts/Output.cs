using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : MonoBehaviour, IOutput
{
    public IInput Input { get; set; }
    public virtual void SetInput(IOutput b)
    {
        
    }
    public Transform GetPos()
    {
        return transform;
    }

    public virtual IOutput GetInput()
    {
        return null;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position,transform.right*1);
    }

    public void CheckPos()
    {
        Ray ray = new Ray(transform.position + Vector3.up / 5, transform.right);
        RaycastHit[] hits = Physics.RaycastAll(ray, 0.7f);
        foreach (var hit in hits)
        {
            Transform transformBulding = hit.transform;
            if (transformBulding.tag == "Conveyor")
            {
                Input = transformBulding.GetComponent<Conveyor>();
                break;
            }
        }
    }
}
