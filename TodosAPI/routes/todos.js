var express = require("express"),
    router = express.Router();

var db = require("../models");


router.get("/hi",function (req,res) {
    db.Todo.find().then(function (todos) {
        res.json(todos);
    }).catch(function (err) {
        res.send(err);
    })
});


module.exports = router;
