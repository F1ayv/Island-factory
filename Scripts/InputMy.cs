using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMy : MonoBehaviour, IInput
{
    public IMekanism parent; 
    public IOutput Output { get; set; }
    public void NewRes(GameObject res)
    {
        StartCoroutine(ResourceToLastPos(res));
    }

    public bool CheckOpacity()
    {
        return true;
    }
    public virtual void SetOutput(IInput b)
    {
        
    }

    public Transform GetPos()
    {
        return transform;
    }

    public IInput GetOutput()
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
                Conveyor conveyor = transformBulding.GetComponent<Conveyor>();
                if (conveyor._conveyorOutput == null)
                {
                    conveyor._conveyorOutput = this;
                    conveyor.CheckPos();
                    Output = transformBulding.GetComponent<Conveyor>();
                }
                break;
            }
        }
    }

    public IEnumerator ResourceToLastPos(GameObject res)
    {
        Vector3 newPos = new Vector3(transform.position.x, res.transform.position.y,
            transform.position.z);
        
        while (res.transform.position.x - transform.position.x > 0.001f || res.transform.position.z - transform.position.z > 0.001f)
        {
            res.transform.position = Vector3.MoveTowards(res.transform.position, newPos, Time.deltaTime * 2);
            yield return Time.deltaTime;
        }
        if(parent!= null)
            parent.GenerationRes(res);
        else
        {
            Destroy(res);
        }
        yield return Time.deltaTime;
    }
}
