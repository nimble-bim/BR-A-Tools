const express = require('express');
const router = express.Router();
const HomeController = require('./controller/home.controller');
const WorksharingController = require('./controller/worksharing.controller');

/* GET home page. */
router.get('/', HomeController.showHome(req, res, next));

router.get('/api/worksharing', WorksharingController.getEvents(req, res, next));
router.post('/api/worksharing', WorksharingController.saveEvent(req, res, next));

module.exports = router;
