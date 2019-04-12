var express = require("express"),
	app = express(),
	env_port = 80;

var todoRoutes = require("./routes/todos"); //导入Router选择路径
var bodyparser = require("body-parser");

app.use(bodyparser.json());
app.use(bodyparser.urlencoded({extended:true}));

app.get('/',function(req,res){
	res.send("Hi there,from root routes!");

});
app.use('/api/todos',todoRoutes); //Router复杂路径选择功能

	
app.listen(env_port,function(){
	console.log("System is running on " + env_port);
});