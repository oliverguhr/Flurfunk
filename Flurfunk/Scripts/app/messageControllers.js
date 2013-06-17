flurfunk.factory('messageService', function ($rootScope, $timeout) {
    var messageService = {};

    messageService.onRefresh = function () {
        console.log("onRefresh");
        messageService.loadNewMessages();
        refreshTimer = $timeout(messageService.onRefresh, 5000);
    }
    var refreshTimer = $timeout(messageService.onRefresh, 5000);


    messageService.loadNewMessages = function () {
        $rootScope.$broadcast('loadNewMessages');
    };

    return messageService;
});

function sendMessageController($scope,$http, messageService) {

    $scope.sendMessage = function () {
        if ($scope.message.length > 140 || $scope.message.length < 1)
            return;
        console.log("new message submited");

        $http.post(app.baseUrl + "message/create", { text: $scope.message }).success(function () {
            console.log("message successful submited")
            $scope.message = '';            
            messageService.loadNewMessages();
        });        
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

function isMessagesInArray(id, messageArray)
{
    for (var i = 0; i < messageArray.length; i++) {
        if (messageArray[i]._id == id)
            return true;
    }
    return false;
}

function loadMessageController($scope, $http) {
    $scope.items = [];
    $scope.busy = false;
    $scope.end = false;

    $scope.getTimeString = function (timestamp) {
        return app.convert.date(timestamp).toLocaleString();
    }

    $scope.getUserImageUrl = function (userProviderId) {
        return "http://graph.facebook.com/" + userProviderId + "/picture?type=normal";;
    }

    $scope.nextPage = function () {
        if ($scope.busy) return;
        $scope.busy = true;

        var url = app.baseUrl + "message/GetOlderThan";
        console.log("Loading Messages Older Than" + $scope.after);
        $http.post(url, { count: 50, time: $scope.after }).success(function (data) {
            if (data.length === 0)
                $scope.end = true;

            if ($scope.end)
                return;

            for (var i = 0; i < data.length; i++) {                
                    $scope.items.push(data[i]);
            }           
            $scope.after = $scope.items[$scope.items.length - 1].Created;
            $scope.busy = false;   
        });
    };

    $scope.$on('loadNewMessages', function () {
        console.log("fetching new messages:" + $scope.items[0].Created);
        var url = app.baseUrl + "message/GetNewerThan";
        $http.post(url, { time: $scope.items[0].Created }).success(function (data) {
            for (var i = 0; i < data.length; i++) {
                if (!isMessagesInArray(data[i]._id, $scope.items))
                    $scope.items.splice(i, 0, data[i]);
            }                       
        });
    });

}

