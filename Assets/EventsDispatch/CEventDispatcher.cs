using System.Collections;
using System;
public delegate void CEventListenerDelegate(CBaseEvent evt);

public class CEventDispatcher
{
    static CEventDispatcher instance;
    public static CEventDispatcher GetInstance()
    {
        if (instance == null)
        {
            instance = new CEventDispatcher();
        }
        return instance;
    }

    private Hashtable listeners = new Hashtable();
    //增加事件监听
    public void AddEventListener(CEventType eventType, CEventListenerDelegate listener)
    {
        CEventListenerDelegate ceventListenerDelegate = this.listeners[eventType] as CEventListenerDelegate;
        ceventListenerDelegate = (CEventListenerDelegate)Delegate.Combine(ceventListenerDelegate, listener);
        this.listeners[eventType] = ceventListenerDelegate;
    }
    //移除事件监听
    public void RemoveEventListener(CEventType eventType, CEventListenerDelegate listener)
    {
        CEventListenerDelegate ceventListenerDelegate = this.listeners[eventType] as CEventListenerDelegate;
        if (ceventListenerDelegate != null)
        {
            ceventListenerDelegate = (CEventListenerDelegate)Delegate.Remove(ceventListenerDelegate, listener);
        }

        this.listeners[eventType] = ceventListenerDelegate;
    }
    //分发事件消息
    public void DispatchEvent(CBaseEvent evt)
    {
        CEventListenerDelegate ceventListenerDelegate = this.listeners[evt.Type] as CEventListenerDelegate;

        if (ceventListenerDelegate != null)
        {
            try
            {
                ceventListenerDelegate(evt);
            }
            catch (System.Exception e)
            {
                throw new System.Exception(string.Concat(new string[]
                    {
                        "Error dispatching event",
                        evt.Type.ToString(),
                        ": ",
                        e.Message,
                        " ",
                        e.StackTrace
                    }), e);
            }
        }
    }

    public void RemoveAll()
    {
        this.listeners.Clear();
    }
}




 
public class CBaseEvent
{
    protected Hashtable arguments;
    protected CEventType type;
    protected Object sender;

    public CEventType Type
    {
        get
        {
            return this.type;
        }
        set
        {
            this.type = value;
        }
    }

    public IDictionary Params
    {
        get
        {
            return this.arguments;
        }
        set
        {
            this.arguments = (value as Hashtable);
        }
    }

    public Object Sender
    {
        get
        {
            return this.sender;
        }
        set
        {
            this.sender = value;
        }
    }

    public override string ToString()
    {
        return this.type + " [ " + ((this.sender == null) ? "null" : this.sender.ToString()) + " ] ";
    }

    public CBaseEvent Clone()
    {
        return new CBaseEvent(this.type, this.arguments, Sender);
    }

    public CBaseEvent(CEventType type, Object sender)
    {
        this.Type = type;
        Sender = sender;
        if (this.arguments == null)
        {
            this.arguments = new Hashtable();
        }
    }

    public CBaseEvent(CEventType type, Hashtable args, Object sender)
    {
        this.Type = type;
        this.arguments = args;
        Sender = sender;
        if (this.arguments == null)
        {
            this.arguments = new Hashtable();
        }
    }
}


public enum CEventType      
{
    GAME_OVER,
    GAME_WIN,
    PAUSE,
    ENERGY_EMEPTY,
    GAME_DATA,
}
