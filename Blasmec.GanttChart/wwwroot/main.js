import "./jquery-3.6.0.min.js"
import "./dhtmlxgantt.js"
import "./gantt-api.js"

var netObjReference;
export function init(dotnetHelper) {
    console.log(dotnetHelper);
    netObjReference = dotnetHelper;
}

export function initGantt(canAddTasks, canEditTasks) {
    gantt.plugins({
        click_drag: true,
        fullscreen: true,
        keyboard_navigation: true,
        marker: true
        //quick_info: true

    });
    gantt.config.fullscreen = true;
    gantt.config.click_drag = true;
    gantt.config.date_format = "%Y-%m-%d %H:%i";

    //gantt.config.date_format = "%d-%m-%Y %H:%i";
    gantt.config.scales = [
        { unit: "month", step: 1, format: "%F, %Y" },
        { unit: "week", step: 1, format: "%W" },
    ];
    gantt.config.columns = [
        { name: "text", label: "Descrizione", tree: true, width: '*' },
        { name: "start_date", label: "Inizio", align: "center" },
        { name: "duration", label: "Durata gg.", align: "center" },
        { name: "add", label: "", hide: !canAddTasks }
    ];
    // LIGHTBOX che si apre quando aggiungi un evento
    //var opts = [
    //    { key: 1, label: 'ACQ' },
    //    { key: 2, label: 'PPP' },
    //    { key: 3, label: 'CIA' }
    //];
    //default lightbox definition   
    //gantt.config.lightbox.sections = [
    //    { name: "description", height: 70, map_to: "text", type: "select", options: opts, focus: true },
    //    { name: "time", height: 72, map_to: "auto", type: "duration" }
    //];

    var dateToStr = gantt.date.date_to_str(gantt.config.task_date);
    gantt.addMarker({
        start_date: new Date(), //a Date object that sets the marker's date
        css: "today", //a CSS class applied to the marker
        text: "Oggi", //the marker title
        title: dateToStr(new Date()) // the marker's tooltip
    });


    gantt.init("gantt_here");
    gantt.attachEvent("onAfterTaskUpdate", function (id,  task) {
        //any custom logic here
        //DotNet.invokeMethodAsync("Blasmec.GanttChart", "Test", id);
        var j = JSON.stringify(task);
        netObjReference.invokeMethodAsync("TaskEdit", j);
        return true;
    });
    if (!canEditTasks) {
        gantt.config.drag_mode = "ignore";
        gantt.config.drag_links = false;
        gantt.config.details_on_dblclick = false;
    }
};

export function expand() {
    gantt.expand();
}
export function loadData(jsonData) {
    //gantt.parse({
    //    data: [
    //        { id: maxid, text: "awe", start_date: startdate, duration: 2, parent: 0, progress: 1, color:"#ff06b0"},
    //        //{ id: 2, text: "task #1", start_date: "2019-08-01 00:00", duration: 5, parent: 1, progress: 1 },
    //        //{ id: 3, text: "task #2", start_date: "2019-08-06 00:00", duration: 2, parent: 1, progress: 0.5 },
    //        //{ id: 4, text: "task #3", start_date: null, duration: null, parent: 1, progress: 0.8, open: true },
    //        //{ id: 5, text: "task #3.1", start_date: "2019-08-09 00:00", duration: 2, parent: 4, progress: 0.2 },
    //        //{ id: 6, text: "task #3.2", start_date: "2019-08-11 00:00", duration: 1, parent: 4, progress: 0 },
    //        //{ id: 7, text: "task mio", start_date: "2019-12-11 00:00", duration: 1, parent: 1, progress: 0 }
    //    ],
    //    links: [
    //        { id: 1, source: 2, target: 3, type: "0" },
    //        { id: 2, source: 3, target: 4, type: "0" },
    //        { id: 3, source: 5, target: 6, type: "0" }
    //    ]
    //});
    var data = JSON.parse(jsonData);
    console.log(data);
    gantt.parse({
        data: data
    });
}


export function get(id) {

    return gantt.getTask(id);
}

export function exportData() {
    //var j = gantt.exportToJSON({
    //    name: "gantt.json"
    //});
    var j = gantt.serialize();
    console.log(j);
    var ser = JSON.stringify(j);
    return ser;
}

//export function hidePluses() {
//    $(".gantt_grid_head_add").hide();
//    $(".gantt_add").hide();
//}
