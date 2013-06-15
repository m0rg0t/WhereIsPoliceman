KitchenSink.Form = function (params) {
    var city = ko.observable(),
        street = ko.observable();

    var viewModel = {
        city: city, 
        street: street
    };

    return viewModel;
};