window.KitchenSink = window.KitchenSink || {};
$(function() {
    KitchenSink.app = new DevExpress.framework.html.HtmlApplication({
        namespace: KitchenSink,
        
        defaultLayout: KitchenSink.config.defaultLayout,
        navigation: KitchenSink.config.navigation
    });
    KitchenSink.app.router.register(":view/:id", { view: "Home", id: undefined });

    KitchenSink.app.router.register(":view/:city/:street", { view: "Lists", city: "Москва", street: "Арбат" });

    function showMenu() {
        KitchenSink.app.viewShown.remove(showMenu);

        if (document.location.hash !== "#Home")
            return;

        setTimeout(function() {
            $(".nav-button").click();
        }, 1000);
    }
   
    KitchenSink.app.viewShown.add(showMenu);
});
