//TODO [ATH-1562] License

using System.Collections.Generic;
using UnityEngine;

namespace Athlos.WebView
{
  public class AthlosGamefaceView : AthlosWebView
  {
    struct Delegate
    {
      public string name;
      public System.Delegate function;
    }

    [Header("Coherent Labs Gameface")]
    [SerializeField] private cohtml.CohtmlView view;

    private bool readyForBindings;
    private List<Delegate> events;
    private List<Delegate> bindings;

    protected override void Awake()
    {
      base.Awake();
      view.Page = InitialUrl;
      readyForBindings = false;
      view.Listener.ReadyForBindings += OnReadyForBindings;
    }

    private void OnReadyForBindings()
    {
      readyForBindings = true;
      view.Listener.ReadyForBindings -= OnReadyForBindings;
      for (int i = 0; i < (events?.Count ?? 0); ++i)
      {
        view.View.RegisterForEvent(events[i].name, events[i].function);
      }
      for (int i = 0; i < (bindings?.Count ?? 0); ++i)
      {
        view.View.BindCall(bindings[i].name, bindings[i].function);
      }
    }

    protected virtual void Start()
    {
      view.View.AddInitialScript(AuthenticationScript);
    }

    public cohtml.CohtmlView View { get { return view; } }
    public cohtml.Net.View NetView { get { return view.View; } }

    public override void ExecuteJavascript(string javascript)
    {
      throw new System.NotSupportedException("GameFace does not support direct Javascript execution. Use trigger events or Bind to execute functions");
    }

    public void TriggerEvent(string eventName)
    {
      view.View.TriggerEvent(eventName);
    }

    public void TriggerEvent<T>(string eventName,
      T data)
    {
      view.View.TriggerEvent(eventName, data);
    }

    public void TriggerEvent<T1, T2>(string eventName,
      T1 t1Data, T2 t2Data)
    {
      view.View.TriggerEvent(eventName, t1Data, t2Data);
    }

    public void TriggerEvent<T1, T2, T3>(string eventName,
      T1 t1Data, T2 t2Data, T3 t3Data)
    {
      view.View.TriggerEvent(eventName, t1Data, t2Data, t3Data);
    }

    public void TriggerEvent<T1, T2, T3, T4>(string eventName,
      T1 t1Data, T2 t2Data, T3 t3Data, T4 t4Data)
    {
      view.View.TriggerEvent(eventName, t1Data, t2Data, t3Data, t4Data);
    }

    public void TriggerEvent<T1, T2, T3, T4, T5>(string eventName,
      T1 t1Data, T2 t2Data, T3 t3Data, T4 t4Data, T5 t5Data)
    {
      view.View.TriggerEvent(eventName, t1Data, t2Data, t3Data, t4Data, t5Data);
    }

    public void TriggerEvent<T1, T2, T3, T4, T5, T6>(string eventName,
      T1 t1Data, T2 t2Data, T3 t3Data, T4 t4Data, T5 t5Data, T6 t6Data)
    {
      view.View.TriggerEvent(eventName, t1Data, t2Data, t3Data, t4Data, t5Data, t6Data);
    }

    public void TriggerEvent<T1, T2, T3, T4, T5, T6, T7>(string eventName,
      T1 t1Data, T2 t2Data, T3 t3Data, T4 t4Data, T5 t5Data, T6 t6Data, T7 t7Data)
    {
      view.View.TriggerEvent(eventName, t1Data, t2Data, t3Data, t4Data, t5Data, t6Data, t7Data);
    }

    public void TriggerEvent<T1, T2, T3, T4, T5, T6, T7, T8>(string eventName,
      T1 t1Data, T2 t2Data, T3 t3Data, T4 t4Data, T5 t5Data, T6 t6Data, T7 t7Data, T8 t8Data)
    {
      view.View.TriggerEvent(eventName, t1Data, t2Data, t3Data, t4Data, t5Data, t6Data, t7Data, t8Data);
    }

    public void RegisterEvent(string name, System.Delegate function)
    {
      if (events == null)
      {
        events = new List<Delegate>();
      }
      events.Add(new Delegate()
      {
        name = name,
        function = function
      });
      if (readyForBindings)
      {
        view.View.RegisterForEvent(name, function);
      }
    }

    public void BindCall(string name, System.Delegate function)
    {
      if (bindings == null)
      {
        bindings = new List<Delegate>();
      }
      bindings.Add(new Delegate()
      {
        name = name,
        function = function
      });
      if (readyForBindings)
      {
        view.View.BindCall(name, function);
      }
    }
  }
}