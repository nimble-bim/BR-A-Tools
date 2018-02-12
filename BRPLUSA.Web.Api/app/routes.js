const express = require('express');
const router = express.Router();
const HomeController = require('./controller/home.controller');
const WorksharingController = require('./controller/worksharing.controller');

/* GET home page. */
router.get('/', HomeController.showHome);

router.get('/api/worksharing', WorksharingController.getEvents);
router.post('/api/worksharing', WorksharingController.saveEvent);

module.exports = router;
