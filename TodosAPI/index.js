var express = require("express"),
	app = express(),
	env_port = 80;

var todoRoutes = require("./routes/todos"); //导入Router选择路径
	
app.get('/',function(req,res){
	res.send("Hi there,from root routes!");

});

app.use('/api/test',todoRoutes); //Router复杂路径选择功能
	
app.listen(env_port,function(){
	console.log("System is running on " + env_port);
});