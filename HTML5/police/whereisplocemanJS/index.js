window.whereisplocemanJS = window.whereisplocemanJS || {};

$(function() {
    whereisplocemanJS.app = new DevExpress.framework.html.HtmlApplication({
        namespace: whereisplocemanJS,
        defaultLayout: whereisplocemanJS.config.defaultLayout,
        navigation: whereisplocemanJS.config.navigation
    });
    whereisplocemanJS.app.router.register(":view/:id", { view: "home", id: undefined });

    whereisplocemanJS.app.navigate();

});
