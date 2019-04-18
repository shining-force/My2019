$(document).ready(function () {
    fetch("/api/todos").then(function (data) {
        return data.json();
    }).then(function (info) {
        info.forEach(function (obj) {
            addTodo(obj);
        })
    }).catch(function (err) {
        console.log(err);
    });

    $('#todoInput').keypress(function (event /*ascii code number*/) {
        if(event.which == 13)
        {
            var usrInput = $('#todoInput').val();
            fetch("/api/todos",
                {method:"POST",
                    headers:{
                    "Content-Type":"application/x-www-form-urlencoded" //必须与body内容一致
                    },
                    body:"name=" + usrInput //如果Content-Type是application/json则为{name:usrInput}
            }
            ).then(function (newTodo) {
                return newTodo.json();
            }).then(function (data) {
                addTodo(data);
            }).catch(function (err) {
                console.log(err);
            })
        }
    });

    $(".list").on("click","li",function () {
        var thisTodo = $(this);

        fetch("/api/todos/" + thisTodo.data("id"),
            {method:"PUT",
                headers:{
                "Content-Type":"application/x-www-form-urlencoded"
                },
                body:"complete=" + !thisTodo.data("complete")
            }).then(function () {
                thisTodo.data("complete",!thisTodo.data("complete"));
                thisTodo.toggleClass("done");
        }).catch(function (err) {
            console.log(err);
        })
});

    $(".list").on("click","span",function (e) {
        e.stopPropagation(); //停止消息向下冒泡，使得消息传输到一下控件
        var thisTodo = $(this).parent();
        fetch("/api/todos/" + thisTodo.data("id"),
            {method:"DELETE",}).then(function (data) {
            thisTodo.remove();
        });
    })
});

function addTodo(todo) {
    var newTodo = $('<li>' + todo.name + '<span>x</span> </li>');
    newTodo.data("id",todo._id);
    if(!todo.complete)
    {
        newTodo.data("complete",false);
    }
    else
    {
        newTodo.data("complete",true);
        newTodo.addClass('done');
    }
    $(".list").append(newTodo);
    $('#todoInput').val('');
}