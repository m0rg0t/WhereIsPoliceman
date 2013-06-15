KitchenSink.Gallery = function(params) {

    var viewModel = {
        data: new DevExpress.data.ArrayStore([
            { src: "content/images/gallery/boats.jpg" },
            { src: "content/images/gallery/cherry.jpg" },
            { src: "content/images/gallery/country.jpg" },
            { src: "content/images/gallery/field.jpg" },
            { src: "content/images/gallery/headphone.jpg" },
            { src: "content/images/gallery/keyboard.jpg" },
            { src: "content/images/gallery/ny.jpg" },
            { src: "content/images/gallery/orange.jpg" },
            { src: "content/images/gallery/palms.jpg" },
            { src: "content/images/gallery/sunrise.jpg" },
            { src: "content/images/gallery/sunset.jpg" },
            { src: "content/images/gallery/town.jpg" }
        ])
    };

    return viewModel;
};