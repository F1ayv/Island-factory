using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Conveyor : Tile, IConveyor
{
    [SerializeField]
    public Mesh[] ModelsConveyor;
    public Mesh[] ModelsConveyorCLoch;
    private String typeConveyor;
    private static float speed = 1;

    private List<Vector3> _baseVectors = new() {Vector3.forward, Vector3.back,Vector3.right, Vector3.left };
    private List <GameObject> resList = new ();
    public IOutput _conveyorInput;
    public IInput _conveyorOutput;
    private GameObject LastRes;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < resList.Count; i++)
        {
            var transform1 = transform.position;
            Vector3 target = new Vector3(transform1.x,resList[i].transform.position.y,transform1.z);
            
            if (i == 0)
            {
                if (LastRes != null)
                {
                    if(Vector3.Distance(LastRes.transform.position, resList[i].transform.position) > 0.4f)
                        resList[i].transform.position = Vector3.MoveTowards(resList[i].transform.position, target,Time.deltaTime*speed);
                }
                else
                    resList[i].transform.position = Vector3.MoveTowards(resList[i].transform.position, target,Time.deltaTime * speed);
            }
            else if (Vector3.Distance(resList[i].transform.position, resList[i - 1].transform.position)>0.4f)
            {
                resList[i].transform.position = Vector3.MoveTowards(resList[i].transform.position, target,
                    Time.deltaTime * speed);
            }

            if (_conveyorOutput != null && Vector3.Distance(resList[i].transform.position, target) <= 0.0001f)
            {
                LastRes = resList[i];
                _conveyorOutput.NewRes(resList[i]);
                resList.RemoveAt(i);
                i--;
            }
        }
    }
    public override void CheckPos()
    {
        bool Output = false, Input = false;
        for (int i = 0; i < _baseVectors.Count; i++)
        {
            Ray ray = new Ray(transform.position + Vector3.up / 3, _baseVectors[i]);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.7f))
            {
                Transform transformBulding = hit.transform;
                if (transformBulding.tag == "Output" && _conveyorInput == null)
                {
                    Debug.Log("1241");
                    IOutput input = transformBulding.GetComponent<IOutput>();
                    if (input.Input == null)
                    {
                        input.Input = this;
                        //Input = true;
                        _conveyorInput = input;
                    }
                }
                else if (transformBulding.tag == "Conveyor")
                {
                    Debug.Log("Найден:" + name);
                    IConveyor conv = transformBulding.GetComponent<IConveyor>();
                    if (conv.GetOutput() == null)
                    {
                        if (conv.GetInput() == null && _conveyorOutput == null)
                        {
                            //transform.rotation = Quaternion.AngleAxis(Quaternion.Angle(transform.rotation,conv.GetPos().transform.rotation),Vector3.up);
                            //transform.LookAt(conv.GetPos());
                            _conveyorOutput=conv;
                            conv.SetInput(this);
                            Input = true;
                            Debug.Log("Вход "+conv.GetPos().name + " = " +name);
                        }
                        else
                        {
                            //conv.GetPos().rotation = Quaternion.AngleAxis(Quaternion.Angle(transform.rotation,conv.GetPos().transform.rotation),Vector3.up);
                            //conv.GetPos().LookAt(transform);
                            if (_conveyorInput == null)
                            { 
                                _conveyorInput = conv;
                                conv.SetOutput(this);
                                //Output = true;
                                Debug.Log("Выход "+conv.GetPos().name + " = " +name);
                            }
                        }
                    }
                    else
                    {
                        if (conv.GetInput() == null && _conveyorOutput==null)
                        {
                            //transform.LookAt(conv.GetPos());
                            //transform.rotation = 
                            _conveyorOutput=conv;
                            SetOutput(conv);
                            conv.SetInput(this);
                            Input = true;
                            Debug.Log("Вход "+conv.GetPos().name + " = " +name);
                        }
                    }
                }
            }
        }

        if (_conveyorInput != null)
        {
            if (_conveyorOutput != null)
            {
                Debug.Log("вход-выход");
                FindNeedMesh();
                // FindNeedMesh(this, _conveyorInput, _conveyorOutput,1,90);
            }
            else
            {
                FindNeedMesh();
                Debug.Log("Только вход");
             //   FindNeedMesh(this, _conveyorInput, _conveyorInput.GetInput(),1,0);
            }
        }
        else if (_conveyorOutput != null)
        {
            FindNeedMesh();    
            Debug.Log("Только выход");
           // FindNeedMesh(this , _conveyorOutput, _conveyorOutput.GetOutput(),-1,-90);
        }
    }

    private void FindNeedMesh()
    {
        Transform transformConv, transformConv1;
        if (_conveyorInput != null && _conveyorOutput != null)
        {

                transformConv = transform;
                transform.LookAt(GetInput().GetPos());
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                transform.eulerAngles += new Vector3(0, 90, 0);
                Transform t = _conveyorInput.GetPos();
                FindPosition(0,t,_conveyorOutput.GetPos(),-1);
                
                transformConv =  _conveyorInput.GetPos();
                if (_conveyorInput.GetInput() == null)
                {
                    Debug.Log("Поворот входа если он 1");
                    _conveyorInput.GetPos().LookAt(transform);
                    _conveyorInput.GetPos().eulerAngles += new Vector3(0, -90, 0);
                }
                else
                {
                    transformConv.LookAt(_conveyorInput.GetInput().GetPos());
                    _conveyorInput.GetPos().eulerAngles += new Vector3(0, 90, 0);
                    if (_conveyorInput.GetInput() == null)
                        return;
                    t = _conveyorInput.GetInput().GetPos();
                    FindPosition(0, t, transform, -1);
                }
                
                transformConv = _conveyorOutput.GetPos();
                if (_conveyorOutput.GetOutput() == null)
                {
                    Debug.Log("Поворот выхода если он 1");
                    _conveyorOutput.GetPos().LookAt(transform);
                    _conveyorOutput.GetPos().eulerAngles += new Vector3(0, 90, 0);
                    return;
                }
                transformConv.LookAt(_conveyorOutput.GetOutput().GetPos());
                _conveyorOutput.GetPos().eulerAngles += new Vector3(0, -90, 0);
                t = _conveyorOutput.GetOutput().GetPos();
                FindPosition(270,t,transform,1);
        }
        else if (_conveyorInput != null)    
        {
            transformConv = _conveyorInput.GetPos();
            transform.LookAt(transformConv);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            transform.eulerAngles += new Vector3(0, 90, 0);
            if (_conveyorInput.GetInput() == null)
                return;
            Transform t = _conveyorInput.GetInput().GetPos();
            FindPosition(0,t,transform,-1);
        }
        else if (_conveyorOutput != null)
        {
            transformConv = _conveyorOutput.GetPos();
            transform.LookAt(transformConv);
            transform.eulerAngles += new Vector3(0, -90, 0);
            if (_conveyorOutput.GetOutput() == null)
            {
                _conveyorOutput.GetPos().rotation = transform.rotation;
                return;
            }
            Transform t = _conveyorOutput.GetOutput().GetPos();
            FindPosition(270,t,transform,1);
        }

        void FindPosition(int angle,Transform t,Transform t1,int k)
        {

            Vector2 pos = new Vector2(t.position.x,t.position.z);
            Vector2 pos1 = new Vector2(t1.position.x,t1.position.z);
            float distanseX = (pos.x - pos1.x), distanseY = (pos.y-pos1.y);
            var tang = distanseX/distanseY;
            float angleIn = Math.Abs(Mathf.Atan(tang) * Mathf.Rad2Deg);
            Debug.Log(Mathf.Atan(tang) * Mathf.Rad2Deg);
            if (Math.Abs(Mathf.Atan(tang) * Mathf.Rad2Deg - 45) < 0.0001f)
            {
                transformConv.eulerAngles += new Vector3(0, angle, 0);
                transformConv.GetChild(0).GetComponent<MeshFilter>().mesh = ModelsConveyor[1];
                transformConv.GetChild(1).GetComponent<MeshFilter>().mesh = ModelsConveyorCLoch[1];
                Debug.Log(transformConv.localRotation.eulerAngles.y);
                if (((transformConv.localRotation.eulerAngles.y + 0) < 0.0001f)||((Mathf.Abs(transformConv.localRotation.eulerAngles.y - 180)) < 0.0001f))
                {
                    Debug.Log(transformConv.localRotation.eulerAngles.y);
                    transformConv.localScale = new Vector3(-1*k, 1, 1*k);
                }
            }

            if (Math.Abs(Mathf.Atan(tang) * Mathf.Rad2Deg + 45) < 0.0001f)
            {
                transformConv.eulerAngles += new Vector3(0, angle, 0);
                transformConv.GetChild(0).GetComponent<MeshFilter>().mesh = ModelsConveyor[1];
                transformConv.GetChild(1).GetComponent<MeshFilter>().mesh = ModelsConveyorCLoch[1];
                Debug.Log(transformConv.localRotation.eulerAngles.y);
                if (Mathf.Abs(transformConv.localRotation.eulerAngles.y - 90) < 0.0001f||Mathf.Abs(transformConv.localRotation.eulerAngles.y - 270) < 0.0001f)
                {
                    Debug.Log(transformConv.localRotation.eulerAngles.y);
                    transformConv.localScale = new Vector3(-1*k, 1, 1*k);
                }
            }
        }
    }

    private void FindNeedMeshFirst()
    {
        
    }

    public IInput GetOutput()
    {
        return _conveyorOutput;
    }

    public IOutput GetInput()
    {
        return _conveyorInput;
    }

    public void SetOutput(IInput b)
    {
        _conveyorOutput = b;
    }

    public IInput Input { get; set; }

    public void SetInput(IOutput b)
    {
        _conveyorInput = b;
    }

    public IOutput Output { get; set; }

    public void NewRes(GameObject res)
    {
        if (res == null)
            return;
        
        res.transform.SetParent(transform);
        resList.Add(res);
    }

    public bool CheckOpacity()
    {
        return resList.Count < 2;
    }

    public void RemoveRes(GameObject res)
    {
        throw new NotImplementedException();
    }

    public void SetCheckLastPosRes(bool b)
    {
        throw new NotImplementedException();
    }

    public Transform GetPos()
    {
        return transform;
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position+Vector3.up/3, Vector3.back);
        
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeZ; y++)
            {
                if((x+y)%2==1)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.blue;
                
                Vector3 pos = new Vector3( + x + transform.position.x , 0, + y + transform.position.z );
                Gizmos.DrawCube(pos, new Vector3(1, 0.1f, 1));
            }
        }
    }
}
