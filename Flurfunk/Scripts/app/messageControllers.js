flurfunk.factory('messageService', function ($rootScope, $timeout) {
    var messageService = {};

    messageService.filterKeyword = "";
    messageService.groupId = "";
    messageService.groupName = "";

    messageService.onRefresh = function () {
        console.log("onRefresh");
        messageService.loadNewMessages();
        messageService.refreshTimer = $timeout(messageService.onRefresh, 5000);
    }
    messageService.refreshTimer = $timeout(messageService.onRefresh, 5000);

    messageService.loadNewMessages = function () {
        $rootScope.$broadcast('loadNewMessages');
    };

    messageService.setKeyword = function (keyword) {        
        messageService.filterKeyword = keyword;
        $rootScope.$broadcast('filterMessages');
    };

    messageService.setGroup = function (groupId, groupName) {
        messageService.groupId = groupId;
        messageService.groupName = groupName;
        $rootScope.$broadcast('groupChanged');
        $rootScope.$broadcast('filterMessages');
    };

    return messageService;
});

function sendMessageController($scope,$http, messageService) {
    $scope.groupName = "allen";
    $scope.sendMessage = function () {
        if ($scope.message.length > 140 || $scope.message.length < 1)
            return;
        console.log("new message submited");

        $http.post(app.baseUrl + "message/create", { text: $scope.message, groupId : messageService.groupId }).success(function () {
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

    $scope.$on('groupChanged', function () {
        if (messageService.groupName.length > 0)
        {
            $scope.groupName = messageService.groupName;
        }
        else
        {
            $scope.groupName = "allen";
        }
    });
}

function isMessagesInArray(id, messageArray)
{
    for (var i = 0; i < messageArray.length; i++) {
        if (messageArray[i]._id == id)
            return true;
    }
    return false;
}

function loadMessageController($scope, $http, messageService) {
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
        if ($scope.busy || $scope.end) return;
        $scope.busy = true;        

        var url = app.baseUrl + "message/GetOlderThan";
        console.log("Loading Messages Older Than" + $scope.after);
        $http.post(url, { count: 50, time: $scope.after, 'keyword': messageService.filterKeyword, "groupId":messageService.groupId }).success(function (data) {
            if (data.length === 0)
                $scope.end = true;          

            for (var i = 0; i < data.length; i++) {                
                    $scope.items.push(data[i]);
            }
            if ($scope.items[$scope.items.length - 1] != undefined)
            {
                $scope.after = $scope.items[$scope.items.length - 1].Created;
            }
            $scope.busy = false;   
        });
    };

    $scope.$on('loadNewMessages', function () {        
        var time
        //if there are no last messages.. 
        if ($scope.items[0] === undefined) 
        {
            //give me all message that have been postet within the last minute
            time ="/Date(" + (new Date().getTime() - 60000)+")/";
        }
        else
        {
            time = $scope.items[0].Created;
        }
        console.log("fetching new messages newer than " + app.convert.date(time));

        var url = app.baseUrl + "message/GetNewerThan";
        $http.post(url, { 'time': time, 'keyword': messageService.filterKeyword, "groupId": messageService.groupId }).success(function (data) {
            for (var i = 0; i < data.length; i++) {
                if (!isMessagesInArray(data[i]._id, $scope.items))
                    $scope.items.splice(i, 0, data[i]);
            }                       
        });
    });

    $scope.$on('filterMessages', function () {
        console.log("load new filtered messages");
        console.log($scope.items);
        //reset status 
        $scope.end = false;
        $scope.after = '';
        $scope.items = [];
        //and load filtered messages
        $scope.nextPage();
        
    });

}

