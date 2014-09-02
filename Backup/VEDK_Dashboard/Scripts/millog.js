
var pickerFlag = false;
google.load('visualization', '1.0', { 'packages': ['controls'] });
google.setOnLoadCallback(drawDashboard());
var dateTime = null;
function drawDashboard(param) {

    var sUrl;
    var a = $('#picker').val() == -1;
    if (param == undefined) {
        if ($('#picker').val() == null || $('#picker').val() == -1) {
            sUrl = "../../siteWebService.svc/GetData";
        }
        else {
            sUrl = "../../siteWebService.svc/GetData?GSRN=" + $('#picker').val();
        }
    }
    else
        sUrl = "../../siteWebService.svc/GetData?GSRN=" + param;
    (function poll() {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: sUrl,
            dataType: "json",
            error: failureMethod,
            success: successMethod
            ////                    complete: poll,
            ////                    timeout: 3600000 //2*60*60*1000
        });
    })();
}

self.setInterval(function () { drawDashboard() }, 60000);
function successMethod(response) {
    response = response.d;
    if (response != null && response.length != 0 && (dateTime == null || dateTime != response[response.length - 1].DateTime)) {
        dateTime = response[response.length - 1].DateTime;
        var data = new google.visualization.DataTable();
        data.addColumn('datetime', 'DateTime');
        data.addColumn('number', 'EffectReal');
        var gsrn = new Array();
        for (var i = 0; i < response.length; i++) {
            var row = new Array();
            row[0] = parseJsonDate(response[i].DateTime);
            row[1] = parseFloat(response[i].EffectReal);
            gsrn[i] = response[i].GSRN.valueOf();
            data.addRow(row);
        }
        if (!pickerFlag) {
            var uniqueArray = gsrn.filter(function (elem, pos) {
                return gsrn.indexOf(elem) == pos;
            })

            $('#picker').val('');
            $("#picker").append($("<option></option>").val("").html('none'));
            $.each(uniqueArray, function (key, value) {
                $("#picker").append($("<option></option>").val(value).html(value));
            });
            pickerFlag = true;
            $("#picker_div").show();
        }
        var dashboard = new google.visualization.Dashboard(
                                document.getElementById('dashboard_div'));
        var control = initChartRangeFilter(response);
        var formatter = new google.visualization.DateFormat({ pattern: 'dd-MM-yyyy hh:mm:ss' });
        formatter.format(data, 0);
        var chart = initChart(response);
        dashboard.bind([control], chart);
        dashboard.draw(data);
    }
}


function parseJsonDate(jsonDate) {
    var dateTimePart = jsonDate.replace(/\/+Date\(([\d+-]+)\)\/+/, '$1');
    var minusSignIndex = dateTimePart.indexOf('-');
    var plusSignIndex = dateTimePart.indexOf('+');
    var signIndex = minusSignIndex > 0 ? minusSignIndex : plusSignIndex;
    if (signIndex > 0) {
        var datePart = dateTimePart.substring(0, signIndex);
        var offsetPart = dateTimePart.substring(signIndex);
        var offset = offsetPart.substring(0, 3) + '.' + ((parseFloat(offsetPart.substring(3)) / 60) * 100).toString();
        return getJSDate(parseFloat(datePart), parseFloat(offset));
    }
    else {
        return getJSDate(dateTimePart, 0);
    }
}


function getJSDate(ms, offset) {
    var date = new Date(ms);
    var utcTime = date.getTime() + (date.getTimezoneOffset() * 60000);
    var jsDate = new Date(utcTime + (3600000 * offset));
    return jsDate;
}


function failureMethod(errMsg) {
    alert(errMsg);
}


function initChartRangeFilter(response) {
    var control = new google.visualization.ControlWrapper({
        'controlType': 'ChartRangeFilter',
        'containerId': 'control_div',
        'options': {
            'filterColumnIndex': 0,
            'ui': {
                'chartType': 'AreaChart',
                'chartOptions': {
                    'chartArea': { 'width': '85%', 'height': '30%' },
                    'hAxis': { 'baselineColor': 'none', 'title': 'Time---->', 'format': 'dd-MM-yyyy hh:mm ' },
                    'animation': { 'duration': 400, 'easing': 'out' }
                },
                'chartView': {
                    'columns': [0, 1]
                },
                'minRangeSize': 1000
            }
        },
        'state': { 'range': { 'start': parseJsonDate(response[response.length - 1].DateTime), 'end': parseJsonDate(response
[0].DateTime)
        }
        }
    });
    return control;
}


function initChart(response) {
    var chart = new google.visualization.ChartWrapper({
        'chartType': 'LineChart',
        'containerId': 'chart_div',
        'options': {
            'chartArea': { 'height': '85%', 'width': '85%' },
            'hAxis': { 'slantedText': false, 'baselineColor': 'none', 'format': 'dd-MM-yyyy hh:mm:ss' },
            'vAxis': { 'viewWindow': { 'min': response[0].EffectReal - 5000, 'max': response[response.length - 1].EffectReal +
5000
            }, 'title': 'Effect Real---->'
            },
            'legend': { 'position': 'none' },
            'series': [{ color: 'Green', visibleInLegend: false }, {}, {}, { color: 'Blue', visibleInLegend: false}],
            'curveType': 'function',
            'backgroundColor': { 'fill': '#99C2FF', 'strokeWidth': 5 },

            'animation': { 'duration': 400, 'easing': 'out' },
            'lineWidth': 1,
            //                        'pointSize': 5,
            'tooltip': { textStyle: { color: '#006699', fontName: 'Arial', fontSize: 11 }, backgroundColor: '#000000',
                showColorCode: true
            }
            //'focusTarget':'category'
        },
        'height': 600,
        'width': 950,
        columns: [0, 1]
    });
    return chart;
}

function ddlChange(ddl) {
    dateTime = null;
    drawDashboard($(ddl).val());
};
   