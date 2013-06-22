function filterController($scope, $http, messageService) {
	$scope.filters = [];
	$scope.addFilter = function () {
		if ($scope.filterKeyword.length < 3)
			return;
		console.log("add new Filter");

		$http.post(app.baseUrl + "filter/add", { keyword: $scope.filterKeyword }).success(function () {
			console.log("filter successful added")
			$scope.filterKeyword = '';
			$scope.filterUpdate();
			$scope.getFilter();
			//messageService.loadNewMessages();
		});
	};

	$scope.addButtonClass = "",
	$scope.filterUpdate = function () {	    
		if ($scope.filterKeyword.length > 2) {
			$scope.addButtonClass = "btn-primary";
		}
		else {
			$scope.addButtonClass = "";
		}
	}

	$scope.getFilter = function () {	   
		$http.get(app.baseUrl + "filter/get").success(function (data) {	        
			$scope.filters = data;
			//messageService.loadNewMessages();
		});
	};

	$scope.removeFilter = function (keyword) {
	    console.log("remove " + keyword);	    
		$http.post(app.baseUrl + "filter/remove", {'keyword':keyword}).success(function () {	        
		    $scope.getFilter();		
		});
	};

	$scope.selectFilter = function (index) {
		if ($('#filter-' + index).hasClass("active")){
			messageService.setKeyword("");
			$(".filters > li").removeClass("active");
			return;
		}
		else{
			$(".filters > li").removeClass("active");
			$('#filter-' + index).addClass("active");
			var keyword = $('#filter-' + index + " .keyword").text();
			messageService.setKeyword(keyword);
		}
	};

	$scope.getFilter();
}