using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ChineseDictionary
{
    public class Canvas2DContext
    {
        private readonly IJSRuntime jsRuntime;
        private readonly ElementReference canvasRef;

        public Canvas2DContext(IJSRuntime jsRuntime, ElementReference canvasRef)
        {
            this.jsRuntime = jsRuntime;
            this.canvasRef = canvasRef;
        }

        public async Task DrawLine(double startX, double startY, double endX, double endY)
        {
            await jsRuntime.InvokeAsync<object>("__blazorCanvasInterop.drawLine", canvasRef, startX, startY, endX, endY);
        }

        public async Task SetStrokeStyleAsync(string strokeStyle)
        {
            await jsRuntime.InvokeAsync<object>("__blazorCanvasInterop.setContextPropertyValue", canvasRef, "strokeStyle", strokeStyle);
        }
    }
}
