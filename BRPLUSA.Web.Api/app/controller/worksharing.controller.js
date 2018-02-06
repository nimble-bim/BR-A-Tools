const wmEvent = require('../models/workshare-event');

function WorksharingController() {

}

WorksharingController.prototype.saveEvent = function(req, res, next) {
  const elems = this.processElements(req.body);

  for(e of elems) {
    var newEvent = new wmEvent(e);
    newEvent.save();
  }
}

WorksharingController.prototype.getEvents = function(req, res, next) {
  // get all events from server
}

WorksharingController.prototype.getEventById = function(req, res, next) {
  // get a specific event from the server by Id
}

WorksharingController.prototype.processElements = function(obj) {
  // turn body json into usable db element
  // return array of elements
}

module.exports = new WorksharingController();
