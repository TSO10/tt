if (!energinetChart) {
    var energinetChart = {
        Series: [{ Name: "serie1", FieldName: "CentralPowerDK1", color: "#26B", axis: "", DisplayName: "CentralPower" },
                { Name: "serie2", FieldName: "LocalCHPPowerDK1", color: "#F80", axis: "", DisplayName: "LocalCHPPowerDK1" },
                { Name: "serie3", FieldName: "WindTurbinesDK1", color: "Black", axis: "r", DisplayName: "Wind"}],
        Customlegend: {},
        GetData: function (year, month, day) {
            $("#loadInfo").text("Load data...");
            $.ajax({
                url: "../../siteWebService.svc/GetEnerginetByDate",
                data: '{ "year":' + year + ',"month":' + month + ',"day":' + day + '}',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                processData: false,
                dataType: "json",
                success: this.GetDataSuccessful,
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert(XMLHttpRequest.responseText); }
            });
        },
        GetDataSuccessful: function (data) {
            $("#chart").html("<div style='height:100%;'></div>");
            energinetChart.LastData = null;
            if (data.d != null && data.d.length != 0) {
                $("#loadInfo").text("Load complete.");
                energinetChart.LastData = data.d;
                var cSeries = {};
                for (var i = 0; i < energinetChart.Series.length; i++) {
                    cSeries[energinetChart.Series[i].Name] = [];
                }               
                var thelabels = new Array(data.d.length);
                var index = -1;               
                for (var i = 0; i < data.d.length; i++) {
                    if (index != data.d[i].Date) {
                        index = thelabels[i] = data.d[i].Date;                   
                    }
                    else {
                        thelabels[i] = "";
                    }
                    for (var j = 0; j < energinetChart.Series.length; j++) {
                        cSeries[energinetChart.Series[j].Name].push($.trim(data.d[i][energinetChart.Series[j].FieldName]));
                    }
                }
                $("#chart div:first").chart({
                    labels: thelabels,
                    template: "google_analytics",
                    values: cSeries,
                    legend: energinetChart.Customlegend
                });
            }
            else {
                $("#chart div:first").chart({
                    labels: null,
                    template: "google_analytics",
                    values: null,
                    legend: energinetChart.Customlegend
                });
                $("#loadInfo").text("Load complete. No data for selected date");
            }
        },
        LastData: null,
        InitializeGraph: function () {
            var cSeries = {};
            for (var i = 0; i < energinetChart.Series.length; i++) {
                energinetChart.Customlegend[energinetChart.Series[i].Name] = energinetChart.Series[i].DisplayName;
                var serie = { fill: true,
                    fillProps: {
                        opacity: .1
                    },
                    color: energinetChart.Series[i].color
                };
                if (energinetChart.Series[i].axis != "") {
                    serie.axis = energinetChart.Series[i].axis;
                }
                cSeries[energinetChart.Series[i].Name] = serie;
            }
            $.elycharts.templates['google_analytics'] = {
                type: "line",
                margins: [10, 15, 25, 160],
                tooltips: function (env, serie, index, value, label) {
                    var str = "";
                    if (energinetChart.LastData != null) {
                        str = "<div class='energinet-tooltips'><span>Hour: </span><span class='display-value'>" + energinetChart.LastData[index].Date + "</span><br/>";
                        for (var i = 0; i < energinetChart.Series.length; i++) {
                            str += "<span style='color:" + energinetChart.Series[i].color + "'>" + energinetChart.Series[i].DisplayName +
                        ": </span><span class='display-value'>" + env.opt.values[energinetChart.Series[i].Name][index] + "</span><br/>";
                        }
                        str += "</div>";
                        return str;
                    }
                    else {
                        return "";
                    }
                },
                defaultSeries: {
                    plotProps: {
                        "stroke-width": 4
                    },
                    dot: true,
                    rounded: false,
                    dotProps: {
                        stroke: "white",
                        size: 5,
                        "stroke-width": 1,
                        opacity: 0 // dots invisible until we hover it
                    },
                    startAnimation: { // use an animation to start plotting the chart
                        active: true,
                        type: "avg", // start from the average line.
                        speed: 1000, // animate in 1 second.
                        easing: ">"
                    },
                    stepAnimation: { // defines an animation for data updates
                        speed: 2000,
                        delay: 0,
                        easing: '<>'
                    },
                    highlight: {
                        scaleSpeed: 0, // do not animate the dot scaling. instant grow.
                        scaleEasing: '',
                        scale: 1.2, // enlarge the dot on hover
                        newProps: {
                            opacity: 1 // show dots on hover
                        }
                    },
                    tooltip: {
                        height: 100,
                        width: 180,
                        padding: [3, 3],
                        offset: [-15, -10],
                        frameProps: {
                            opacity: 0.95,
                            /* fill: "white", */
                            stroke: "#000"

                        }
                    }
                },
                series: cSeries,
                defaultAxis: {
                    labels: true,
                    labelsProps: {
                        fill: "#49B",
                        "font-size": "10px"
                    },
                    labelsAnchor: "start",
                    labelsMargin: 5,
                    labelsDistance: -8
                },
                axis: {
                    l: { // left axis
                        title: 'Power',
                        titleDistance: 10,
                        labels: true,
                        labelsDistance: 0,
                        labelsSkip: 1,
                        labelsAnchor: "start",
                        labelsMargin: 15,
                        labelsProps: {
                            fill: "#AAA",
                            "font-size": "11px",
                            "font-weight": "bold"
                        }
                    },
                    r: { // right axis
                        title: 'Wind',
                        titleDistance: 10,
                        labels: true,
                        labelsDistance: 0,
                        labelsSkip: 1,
                        labelsAnchor: "end",
                        labelsMargin: 15,
                        labelsProps: {
                            fill: "#AAA",
                            "font-size": "11px",
                            "font-weight": "bold"
                        }
                    },
                    x:
                    {
                        title: 'Hour',
                        titleDistance: 10
                    }
                },
                features: {
                    mousearea: {
                        type: 'axis'
                    },
                    tooltip: {
                        positionHandler: function (env, tooltipConf, mouseAreaData, suggestedX, suggestedY) {
                            return [mouseAreaData.event.pageX, mouseAreaData.event.pageY, true]
                        }
                    },
                    grid: {
                        draw: true, // draw both x and y grids
                        forceBorder: [true, false, true, false], // force grid for external border
                        ny: 2, // use 10 divisions for y grid
                        nx: 5, // 10 divisions for x grid
                        props: {
                            stroke: "#CCC" // color for the grid
                        }
                    },
                    legend: {
                        x: 0, y: 10, width: 140, height: 100,
                        defaultDotProps: { type: 'rect', r: 5 }
                    }
                }
            }
        }
    }
}

$(document).ready(function () {
    energinetChart.InitializeGraph();
    var d = new Date();
    energinetChart.GetData(d.getFullYear(), d.getMonth() + 1, d.getDate());
    $("#datepicker").datepicker({
        onSelect: function (value, date) {
            energinetChart.GetData(date.selectedYear, parseInt(date.selectedMonth) + 1, date.selectedDay);
        }
    });
});