var instructor = {
    firstName:"Kevin",
    sayHi:function() {
        setTimeout( () => {
            console.log(this.firstName);
        },1000);
    }
}