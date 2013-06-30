function groupController($scope, $http, messageService) {
	$scope.groups = [];
	$scope.serchResult = [];
	$scope.addGroup = function () {
		if ($scope.groupName.length < 3)
			return;
		console.log("add new group");

		$http.post(app.baseUrl + "group/create", { name: $scope.groupName }).success(function () {
		    console.log("filter successful added");
		    $scope.getGroups();
		    $scope.closeSearch();
		});
		$scope.groupName = "";
	};

	$scope.addButtonClass = "",
	$scope.groupNameUpdate = function () {
		if ($scope.groupName.length > 2) {
		    $scope.addButtonClass = "btn-primary";
		    $scope.find($scope.groupName);
		}
		else {
			$scope.addButtonClass = "";
		}
	}

	$scope.find = function (groupName) {
	    $http.post(app.baseUrl + "group/find", {keyword: groupName}).success(function (data) {
	        $scope.serchResult = data;

	        if (data.length > 0)
	        {
	            $scope.openSearch();
	        }
	        else
	        {
	            $scope.closeSearch();
	        }
		});
	};

	$scope.getGroups = function () {
	    $http.get(app.baseUrl + "group/getFromUser").success(function (data) {
	        $scope.groups = data;	        
	    });
	};

	$scope.closeSearch = function () {
	    $scope.search = "";
	};

	$scope.openSearch = function () {
	    $scope.search = "open";
	};

	$scope.join = function (id) {
	    $http.post(app.baseUrl + "group/join", {groupId: id}).success(function (data) {
	        $scope.getGroups();
	        $scope.groupName ="";
	    });
	    $scope.closeSearch();
	};

	$scope.leave = function (id) {
	    $http.post(app.baseUrl + "group/leave", { groupId: id }).success(function (data) {
	        $scope.getGroups();
	    });	    
	};

	$scope.selectGroup = function (index, groupId) {
	    if ($('#group-' + index).hasClass("active")) {
	        messageService.setGroup("","");
	        $(".groups > li").removeClass("active");
	        return;
	    }
	    else {
	        $(".groups > li").removeClass("active");
	        $('#group-' + index).addClass("active");
	        var name = $('#group-' + index + " .groupName").text();
	        messageService.setGroup(groupId, name);
	    }
	};

	$scope.getGroups();
}