// alert("connected");
//
// function callback() {
//     console.log("come from callback");
// }
//
// function higherOrder(fn) {
//     console.log("About to call callback");
//
//     fn();
//
//     console.log("Callback has been invoked");
//
//
// }
//
// //回调的实现
// function myGreeting(name,formatter){
//     alert("Hello " + formatter(name));
// }
//
// function upperCase(info) {
//     return info.toUpperCase();
// }
//
// myGreeting("Kevin",upperCase);
//
// num = 3;
// var timeId = setInterval(function () {
//     num--;
//     if(num != 0)
//     {
//         console.log("Timer:",num);
//     }
//     else{
//         console.log("Ring Ring Ring!!!");
//         clearInterval(timeId);
//     }
//
// },1000)
//

// console.log("first")
//
// setInterval(function () {
//     console.log("second");
// },0)

//JSON请求
var btn_gen_pic = document.querySelector("#btn_gen_pic");
var img_picture = document.querySelector("#img_picture");
btn_gen_pic.addEventListener("click",function () {
    var xmlHttpRequest = new XMLHttpRequest();
    xmlHttpRequest.onreadystatechange = function () {
        if(this.readyState == 4){
            var answer = this.responseText;
            var data = JSON.parse(answer);
            img_picture.setAttribute("src",data.message);
            console.log(data.message);
        }
    }

    xmlHttpRequest.open("get","https://dog.ceo/api/breeds/image/random");
    xmlHttpRequest.send();

});

fetch().then(function (res) {

}).catch(function (error) {

});

JSON.stringify