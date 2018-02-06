const mongoose = require('mongoose');
// const timestamps = require('mongoose-timestamp');

const wmSchema = 
  new mongoose.Schema({
    id: Number,
    event_type: String,
    model_name: String,
    time_created: String,
  }, 
  { collection: 'worksharing_events'}
);

// create the model
// specify its db
const wsModel = mongoose.model('WorksharingEvents', wmSchema);

// adding a little test
// export the model
module.exports = wsModel;