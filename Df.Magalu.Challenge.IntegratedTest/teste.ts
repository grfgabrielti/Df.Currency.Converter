console.log("************************ get object redis *********************")
var redis = require('redis');
var client = redis.createClient();

client.on('connect', function() {
    console.log('Redis client connected');
});

client.on('error', function (err: string) {
    console.log('Something went wrong ' + err);
});

var result = client.get('ConversorDeMoedasPortfolioMock');
console.log(result);