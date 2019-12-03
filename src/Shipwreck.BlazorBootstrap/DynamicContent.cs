using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Shipwreck.BlazorBootstrap
{
    public abstract class DynamicContent : IDisposable
    {
        private bool _IsRendered;

        protected DynamicContent(DynamicContentContainer container, Type contentType, IEnumerable<KeyValuePair<string, object>> properties)
        {
            Container = container;
            ContentType = contentType;
            Properties = Array.AsReadOnly(properties.ToArray());
        }

        public DynamicContentContainer Container { get; }
        public Type ContentType { get; }
        public ReadOnlyCollection<KeyValuePair<string, object>> Properties { get; }

        private WeakReference<ComponentBase> _Content;

        public ComponentBase Content
            => _Content != null && _Content.TryGetTarget(out var c) ? c : null;

        protected internal void BuildRenderTree(RenderTreeBuilder builder, ref int sequence)
        {
            builder.OpenComponent(sequence++, ContentType);
            foreach (var kv in Properties)
            {
                builder.AddAttribute(sequence++, kv.Key, kv.Value);
            }
            builder.AddComponentReferenceCapture(
                sequence++,
                obj => _Content = obj is ComponentBase c ? new WeakReference<ComponentBase>(c) : null);

            builder.CloseComponent();
        }

        internal Task OnAfterRenderAsync()
        {
            if (_IsRendered)
            {
                return OnAfterRenderAsync(false);
            }
            else
            {
                var t = OnAfterRenderAsync(true);
                _IsRendered = true;
                return t;
            }
        }

        protected virtual Task OnAfterRenderAsync(bool firstRender) => null;

        #region IDisposable

        protected bool IsDisposed { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Container.Remove(this);
                }
                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable
    }
}