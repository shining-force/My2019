var express = require('express'),
    app = express(),
    path = require('path'),
    port = process.env.PORT || 80;

app.get("/",(req,res)=>{
  res.sendfile(path.join(__dirname,"my-app/build","index.html",));
});

app.listen(port,function () {
    console.log("System running");
});
