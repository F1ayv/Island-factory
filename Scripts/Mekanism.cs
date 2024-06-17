using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mekanism : Tile, IMekanism
{
    private int _needOutputID = 0;
    public List<IInput> Inputs { get; set;}
    public List<IOutput> Outputs { get; set ; }
    public Dictionary<int, int> ResInside = new();
    public Recipe Recipe;
    public GameObject prefabRes;

    [SerializeField]
    private float _timerSpawnerRes, _reloadTimeSpawnerRes = 1;
    private float lastOutput;

    public void SetRecipe(Recipe recipe)
    {
        Recipe = recipe;
        ResInside = new Dictionary<int, int>();
        foreach (var ins in recipe.inputs)
        {
            ResInside.Add(ins.res.resId,0);
        }
    }
    public virtual bool GenerationRes(GameObject gameObject)
    {
        if (Recipe != null  && Recipe.inputs.Length != null)
        {
            for (int i = 0; i < Recipe.inputs.Length; i++)
            {
                if (Recipe.inputs[i].amount > ResInside[i])
                    return false;
            }
        }
        
        int emptyInput = CheckInput(_needOutputID, Outputs.Count);
        if(emptyInput < 0)
            emptyInput = CheckInput(0, _needOutputID);
        if (emptyInput < 0)
        {
            return false;
        }

        _needOutputID = emptyInput;

        int j = 1;
        if(Recipe != null  && Recipe.inputs.Length != null)
            j = Recipe.outputs[0].amount;

        for (int t = 0; t < j; t++)
        {
            GameObject res = Instantiate(gameObject, Outputs[_needOutputID].Input.GetPos());
            res.transform.position = new Vector3(Outputs[_needOutputID].GetPos().position.x, Outputs[_needOutputID].Input.GetPos().position.y+ 0.4f,
                Outputs[_needOutputID].GetPos().position.z);
            Outputs[_needOutputID].Input.NewRes(res);
        }
        _needOutputID++;
            
        if(Recipe != null)
            foreach (var input in Recipe.inputs)
            {
                ResInside[input.res.resId] = 0;
            }
        return true;

        int CheckInput(int startPos,int finalPos)
        {
            while (startPos < finalPos)
            {
                if (Outputs[startPos].Input == null || !Outputs[startPos].Input.CheckOpacity())
                {
                    startPos++;
                }
                else
                    return startPos;
            }
            return -1;
        }
    }

    public void FindOutputsInputs()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Output")
            {
                Output output = transform.GetChild(i).GetComponent<Output>();
                Outputs.Add(output);
            }
            
            if (transform.GetChild(i).tag == "Input")
            {
                InputMy input = transform.GetChild(i).GetComponent<InputMy>();
                input.parent = this;
                Inputs.Add(input);
            }
        }   
    }

    public abstract bool GetRes(Res res);

    public override void CheckPos()
    {
        foreach (var output in Outputs)
        {
            if(!(output is Output))
                continue;
            
            Output outputReal = output as Output;
            outputReal.CheckPos();
        }
        
        foreach (var input in Inputs)
        {
            if(!(input is InputMy))
                continue;
            
            InputMy inputReal = input as InputMy;
            inputReal.CheckPos();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Recipe != null)
            SetRecipe(Recipe);
        Inputs = new List<IInput>();
        Outputs = new List<IOutput>();
        FindOutputsInputs();
    }

    // Update is called once per frame
    void Update()
    {
        _timerSpawnerRes += Time.deltaTime;
        if (_timerSpawnerRes >= _reloadTimeSpawnerRes)
        {
            if(prefabRes!=null)
                GenerationRes(prefabRes);
            _timerSpawnerRes = 0;
        }
    }
}
