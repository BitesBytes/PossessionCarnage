//Questa classe e' stata realizzata grazie al contributo di Codemonkey
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Various requirement
[RequireComponent(typeof(Rigidbody))]

//Class
public class CodeStyleExample
{
    //Contants: UpperCase SnakeCase
    public const int CONSTANT_FIELD = 56;

    //Porperties: PascalCase
    public static CodeStyleExample Instance { get; private set; }

    //Events: PascalCase
    public event EventHandler<OnSomethingHappenedEventArgs> OnSomethingHappened;
    //Other Event classes
    public class OnSomethingHappenedEventArgs: EventArgs
    {
        public int randomNum;
    }

    //Serialized Fields: camelCase
    [SerializeField] private int randomNum;

    //Private Fields: camelCase
    private float membervariable;

    //Public Fields: PascalCase
    public float TestPublicVariable;

    //Function Names: PascalCase
    private void Awake()
    {
        Instance = this;

        DoSomething(10f);

        DoSomething();
    }

    //Function Params: camelCase
    private void DoSomething(float time)
    {
        //Do something...
        membervariable = time * Time.deltaTime;
        if(membervariable > 0f)
        {
            //Do something else ...
        }
    }


    private void DoSomething()
    {
        OnSomethingHappened?.Invoke(this, new OnSomethingHappenedEventArgs{ randomNum = 8 });
    }
}