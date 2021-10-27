using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Blasmec.GanttChart
{
    public partial class Gantt
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        IEnumerable<GanttTask> _tasks = new List<GanttTask>();
        private IJSObjectReference module;
        [Parameter] public EventCallback<GanttTask> OnTaskEdit { get; set; }
        [Parameter] public IEnumerable<GanttTask> Data { get; set; }
        [Parameter] public bool CanAddTasks { get; set; }
        [Parameter] public bool CanEditTasks { get; set; }
        async Task Init()
        {
            module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blasmec.GanttChart/main.js");
        }
        //async Task LoadFake()
        //{
        //    //var dt = DateTime.Now.ToString("yyyy-MM-d");
        //    //await module.InvokeVoidAsync("loadData", dt);
        //    var l = Enumerable.Range(1, 5).Select(i =>
        //        new GanttTask()
        //        {
        //            Id = i,
        //            Color = "#ff8070",
        //            //Duration = 1,
        //            StartDate = DateTime.Today.ToUniversalTime() + TimeSpan.FromDays(i),
        //            EndDate = (DateTime.Today + TimeSpan.FromDays(14 + i)).ToUniversalTime(),
        //            Progress = 0.5,
        //            Text = $"VE MO VA #{i}"
        //        });
        //    var s = JsonSerializer.Serialize(l);
        //    await module.InvokeVoidAsync("loadData", s);

        //}

        //public async Task Load(IEnumerable<GanttTask> tasks)
        //{
        //    Tasks = Tasks.Concat(tasks);
        //    var s = JsonSerializer.Serialize(Tasks);
        //    await module.InvokeVoidAsync("loadData", s);
        //}


        [JSInvokable]
        public Task TaskEdit(string jsonTask)
        {
            Console.WriteLine(jsonTask);
            var task = JsonSerializer.Deserialize<GanttTask>(jsonTask);
            OnTaskEdit.InvokeAsync(task);
            return Task.CompletedTask;
        }


        public async Task<IEnumerable<GanttTask>> Export()
        {
            var ser = await module.InvokeAsync<string>("exportData");
            var j = JObject.Parse(ser);
            var d =  j["data"];
            var tasks = JsonSerializer.Deserialize<List<GanttTask>>(d.ToString());
            tasks.ForEach(task => Console.WriteLine(task.Text));
            Console.WriteLine(ser);
            return tasks;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await Init();
                await module.InvokeVoidAsync("init", DotNetObjectReference.Create(this));
                await module.InvokeVoidAsync("initGantt", CanAddTasks, CanEditTasks);
                var tasksJson = JsonSerializer.Serialize(_tasks);
                await module.InvokeVoidAsync("loadData", tasksJson);
                //await module.InvokeVoidAsync("hidePluses");
            }

        }

        protected override void OnInitialized()
        {
            _tasks = Data;
            base.OnInitialized();
        }
    }
}
