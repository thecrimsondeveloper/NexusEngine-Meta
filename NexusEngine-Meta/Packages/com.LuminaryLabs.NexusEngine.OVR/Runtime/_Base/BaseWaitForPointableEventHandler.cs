using System;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using Oculus.Interaction;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseWaitForPointableEventHandler : BaseSequence<BaseWaitForPointableEventHandlerData>
    {
        private IPointable _pointable;
        private bool _isEventTriggered;

        private PointerEventType triggerEventType;



    

        /// <summary>
        /// Initializes the sequence with the provided data.
        /// </summary>
        /// <param name="currentData">The current data for the sequence.</param>
        /// <returns>A UniTask representing the initialization process.</returns>
        protected override UniTask Initialize(BaseWaitForPointableEventHandlerData currentData)
        {

            triggerEventType = currentData.triggerEventType;
            _pointable = currentData.pointableObject as IPointable;

            if (_pointable != null)
            {
                return UniTask.CompletedTask;
            }
            

            Debug.LogError("Pointable object is either null or does not implement IPointable.");
            return UniTask.FromException(new InvalidOperationException("Invalid Pointable Object"));
        }

        /// <summary>
        /// Called when the sequence begins.
        /// </summary>
        protected override void OnBegin()
        {
            if (_pointable == null)
            {
                this.Complete();
                return;
            }

            // Subscribe to pointer events
            _pointable.WhenPointerEventRaised += HandlePointerEvent;
        }

        /// <summary>
        /// Handles pointer events from the subscribed Pointable object.
        /// </summary>
        /// <param name="evt">The pointer event.</param>
        private void HandlePointerEvent(PointerEvent evt)
        {
            if (triggerEventType != evt.Type) return;


            // Perform any necessary actions here
            _isEventTriggered = true;

            // Unsubscribe and complete the sequence
            _pointable.WhenPointerEventRaised -= HandlePointerEvent;
            Complete();
        }

        /// <summary>
        /// Called when the sequence finishes.
        /// </summary>
        protected override void OnUnloaded()
        {
            // Ensure subscription is removed if the sequence ends prematurely
            if (_pointable != null)
            {
                _pointable.WhenPointerEventRaised -= HandlePointerEvent;
            }
            base.OnUnloaded();
        }
    }

    [Serializable]
    public class BaseWaitForPointableEventHandlerData : BaseSequenceData
    {
        [Tooltip("The Pointable component to listen to.")]
        [SerializeField]
        [Interface(typeof(IPointable), new Type[] { })]
        public UnityEngine.Object pointableObject;
        public PointerEventType triggerEventType;
    }
}
