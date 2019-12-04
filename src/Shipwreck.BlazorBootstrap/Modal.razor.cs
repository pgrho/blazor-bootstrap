using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Shipwreck.BlazorBootstrap
{
    public partial class Modal : ComponentBase
    {
#pragma warning disable IDE0044
        private ElementReference _Element;
#pragma warning restore IDE0044

        private bool _IsRendered;

        private IJSInProcessRuntime InProcessJS => JS as IJSInProcessRuntime;

        [Inject]
        public IJSRuntime JS { get; set; }

        [Parameter]
        public ModalBackdrop Backdrop { get; set; } = ModalBackdrop.Enabled;

        [Parameter]
        public bool HandlesKeyboard { get; set; } = true;

        [Parameter]
        public bool ShouldFocus { get; set; } = true;

        #region Class

        [Parameter]
        public bool IsFade { get; set; } = true;

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
            set => SetIsOpen(value, _IsRendered);
        }

        private void SetIsOpen(bool value, bool shouldRender)
        {
            if (value != _IsOpen)
            {
                _IsOpen = value;
                IsOpenChanged?.Invoke(_IsOpen);
                if (shouldRender)
                {
                    if (_IsRendered)
                    {
                        InvokeMethod(_IsOpen ? "show" : "hide");
                    }
                    else
                    {
                        StateHasChanged();
                    }
                }
            }
        }

        [Parameter]
        public Action<bool> IsOpenChanged { get; set; }

        #endregion IsOpen

        public event EventHandler Showing;
        public event EventHandler Shown;
        public event EventHandler Hiding;
        public event EventHandler Hidden;
        public event EventHandler HidePrevented;

        #region JS Callbacks

        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetShowing()
        {
            SetIsOpen(true, false);
            OnShowing();
        }

        protected virtual void OnShowing()
            => Showing?.Invoke(this, EventArgs.Empty);

        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetShown()
        {
            SetIsOpen(true, false);
            OnShown();
        }

        protected virtual void OnShown()
            => Shown?.Invoke(this, EventArgs.Empty);

        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetHiding()
        {
            SetIsOpen(false, false);
            OnHiding();
        }

        protected virtual void OnHiding()
            => Hiding?.Invoke(this, EventArgs.Empty);

        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetHidden()
        {
            SetIsOpen(false, false);
            OnHidden();
        }

        protected virtual void OnHidden()
            => Hidden?.Invoke(this, EventArgs.Empty);

        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetHidePrevented()
        {
            OnHidePrevented();
        }

        protected virtual void OnHidePrevented()
            => HidePrevented?.Invoke(this, EventArgs.Empty);

        #endregion JS Callbacks

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var t = base.OnAfterRenderAsync(firstRender);
            if (t != null)
            {
                await t.ConfigureAwait(false);
            }
            if (!_IsRendered)
            {
                _IsRendered = true;

                if (InProcessJS != null)
                {
                    InProcessJS.InvokeVoid("Shipwreck.BlazorBootstrap.initModal", _Element, DotNetObjectReference.Create(this), _IsOpen);
                }
                else
                {
                    await JS.InvokeVoidAsync("Shipwreck.BlazorBootstrap.initModal", _Element, DotNetObjectReference.Create(this), _IsOpen).ConfigureAwait(false);
                }
            }
        }

        public ValueTask ShowAsync()
        {
            if (_IsRendered && _IsOpen)
            {
                return InvokeMethod("show");
            }
            else
            {
                IsOpen = true;
                return default;
            }
        }

        public ValueTask HideAsync()
        {
            if (_IsRendered && !_IsOpen)
            {
                return InvokeMethod("hide");
            }
            else
            {
                IsOpen = false;
                return default;
            }
        }

        public ValueTask HandleUpdateAsync()
        {
            if (_IsRendered)
            {
                return InvokeMethod("handleUpdate");
            }
            else
            {
                return default;
            }
        }

        private ValueTask InvokeMethod(string name)
        {
            if (InProcessJS != null)
            {
                InProcessJS.InvokeVoid("Shipwreck.BlazorBootstrap.invokeModal", _Element, name);
                return default;
            }
            else
            {
                return JS.InvokeVoidAsync("Shipwreck.BlazorBootstrap.invokeModal", _Element, name);
            }
        }
    }
}