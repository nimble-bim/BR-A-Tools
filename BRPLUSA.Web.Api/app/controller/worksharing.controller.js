'use strict'

const wmEvent = require('../models/workshare-event');

var WorksharingController = function() {
  var self = this;
  this.saveEvent = saveEvent;
  this.processElements = processElements;
}

function saveEvent(req, res, next) {
  const elems = this.processElements(req.body);

  for(e of elems) {
    var newEvent = new wmEvent(e);
    newEvent.save();
  }

  res.send('Finished adding items to the database');
}

WorksharingController.prototype.getEvents = function(req, res, next) {
  // get all events from server
  res.send('Here are all the items from the db:');
}

WorksharingController.prototype.getEventById = function(req, res, next) {
  // get a specific event from the server by Id
}

function processElements(obj) {
  // turn body json into usable db element
  // return array of elements
  console.log('processing elements');
}

module.exports = new WorksharingController();
