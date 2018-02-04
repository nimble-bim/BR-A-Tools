'use strict';
var express = require('express');
var router = express.Router();

/* GET users listing. */
router.get('/', function (req, res) {
    res.send('respond with a resource');
});

router.post('/', function (req, res) {
    console.log('recevied a resource');
    console.log(req.body);
    res.send('received a resource');
});

module.exports = router;
