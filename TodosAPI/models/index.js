var mongoose = require("mongoose");
mongoose.set('debug',true); //打开调试模式
mongoose.connect("mongodb+srv://DbAdmin:BvYFdbcfpDZZEsoX@advancedweb-ogzx4.azure.mongodb.net/test?retryWrites=true"); //连接数据库

mongoose.Promise = Promise; //使用Javascript Promise 语法

module.exports.Todo = require('./todo'); //导入todo模组

