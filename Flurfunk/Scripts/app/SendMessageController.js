function sendMessageController($scope) {

    $scope.sendMessage = function () {
        if ($scope.message.length > 140 || $scope.message.length < 1)
            return;
        console.log("new message submited");
        app.ajax.request("message/create", { text: $scope.message }, function () {
            console.log("message successful submited" + $scope.message )
            $scope.message = '';
        })
    };

    $scope.messageButtonClass = "primary",
    $scope.messageUpdate = function () {
        if ($scope.message.length > 140)
        {
            $scope.messageButtonClass = "danger";
        }
        else
        {
            $scope.messageButtonClass = "primary";
        }
    }
}

function loadMessageController($scope, $http) {

    $scope.loadMessages = function () {
        console.log("load messages");
        app.ajax.request("message/get", {}, function (result) {
            console.log("message successful loaded" + result)
            
        })
    };
      
    $scope.items = [];
    $scope.busy = false;
    $scope.lastIndex = '';

    $scope.nextPage = function () {
        if ($scope.busy) return;
        $scope.busy = true;

        //app.ajax.request("message/get", {}, function (result) {
        //    console.log(result);
        //    for (var i = 0; i < result.length; i++) {
        //        console.log(i);
        //        $scope.items.push(result[i]);
        //    }
        //    console.log($scope.items);
        //    $scope.after = "t3_" + result.length;
        //    $scope.busy = false;
        //});

        var url = app.baseUrl + "message/get";
        $http.post(url).success(function (data) {           
            for (var i = 0; i < data.length; i++) {
                $scope.items.push(data[i]);
            }
            $scope.after = "t3_" + data.length;
            $scope.busy = false;   
        });
    };
}

