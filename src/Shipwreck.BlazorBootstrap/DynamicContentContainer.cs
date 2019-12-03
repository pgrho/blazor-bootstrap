using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Shipwreck.BlazorBootstrap
{
    public class DynamicContentContainer : ComponentBase
    {
        private List<DynamicContent> _Contents = new List<DynamicContent>();
        private List<DynamicContent> _Removing;

        public void Add(DynamicContent content)
        {
            _Contents.Add(content);
            StateHasChanged();
        }

        public void Remove(DynamicContent content)
        {
            if (_Contents.Remove(content))
            {
                (_Removing ?? (_Removing = new List<DynamicContent>(1))).Add(content);
                StateHasChanged();
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var i = 0;
            foreach (var c in _Contents)
            {
                c.BuildRenderTree(builder, ref i);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var t = base.OnAfterRenderAsync(firstRender);
            if (t != null)
            {
                await t.ConfigureAwait(false);
            }
            foreach (var c in _Contents)
            {
                var ct = c.OnAfterRenderAsync();
                if (ct != null)
                {
                    await ct.ConfigureAwait(false);
                }
            }
            if (_Removing != null)
            {
                foreach (var r in _Removing)
                {
                    r.Dispose();
                }
                _Removing = null;
            }
        }
    }
}