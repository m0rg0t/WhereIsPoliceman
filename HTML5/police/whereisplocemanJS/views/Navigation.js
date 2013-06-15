KitchenSink.Navigation = function(params) {
    var viewModel = {
        toolbar: {
            items: [
              {
                  align: "left",
                  widget: "button",
                  options: {
                      type: "back",
                      text: "Back",
                      clickAction: function(e) {
                            KitchenSink.app.navigate("Home");
                      }
                  }
              },
              {
                  align: "right",
                  widget: "button",
                  options: {
                      icon: "plus",
                      clickAction: function(e) {
                          console.log("plus click")
                      }
                  }
              },
              {
                  align: "right",
                  widget: "dropDownMenu",
                  options: {
                      items: [
                          "Add",
                          "Edit",
                          "Remove"
                      ]
                  }
              },
              {
                  align: "center",
                  text: "Toolbar"
              }
            ]
        },
        navbar: {
            currentImage: ko.observable("./content/images/navigation/icon-home.png"),
            itemClickAction: function(e) {
                this.navbar.currentImage(e.itemData.image);
            },
            items: [
                { text: "Home", icon: "home", image: "./content/images/navigation/icon-home.png" },
                { text: "User", icon: "user", image: "./content/images/navigation/icon-user.png" },
                { text: "Comment", icon: "comment", image: "./content/images/navigation/icon-message.png" },
                { text: "Photo", icon: "photo", image: "./content/images/navigation/icon-image.png" }
            ],
            selectedIndex: ko.observable(0)
        }
    };
    return viewModel;
};