var mongoose = require("mongoose");
var todoSchema = new mongoose.Schema(  //数据库结构
    {
        name:{
            type:String,
            required:"this field can't be empty"
        },
        complete:{
            type:Boolean,
            default:false
        },
        created_date:{
            type:Date,
            default: Date.now
        }
    }
);

var Todo = mongoose.model("Todo"/* model 名称*/,todoSchema/* Schema实例 */); //产生Model
module.exports =  Todo; //导出model