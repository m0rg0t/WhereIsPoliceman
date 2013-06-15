KitchenSink.Lists = function(params) { 
    var lists = [
        {
            items: KitchenSink.db.products,
            grouped: ko.observable(false),
            customTemplate: ko.observable(false),
            showSearchField: ko.observable(false)
        },
        {
            items: KitchenSink.db.productsGrouped,
            grouped: ko.observable(true),
            customTemplate: ko.observable(false),
            showSearchField: ko.observable(false)
        },
        {
            items: ko.observable(KitchenSink.db.productsCustom),
            grouped: ko.observable(false),
            customTemplate: ko.observable(true),
            showSearchField: ko.observable(true),
            searchQuery: ko.observable().extend({ throttle: 500 })
        }
    ];

    /*console.log(params.city);
    console.log(params.street);*/
    var title = "Поиск по адресу";
    if (params.city != "" && params.street != "") {
        title = params.city + ", " + params.street;
		
		
        //function onDeviceReady() {
            $.get('http://api.openpolice.ru/api/v1/refbook/Copfinder/place=' + params.city + '&page=1&perpage=10&street=' + params.street, function (data) {
                alert(data);
            });
        //}
        //$(document).ready(function () {
        //    document.addEventListener("deviceready", onDeviceReady, true);
        //});

    /*$.jsonp({
        url: 'http://api.openpolice.ru/api/v1/refbook/Copfinder/place=' + params.city + '&page=1&perpage=10&street=' + params.street,
        callbackParameter: 'callback',
        success: function (data, status) {
            alert(status);
            alert(data);
            //$('#your-tweets').append('<li>The feed loads fine');
            $.each(data, function(i,item){ 
                var tweet = item.text;
                //$('#your-tweets').append('<li>'+tweet);
            });
        },
        error: function () {
            alert('wrong');
            //$('#your-tweets').append('<li>There was an error loading the feed');
        }
    });*/

        /*$.ajax({
            url: 'http://api.openpolice.ru/api/v1/refbook/Copfinder/place=' + params.city + '&page=1&perpage=10&street=' + params.street,
            success: function (data) {
                alert(data);
                console.log(data);
                console.dir(data);
                //it works, do something with the data
            },
            error: function () {
                alert("wrong");
                console.los("wrong");
                //something went wrong, handle the error and display a message
            }
        });*/
    };

    var viewModel = {
        city: params.city,
        street: params.street,
        title: title,
        tabs: [
            { text: "Simple" },
            { text: "Grouped" },
            { text: title }
        ],
        selectedTab: ko.observable(0),
        tabContent: ko.observable()
    };

    lists[2].searchQuery.subscribe(function(value) {
        var result = $.grep(KitchenSink.db.productsCustom, function(product, index) {
            var regExp = new RegExp(value, "i");
            return !!product.Name.match(regExp);
        });
        lists[2].items(result);
    });
    ko.computed(function() {
        viewModel.tabContent(lists[viewModel.selectedTab()]);
    });
    
    return viewModel;
};