using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using System;
using UnityEngine.Events;
using Oculus.Interaction.Input;
using UnityEngine.PlayerLoop;

public class ChangeParentWhenPointableEvent : MonoBehaviour
{
    public enum PointableEventType
    {
        Hover,
        Unhover,
        Select,
        Unselect,
        Move,
        Cancel
    }

    [Tooltip("The Pointable component to wrap.")]
    [SerializeField]
    [Interface(typeof(IPointable), new Type[] { })]
    private UnityEngine.Object _pointableObject;

    [Tooltip("The type of Pointable event that triggers the parenting change.")]
    [SerializeField]
    private PointableEventType _triggerEventType;

    [Tooltip("The new parent Transform.")]
    [SerializeField]
    private Transform _newParent;

    private IPointable _pointable;
    private HashSet<int> _activePointers = new HashSet<int>();

    public UnityEvent<PointerEvent> OnPointerEvent = new UnityEvent<PointerEvent>();
    public UnityEvent<IHand> OnHandEvent = new UnityEvent<IHand>();

    private void Awake()
    {
        if (_pointableObject is IPointable pointable)
        {
            _pointable = pointable;
        }
        else
        {
            Debug.LogError("The assigned Pointable object does not implement IPointable.");
        }
    }

    private void OnEnable()
    {
        if (_pointable == null)
        {
            return;
        }

        _pointable.WhenPointerEventRaised += HandlePointerEvent;
    }

    private void OnDisable()
    {
        if (_pointable == null)
        {
            return;
        }

        _pointable.WhenPointerEventRaised -= HandlePointerEvent;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        switch (_triggerEventType)
        {
            case PointableEventType.Hover:
                if (evt.Type == PointerEventType.Hover)
                {
                    OnEventTriggered(evt);
                }
                break;
            case PointableEventType.Unhover:
                if (evt.Type == PointerEventType.Unhover)
                {
                    OnEventTriggered(evt);
                }
                break;
            case PointableEventType.Select:
                if (evt.Type == PointerEventType.Select)
                {
                    OnEventTriggered(evt);
                }
                break;
            case PointableEventType.Unselect:
                if (evt.Type == PointerEventType.Unselect)
                {
                    OnEventTriggered(evt);
                }
                break;
            case PointableEventType.Move:
                if (evt.Type == PointerEventType.Move)
                {
                    OnEventTriggered(evt);
                }
                break;
            case PointableEventType.Cancel:
                if (evt.Type == PointerEventType.Cancel)
                {
                    OnEventTriggered(evt);
                }
                break;
        }
    }

    private void OnEventTriggered(PointerEvent evt)
    {
        Debug.Log($"Event From: {evt.Data.GetType()}");

        OnPointerEvent.Invoke(evt);

        if(evt.Data is MonoBehaviour data)
        {
           if(data.TryGetComponent( out HandRef handRef))
           {
                OnHandEvent.Invoke(handRef.Hand);
           }
        }
    
    }

}
