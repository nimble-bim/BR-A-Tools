function HomeController() {

}

// should add to submodule
HomeController.prototype.showHome = (req, res, next) => {
  res.render('index', {title: "i'm the app! Coming from the homecontroller"});
}

module.exports = new HomeController();
