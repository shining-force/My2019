var x = new Promise(function (resolve,reject) {
    if(a===1)
    {resolve(1);}
    else
    {reject(2);}
}).then(function (data) {
    console.log(data);
}).catch(function (data) {
    console.log(data);
})