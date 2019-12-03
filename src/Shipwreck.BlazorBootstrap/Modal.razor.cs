using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Shipwreck.BlazorBootstrap
{
    public partial class Modal
    {
        private ElementReference _Element;

        [Parameter]
        public ModalBackdrop Backdrop { get; set; }

        [Parameter]
        public bool HandlesKeyboard { get; set; }

        [Parameter]
        public bool ShouldFocus { get; set; }

        #region Class

        [Parameter]
        public bool IsFade { get; set; }

        [Parameter]
        public string ExtraClasses { get; set; }

        #endregion Class

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> Attributes { get; set; }

        #region IsOpen

        private bool _IsOpen;

        [Parameter]
        public bool IsOpen
        {
            get => _IsOpen;
            set
            {
                if (value != _IsOpen)
                {
                    _IsOpen = value;
                    IsOpenChanged?.Invoke(_IsOpen);
                    StateHasChanged();
                }
            }
        }

        [Parameter]
        public Action<bool> IsOpenChanged { get; set; }

        #endregion IsOpen
    }
}