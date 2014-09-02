var helper = {
    ShowLoadImage: function (lnk) {
        $(lnk).parent().find("img").eq(1).css("display", "inline");
    }
}

if (!agreementUpdater) {
    var agreementUpdater = {
        time: null,
        timeout: 10000,
        effectTime: 1000,
        agreementPanel: null,
        InitUpdater: function () {
            $("body").append("<div id='agreementpanel' class='agreement-panel'></div>");
            agreementUpdater.agreementPanel = $("#agreementpanel");
            agreementUpdater.time = agreementUpdater.GetTimeMilisecond();
            agreementUpdater.UpdateAgreement();
        },
        GetTimeMilisecond: function () {
            var now = new Date();
            return now.getTime();
        },
        UpdateAgreement: function () {
            $.ajax({
                url: "../../siteWebService.svc/GetNewAgreement",
                data: '{ "time":' + agreementUpdater.time + '}',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                processData: false,
                dataType: "json",
                success: function (data) {
                    if (data.d != null) {
                        $(agreementUpdater.agreementPanel).text("New Agreement made by user " + data.d.Agreement_ID +
                        " on " + data.d.Produktnavn + "+ " + data.d.Description + ". Price : " + data.d.Pris);
                        agreementUpdater.time = parseJsonDate(data.d.Dato_prissikring).getTime();
                        if ($(agreementUpdater.agreementPanel).css('display') == 'none') {
                            $(agreementUpdater.agreementPanel).fadeIn(agreementUpdater.effectTime, function () {
                                setTimeout(agreementUpdater.UpdateAgreement, agreementUpdater.timeout);
                            });
                        }
                        else {
                            $(agreementUpdater.agreementPanel).fadeOut(agreementUpdater.effectTime, function () {
                                $(agreementUpdater.agreementPanel).fadeIn(agreementUpdater.effectTime, function () {
                                    setTimeout(agreementUpdater.UpdateAgreement, agreementUpdater.timeout);
                                });
                            });
                        }

                    }
                    else {
                        if ($(agreementUpdater.agreementPanel).css('display') == 'block') {
                            $(agreementUpdater.agreementPanel).fadeOut(agreementUpdater.effectTime, function () {
                                setTimeout(agreementUpdater.UpdateAgreement, agreementUpdater.timeout);
                            });
                        }
                        else {
                            setTimeout(agreementUpdater.UpdateAgreement, agreementUpdater.timeout);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert(XMLHttpRequest.responseText); }               
            });
        }
    };
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

