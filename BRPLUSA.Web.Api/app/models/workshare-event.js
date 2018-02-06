const mongoose = require('mongoose');
// const timestamps = require('mongoose-timestamp');

const wmEvent = 
  new mongoose.Schema({
    id: Number,
    event_type: String,
    model_name: String,
    time_created: String,
      
  }, 
  { collection: 'worksharing_events'}
);

  module.exports = exports = mongoose.model('WorksharingEvent', wmEvent);