function HomeController() {

}

HomeController.prototype.showHome = (req, res, next) => {
  res.send("i'm the app! Coming from the homecontroller");
}

module.exports = new HomeController();
