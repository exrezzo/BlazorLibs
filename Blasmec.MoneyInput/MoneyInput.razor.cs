using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blasmec.MoneyInput
{
    public partial class MoneyInput
    {
        [Inject] public IJSRuntime JsRuntime { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            
        }
    }
}
