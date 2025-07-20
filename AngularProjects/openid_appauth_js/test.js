

// case1 :  callback
var users1 = ["dhiren", "Sunil"];

function addUser(username, callback){
    // setTimeout is a asynchronous function. It does not pause execution for outer code.
    setTimeout(() =>{
        console.log( "###### case 1: #######")
        users1.push(username);
        callback(users1); 
    }, 2000);
    callback(users1);  // this get called first, instead of the callback from inside setTimeout.
};

function getUsers(users){
        console.log(users);
}

addUser("Rohit", getUsers);

// case2: Promise
var users2 = ["dhiren", "Sunil"];

function addUser2(username){
    let promise = new Promise((resolve,reject)=>{
        setTimeout(()=>{
            console.log( "###### case 2: #######")
            users2.push(username);
            resolve(users2)
        },2000)
    });
    return promise;
};

addUser2("Rohit").then( (result)=>{
    getUsers(result);
})


// case3: promise with async await
var users3 = ["dhiren", "Sunil"];


async function addUser3(username){
    // let response = await fetch('/article/promise-chaining/user.json');
    let promise =await new Promise((resolve,reject)=>{
        setTimeout(()=>{
            console.log( "###### case 3: #######")
            users3.push(username);
            resolve(users3)
        },2000);
    });
    // here we put getUsers outside the setTimeout and it still works, because now it is waiting.
    getUsers(users3);
};

addUser3("Rohit"); // getUsers is added inside the async
getUsers(users3);  // this get called first

// case4: rxjs observable
const { Observable } = require("rxjs");
var users4 = ["dhiren", "Sunil"];
function addUser4(username){
    const observable = new Observable((subscriber)=>{
        setTimeout(()=>{
            console.log( "###### case 4: #######")
            users4.push(username);
            subscriber.next(users4)          
        },2000);
    });
    return observable;
    }


const observable = addUser4("Rohit");
observable.subscribe({
    next: (value)=>{
        console.log(value);
    },
    complete: () =>{
        console.log("end")
    }
})


/* [Running] node "/home/dhiren/scratch/oauthclient/oauthdemo/test.js"
[ 'dhiren', 'Sunil' ]
[ 'dhiren', 'Sunil' ]
###### case 1: #######
[ 'dhiren', 'Sunil', 'Rohit' ]
###### case 2: #######
[ 'dhiren', 'Sunil', 'Rohit' ]
###### case 3: #######
[ 'dhiren', 'Sunil', 'Rohit' ]
###### case 4: #######
[ 'dhiren', 'Sunil', 'Rohit' ]

[Done] exited with code=0 in 2.065 seconds */