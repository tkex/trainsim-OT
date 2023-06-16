using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject
{
    private List<Observer> observers = new List<Observer>();

    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    protected void Notify()
    {
        foreach (Observer observer in observers)
        {
            observer.OnNotify();
        }
    }
}