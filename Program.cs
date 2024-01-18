using Connector;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

var db = new ConnectionPoint();

app.UseHttpsRedirection();

app.Use((e,next)=>{ 
    db.Database.EnsureCreated();
    return next();
    });

app.MapGet("/user",([FromBody] Userpass userData)=>
    db?.Users.Where(e=>
    e.Username == userData.username
    && e.password == userData.password).Count() > 0
);
app.MapPost("/user",([FromBody] Userpass userData)=>{
    db.Users.Add(new User{
        dateJoined= new DateTime(),
        Username = userData.username,
        password = userData.password,
    });
    db.SaveChanges();
});

app.MapPost("/card",([FromBody] RankCard data)=>{
    db.RankCard.Add(data);
});

app.MapPatch("/card",([FromBody] RankCard data)=>{
    var obj = db.RankCard.FirstOrDefault(e=>
        e.year == data.year
        && e.Ranking == data.Ranking
    );
    obj!.rank = data.rank;
});

app.MapGet("/card",(string Ranking)=>
    JsonConvert.SerializeObject(db?.RankCard.Where(e=>e.Ranking == Ranking).ToArray())
);

app.Run();