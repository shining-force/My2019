var express = require("express"),
    app = express(),
    port = 80;

app.use(express.static(__dirname + "/public"));
app.use(express.static(__dirname + "/views"));

app.get("/",function (req,res) {
    res.sendFile("index.html");
});

app.listen(port,function () {
   console.log("running");
});