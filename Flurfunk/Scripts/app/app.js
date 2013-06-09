var flurfunk = angular.module('flurfunk', ['infinite-scroll']);

app.convert = {
    date: function (jsonTime)
    {
        // json time -> /Date(1370804916504)/
        return new Date(parseInt(jsonTime.substr(6)));
    }
}