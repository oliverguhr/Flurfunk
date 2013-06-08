app.ajax = {
    request: function (serviceUrl, data, onSuccess, onError, asyncBoolean) {

        serviceUrl = app.baseUrl + serviceUrl;
        //todo: oli fix den login per ajax
        console.log("start request to:"+serviceUrl)

        if (asyncBoolean === undefined)
        { asyncBoolean = true; }
        //if (serviceUrl.indexOf('Account') === -1 && app.login.isUserLoggedIn() === false)
        //    return;
        $.ajax({
            type: "POST",
            url: serviceUrl,
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            async: asyncBoolean,
            success: function (data) {
                if (onSuccess != undefined)
                    onSuccess(data);
            },
            error: function (error) {
                if (onError != undefined) {
                    onError(error);
                }
                else {
                    alert(error);
                }
            }
        });
    }
}
