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

console.log("first")

setInterval(function () {
    console.log("second");
},0)