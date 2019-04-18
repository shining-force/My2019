var express = require("express"),
	app = express(),
	env_port = process.env.PORT || 80;

var todoRoutes = require("./routes/todos"); //导入Router选择路径
var bodyparser = require("body-parser");

app.use(bodyparser.json()); //html解析json
app.use(bodyparser.urlencoded({extended:true})); //url 添加编码
app.use(express.static(__dirname + "/public")); //添加静态文件位置 用于存放css文件
app.use(express.static(__dirname + '/views'));  //添加静态文件位置 用于存放html文件

app.get('/',function(req,res){
	// res.send("Hi there,from root routes!");
	res.sendfile('index.html');
});

app.get('/check',function (req,res) {
    res.send('test');
});

app.use('/api/todos',todoRoutes); //Router复杂路径选择功能


	
app.listen(env_port,function(){
	console.log("System is running on " + env_port);
});