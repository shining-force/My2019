var express = require('express'),
    path = require('path'),
    app = express(),
    port = process.env.PORT || 80;

app.use(express.static(path.join(__dirname, 'my-app/build')));

// app.use(express.static(__dirname + '/public'));
// app.use(express.static(__dirname + '/views'));

app.get('/',(req,res)=>{
    res.sendFile(path.join(__dirname, 'my-app/build', 'index.html'));
});

app.listen(port,()=>{
    console.log("Server started");
});