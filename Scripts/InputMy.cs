using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMy : MonoBehaviour, IInput
{
    public IMekanism parent; 
    public IOutput Output { get; set; }
    private GameObject insideRes;
    public void NewRes(GameObject res)
    {
        insideRes = res;
        StartCoroutine(ResourceToLastPos(res));
    }

    public bool CheckOpacity()
    {
        return insideRes == null;
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

        while (Vector3.Distance(res.transform.position,newPos) > 0.001f)
        {
            res.transform.position = Vector3.MoveTowards(res.transform.position, newPos, Time.deltaTime * 2);
            yield return Time.deltaTime;
        }

        if (parent != null)
        {
            while (!parent.GetRes(res))
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        Destroy(res);
        insideRes = null;
        
        yield return Time.deltaTime;
    }
}
