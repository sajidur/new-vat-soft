var app = angular.module('pos', ['ngRoute', 'ui.bootstrap', 'ngCookies']);

app.filter('rawHtml', ['$sce', function($sce){
	  return function(val) {
		return $sce.trustAsHtml(val);
	  };
}]);

// configure our routes
app.config(function($routeProvider) {
	$routeProvider		
	.when('/', {
		templateUrl : 'home.html',
		controller  : 'homectrl'
	})
	
	.when('/login', {
		templateUrl : 'login.html',
		controller  : 'loginctrl'
	})

	.when('/detail/:property_id', {
		templateUrl : 'page/detail.html',
		controller  : 'detailctrl'
	})

	.when('/search', {
		templateUrl : 'page/search.html',
		controller  : 'searchctrl'
	})

	.when('/register', {
		templateUrl : 'page/register.html',
		controller  : 'registerctrl'	
	})
	
	.when('/post', {
		templateUrl : 'page/posting.html',
		controller  : 'postctrl'	
	})
	
	.otherwise({
        redirectTo: '/'
    });
});
	
app.filter('startFrom', function() {
return function(input, start) {
	if(input) {
		start = +start; //parse to int
		return input.slice(start);
	}
	return [];
}
});

app.factory('Scopes', function ($rootScope) {
	var mem = {};
 
	return {
		store: function (key, value) {
			$rootScope.$emit('scope.stored', key);
			mem[key] = value;
		},
		get: function (key) {
			return mem[key];
		}
	};
});


app.run(function ($rootScope, $timeout, $http, filterFilter, $cookieStore, $cookies) {
	
	$rootScope.domainname="http://myanmarhousing.com.mm";
	$rootScope.searchpara=[];
	var expireDate = new Date();
		
	expireDate.setMinutes(expireDate.getMinutes() + 1);			// 1 day long time
	
	if(localStorage.getItem('Logindatas')){	
		console.log(localStorage.getItem('Logindatas'));
		$rootScope.Logindatas=localStorage.getItem('Logindatas');	
		$rootScope.logininfo=false;
		$rootScope.logoutinfo=true;
		//localStorage.removeItem(key);
	}else{
		$rootScope.Logindatas=[];
		$rootScope.logininfo=true;
		$rootScope.logoutinfo=false;
	}
	
	//$rootScope.Logindatas
	
	$rootScope.logout = function(){
		localStorage.removeItem('Logindatas');
		location.reload();
	}
	
	if(localStorage.getItem('sellan')){			//if($cookies.get('sellan')){
		
		//alert(localStorage.getItem('sellan'));
		//alert("local storage data has");
		
		$rootScope.sellan=localStorage.getItem('sellan');		//$rootScope.sellan=$cookies.get('sellan');
		if($rootScope.sellan=='mm'){
			$rootScope.mmshow=true;
		}else{
			$rootScope.enshow=true;
		}
		
	}else{
		//alert("local storage no data");
		localStorage.setItem('sellan', 'en');		//$cookies.put('sellan', 'en', {'expires': expireDate});
		window.location.reload();
		
	}
	
	
	
	$rootScope.babylay=true;
	
	$rootScope.$on('$routeChangeStart', function() {
		$('#mainview').hide();
		//$rootScope.footer = false;
	});
	$rootScope.$on('$routeChangeSuccess', function() {
		//$('#mainview').fadeIn( "fast", function() {
			
		//});
		setTimeout(function () {
			$('#mainview').fadeIn();
			$('#cbp-spmenu-s2').removeClass('cbp-spmenu-open');
			$('body').removeClass('cbp-spmenu-push-toleft');
		}, 100);
		//$rootScope.footer = true;
	});

	$rootScope.customvalidate = function(){
		var err=false;
		$('.cusvalidate').each(function(){

			var oneerr=false;

			if($(this).val()==""){
				$(this).css("border-color", "red");
				$(this).closest('div').find('.err').html($(this).attr('data-val-required'));
				err=true;
				oneerr=true;
			} 
			if($(this).attr('data-val-email')){
				
			    var email = $(this).val();
				var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/igm;
				if (re.test(email)) {
					
					
				} else {
					$(this).css("border-color", "red");
					$(this).closest('div').find('.err').html($(this).attr('data-val-email'));
				    err=true;
				    oneerr=true;
				}

			}

			if($(this).attr('data-val-count')){
				var count=$(this).attr('data-val-count');
			    var countval = $(this).val();
				
				if(countval.length<count){
					$(this).css("border-color", "red");
					$(this).closest('div').find('.err').html("This field must be at least "+count+" characters");
					oneerr=true;
					err=true;	
				}
				
			}

			if($(this).attr('data-num')){
				res=!isNaN(parseFloat($(this).val())) && isFinite($(this).val());
				if(res==false){
					$(this).css("border-color", "red");
					$(this).closest('div').find('.err').html("This field must be only number");
					oneerr=true;
					err=true;	
				}
			}	
			

			if(oneerr==false){				
				$(this).css("border-color", "");
				$(this).closest('div').find('.err').html('');	
			}
		});
		return err;
	}

	$rootScope.customvalidateforpost = function(idname){
		var err=false;
		$('#'+idname+' .cusvalidate').each(function(){

			var oneerr=false;

			if($(this).val()==""){
				$(this).css("border-color", "red");
				$(this).closest('div').find('.err').html($(this).attr('data-val-required'));
				err=true;
				oneerr=true;
			} 
			if($(this).attr('data-val-email')){
				
			    var email = $(this).val();
				var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/igm;
				if (re.test(email)) {
					
					
				} else {
					$(this).css("border-color", "red");
					$(this).closest('div').find('.err').html($(this).attr('data-val-email'));
				    err=true;
				    oneerr=true;
				}

			}

			if($(this).attr('data-val-count')){
				var count=$(this).attr('data-val-count');
			    var countval = $(this).val();
				
				if(countval.length<count){
					$(this).css("border-color", "red");
					$(this).closest('div').find('.err').html("This field must be at least "+count+" characters");
					oneerr=true;
					err=true;	
				}
				
			}

			if($(this).attr('data-num')){
				res=!isNaN(parseFloat($(this).val())) && isFinite($(this).val());
				if(res==false){
					$(this).css("border-color", "red");
					$(this).closest('div').find('.err').html("This field must be only number");
					oneerr=true;
					err=true;	
				}
			}	
			

			if(oneerr==false){				
				$(this).css("border-color", "");
				$(this).closest('div').find('.err').html('');	
			}
		});
		return err;
	}
});



function sleep(milliseconds) {
  var start = new Date().getTime();
  for (var i = 0; i < 1e7; i++) {
    if ((new Date().getTime() - start) > milliseconds){
      break;
    }
  }
}

app.controller("rootcontroller", ['$scope', '$cookies', '$cookieStore', '$window', function($scope, $cookies, $cookieStore, $window) {
	
	$scope.changelan = function(lan){
		
		var expireDate = new Date();
		expireDate.setMinutes(expireDate.getMinutes() + 1);			// 1 minute long time
		
		localStorage.setItem('sellan', lan);			//$cookies.put('sellan', lan, {'expires': expireDate});
		window.location.reload();
		
		/*
		  // Put cookie
		  $cookieStore.put('myFavorite','oatmeal');
		  // Get cookie
		  var favoriteCookie = $cookieStore.get('myFavorite');
		  // Removing a cookie
		  $cookieStore.remove('myFavorite');
		*/
	}
	
	$scope.set_cookie = function(s, lan) {
		var secs = parseInt(s);
		if (isNaN(secs)) {
			secs = 5;
		}
		var now = new Date();
		var exp = new Date(now.getTime() + secs*1000);
		var status = '';
		document.cookie = 'ExpirationCookieTest=1; expires='+exp.toUTCString();
		// if (document.cookie && document.cookie.indexOf('ExpirationCookieTest=1') != -1) {
			// status = 'Cookie successfully set. Expiration in '+secs+' seconds';
		// } else {
			// status = 'Cookie NOT set. Please make sure your browser is accepting cookies';
		// }

		// $('#cookiestatus').text(status);
	}

	$scope.get_cookie = function() {
		var status = '';
		if (document.cookie && document.cookie.indexOf('ExpirationCookieTest=1') != -1) {
			status = 'Cookie is present';
		} else {
			status = 'Cookie is NOT present. It may be expired, or never set';
		}
		$('#cookiestatus').text(status);
	}
	
}]);

	
	


