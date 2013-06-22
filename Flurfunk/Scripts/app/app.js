var flurfunk = angular.module('flurfunk', ['infinite-scroll']);

app.convert = {
    date: function (jsonTime)
    {
        // json time -> /Date(1370804916504)/
        return new Date(parseInt(jsonTime.substr(6)));
    }    
}

flurfunk.directive('eatClick', function () {
    return function (scope, element, attrs) {
        $(element).click(function (event) {
            event.preventDefault();
            return false;
        });
    }
})
