// var firstName = prompt("what is your first name?");
// var lastName = prompt("what is your last name?");
// var _age = prompt("how old are you?");
// console.log("Nice to meet you " + firstName + " " + lastName);
// console.log("you are " + _age + " years old ");

// const EMPTY_STRING = "";
// var info = document.getElementById("information");
// var infoSelector = document.querySelector("#information");
// var infoSelectorAll = document.querySelectorAll(".special")[0];
// var infoClassName = document.getElementsByClassName("special")[0];
// var infoTagName = document.getElementsByTagName("p")[0];


var myButton = document.querySelector("#myButton");
// myButton.classList.add("purple");

if(myButton === null)
    alert("obj not found");
else
{
    myButton.addEventListener("click",function () {
        document.body.classList.toggle("purple");
    });
}