/// <reference path="../js/jquery-1.9.1.min.js"; />
/// <reference path="../js/knockout-2.2.1.js"; />
/// <reference path="../js/dxtreme.core-12.2.js"; />

(function() {

    var products = ["Chai", "Chang", "Aniseed Syrup", "Chef Anton's Cajun Seasoning", "Chef Anton's Gumbo Mix", "Grandma's Boysenberry Spread",
                   "Ikura", "Queso Cabrales", "Queso Manchego La Pastora", "Konbu", "Tofu", "Genen Shouyu", "Pavlova", "Alice Mutton", "Carnarvon Tigers",
                   "Teatime Chocolate Biscuits"];
    var productsCustom = [
            { Name: products[0], Price: 0.99, Image: "content/images/01.jpg" },
            { Name: products[1], Price: 1.19, Image: "content/images/02.jpg" },
            { Name: products[2], Price: 2.19, Image: "content/images/03.jpg" },
            { Name: products[3], Price: 0.39, Image: "content/images/04.jpg" },
            { Name: products[4], Price: 0.75, Image: "content/images/05.jpg" },
            { Name: products[5], Price: 1.25, Image: "content/images/06.jpg" },
            { Name: products[6], Price: 1.55, Image: "content/images/07.jpg" },
            { Name: products[7], Price: 2.55, Image: "content/images/08.jpg" },
            { Name: products[8], Price: 2.65, Image: "content/images/09.jpg" },
            { Name: products[9], Price: 2.35, Image: "content/images/10.jpg" },
            { Name: products[10], Price: 1.65, Image: "content/images/11.jpg" },
            { Name: products[11], Price: 2.05, Image: "content/images/12.jpg" },
            { Name: products[12], Price: 2.85, Image: "content/images/13.jpg" },
            { Name: products[13], Price: 0.85, Image: "content/images/14.jpg" },
            { Name: products[14], Price: 1.85, Image: "content/images/15.jpg" },
            { Name: products[15], Price: 2.15, Image: "content/images/16.jpg" }
    ];
    var productsGrouped = [
        {
            key: "Group 1",
            items: products.slice(0, 5)
        },
        {
            key: "Group 2",
            items: products.slice(6, 10)
        },
        {
            key: "Group 3",
            items: products.slice(11, 15)
        }
    ];

    KitchenSink.db = {
        products: products,
        productsCustom: productsCustom,
        productsGrouped: productsGrouped
    };
})();